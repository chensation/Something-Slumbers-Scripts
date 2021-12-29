using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHoverHint : MonoBehaviour
{
    public string HintText;
    public InteractiveTextContainer ThoughtBubble;
    private bool _thinking = false;
    
    [System.NonSerialized]
    public bool Enabled = true;

    private void Start()
    {
        if(ThoughtBubble == null)
            ThoughtBubble = GameObject.FindGameObjectWithTag("ThoughtBubble").GetComponent<InteractiveTextContainer>();
    }
    private void OnMouseEnter()
    {
        if (!_thinking && Enabled && !DialogueManager.InDialogue)
        {
            ThoughtBubble.ChangeText(HintText);
            ThoughtBubble.StopClosing();
            _thinking = true;
        }
    }

    private void OnMouseExit()
    {
        if(Enabled)
            CloseHint();
    }

    public void CloseHint()
    {
        _thinking = false;
        ThoughtBubble.Close();
    }
}
