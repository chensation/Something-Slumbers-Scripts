using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPromptManager : MonoBehaviour
{

    public ButtonPrompt LeftClick;
    public ButtonPrompt RightClick;
    public ButtonPrompt MouseWheel;
    public ButtonPrompt Space;
    public ButtonPrompt EscapeKey;

    public InteractiveTextContainer ThoughtBubble;
    private bool _overwriteInteract = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.CurrentScene.Equals(GameManager.Scene.Tutorial) || GameManager.CurrentScene.Equals(GameManager.Scene.Cabin))
        {

            LeftClick.SetText("Interact");
            RightClick.Deactivate();
            EscapeKey.SetText("Pause Game");
        }

        if (!_overwriteInteract)
        {
            LeftClick.SetText("Interact");

        }

        if (GameManager.CurrentScene.Equals(GameManager.Scene.Camera))
        {
            RightClick.SetText("Zoom Out");
            Space.SetText("Take Photo");
            EscapeKey.SetText("Return to Cabin");

        }

        if (HandManager.Currently.Equals(HandManager.state.HoldingRadio) && !DialogueManager.InDialogue)
        {
            MouseWheel.SetText("Adjust Frequency");
        }
        
        if (CombineManager.InCombineMode)
        {
            LeftClick.SetText("Combine");
        }

        if (PeachManager.InUI)
        {
            LeftClick.SetText("Interact");
            RightClick.SetText("Stop Inspecting");
            MouseWheel.Deactivate();
            EscapeKey.SetText("Stop Inspecting");
        }

        if (DialogueManager.InDialogue || (ThoughtBubble != null && ThoughtBubble.DialogueMode))
        {
            LeftClick.SetText("Interact");
            RightClick.Deactivate();
            MouseWheel.Deactivate();
            EscapeKey.Deactivate();
        }

        if (MovementManager.CamLocked)
        {
            Space.Deactivate();
        }

        if (PhotoAlbum.AlbumOpen)
        {
            LeftClick.Deactivate();
            RightClick.SetText("Close Album");
            EscapeKey.SetText("Close Album");
        }



    }
    public void ChangeInteractText(string text)
    {
        LeftClick.SetText(text);
        _overwriteInteract = true;
    }

    public void ResetInteractText()
    {
        _overwriteInteract = false;
    }
}
