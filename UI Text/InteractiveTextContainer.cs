using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveTextContainer : MonoBehaviour
{
    [System.NonSerialized]
    public bool DialogueMode = false;
    
    public float OpenDuration = 3f;
    
    public TextBox MyTextBox;
    public GameObject BubbleOutline;
    public Image Background;
    public GameObject Continue;

    public AudioClip UpdateSoundEffect;
    public bool AllowOpenOnHover = true;

    private bool _isOpen = false;
    private Coroutine _lastCoroutine = null;
    private Animator _animator;

    private AudioManager _audioManager;

    private Thoughts _currentThoughts;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioManager>();
    }

    private void Update()
    {
        if (Continue)
            Continue.SetActive(DialogueMode);
    }
    public void MouseEnterEvent()
    {
        if(Background.color.a != 0)
        {
            if (!DialogueMode && (AllowOpenOnHover || _isOpen))
            {
                Open();
                if (_lastCoroutine != null)
                    StopCoroutine(_lastCoroutine);
            }
            if (DialogueMode)
            {
                BubbleOutline.GetComponent<Animator>().speed = 0;
            }
        }
        
        
    }

    public void MouseExitEvent()
    {
        if (Background.color.a != 0)
        {
            if (!DialogueMode)
                Close();
            if(BubbleOutline)
                BubbleOutline.GetComponent<Animator>().speed = 1;

        }
    }

    public void OnMouseUp()
    {
        if (DialogueMode)
        {
            IncrementDialogue();
        }
    }

    public void ChangeText(string thought)
    {
        if (!string.IsNullOrWhiteSpace(thought))
        {
            Open();
            MyTextBox.updateText(thought);
            _audioManager.PlaySound(UpdateSoundEffect);

        }

    }

    public void Open()
    {

        if (_isOpen)
        {
            StopClosing();
        }
        else
        {
            _animator.SetBool("Open", true);
        }

        _isOpen = true;

        if (!DialogueMode)
        {
            _lastCoroutine = StartCoroutine(WaitThenClose());

        }
    }

    public void StopClosing()
    {
        if(_lastCoroutine != null)
            StopCoroutine(_lastCoroutine);
    }
    
    IEnumerator WaitThenClose()
    {
        yield return new WaitForSeconds(OpenDuration);
        Close();
    }

    public void Close()
    {
        _animator.SetBool("Open", false);
        Debug.Log(System.Environment.StackTrace);
        _isOpen = false;
    }

    public void Think(Thoughts thoughts)
    {
        _currentThoughts = thoughts;
        DialogueMode = true;
        BubbleOutline.SetActive(true);
        IncrementDialogue();

        InteractiveObject.UniversalInteractable = false;


    }

    public void StopThinking()
    {
        DialogueMode = false;
        Close();
        _currentThoughts.Reset();

        MyTextBox.ChangeTextColor(_currentThoughts.NormalTextColor);
        Background.color = _currentThoughts.NormalBackgroundColor;
        if(_currentThoughts.NormalFont)
            MyTextBox.ChangeFont(_currentThoughts.NormalFont);

        _currentThoughts = null;
        BubbleOutline.SetActive(false);
        BubbleOutline.GetComponent<Animator>().speed = 1;


        InteractiveObject.UniversalInteractable = true;


    }

    private void IncrementDialogue()
    {
        var thought = _currentThoughts.Increment();
        if(thought != null)
        {
            if (thought.StartsWith("\\o/"))
            {
                MyTextBox.ChangeTextColor(_currentThoughts.StrangeTextColor);
                thought = thought.Substring(3);
            }
            else if (thought.StartsWith("/o\\"))
            {
                MyTextBox.ChangeTextColor(_currentThoughts.NormalTextColor);
                thought = thought.Substring(3);
            }
            else if (thought.StartsWith("~o~")){
                Background.color = _currentThoughts.StrangeBackgroundColor;
                thought = thought.Substring(3);
            }
            else if (thought.StartsWith("-o-")){
                Background.color = _currentThoughts.NormalBackgroundColor;
                thought = thought.Substring(3);
            }
            else if (thought.StartsWith("^o^"))
            {
                MyTextBox.ChangeFont(_currentThoughts.StrangeFont);
                thought = thought.Substring(3);
            }
            else if (thought.StartsWith("_o_"))
            {
                MyTextBox.ChangeFont(_currentThoughts.NormalFont);
                thought = thought.Substring(3);
            }


            ChangeText(thought);
        }
        else
        {
            StopThinking();
        }
    }

}
