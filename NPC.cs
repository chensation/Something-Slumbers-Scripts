using System;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class NPC : MonoBehaviour
{
    [Range(0,1)]
    public float RadioFrequency;
    public float Tolerance = 0.05f;
    public AudioSource RadioStatic;
    public AudioSource RadioWave;
    [Range(0, 1)]
    public float AudioMaxVolume = 1;


    [System.NonSerialized]
    public bool EnableRadioHint = true;

    private Radio _radio = null;
    private InteractiveObject _interaction;

    private bool _inFrequency = false;

    private bool _hovered = false;

    // The Yarn Program we want to load
    private DialogueManager _dialogueManager;
    public YarnProgram ScriptToLoad;
    public Sprite Portrait;
    public RuntimeAnimatorController PortraitController;


    private ButtonPromptManager _buttonPromptManager;
    private OnHoverHint _hint;

    // Start is called before the first frame update
    void Start()
    {
        _radio = GameObject.FindGameObjectWithTag("RadioUI")?.GetComponent<Radio>();
        _interaction = GetComponent<InteractiveObject>();
        _dialogueManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<DialogueManager>();
        _buttonPromptManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ButtonPromptManager>();

        _interaction.Interactive = false;
        _hint = GetComponent<OnHoverHint>();

        if(ObjectiveManager.ProgressTracker < 7 && EnableRadioHint) //not at speaking to people yet
        {
            _hint.HintText = "I don't feel like talking to people right now.";
        }

    }

    private void Update()
    {
        if (HandManager.Currently.Equals(HandManager.state.HoldingRadio))
        {
            CheckIfFrequencyAlign();
            AdjustAudio();

        }
        else
        {
            RadioWave.volume = 0;
            RadioStatic.volume = 0;

            if(ObjectiveManager.ProgressTracker >= 7 && EnableRadioHint){
                _hint.HintText = "I'll need to use my radio if I want to talk to people.";
            }
        }

    }

    private void OnMouseEnter()
    {
        if(!MoveCamera.Zooming && !MoveCamera.Panning && !DialogueManager.InDialogue)
            _hovered = true;
    }

    private void OnMouseExit()
    {
        if (!MoveCamera.Zooming && !MoveCamera.Panning)
        {
            _hovered = false;
            _buttonPromptManager.ResetInteractText();
        }

    }

    public void StartConversation()
    {
        _interaction.Interactive = false;
        _interaction.Deselect();
        _dialogueManager.StartConversation(ScriptToLoad, Portrait, PortraitController);
    }

    public void CheckIfFrequencyAlign()
    {
        if (ObjectiveManager.ProgressTracker >= 7) //can speak to people now
        {
            if (_radio.Frequency < RadioFrequency + Tolerance && _radio.Frequency > RadioFrequency - Tolerance)
            {
                _inFrequency = true;
                _hint.HintText = "";
                if (_hovered)
                {
                    _interaction.MouseEnterFunc();
                    _buttonPromptManager.ChangeInteractText("Speak");

                }
            }
            else
            {
                _inFrequency = false;
                _hint.HintText = "They can't hear me if the radio is not on the right frequency.";
                if (_interaction.Hovered)
                {
                    _interaction.Deselect();
                }
            }

            _interaction.Interactive = _inFrequency;

        }
        
    }

    public void AdjustAudio()
    {
        var frequencyDelta = Math.Abs(RadioFrequency - _radio.Frequency);
        RadioWave.volume = (1- frequencyDelta)*AudioMaxVolume;
        RadioStatic.volume = frequencyDelta * 0.7f;
    }
}
