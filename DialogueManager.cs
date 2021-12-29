using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;
using TMPro;
using System.Linq;

public class DialogueManager : MonoBehaviour
{
    public static bool InDialogue = false;

    public DialogueRunner DialogueRunner;
    public Image Portrait;
    public TextMeshProUGUI Speech;
    public TextMeshProUGUI[] OptionButtons;

    [System.Serializable]
    public struct Character
    {
        public string name;
        public Color color;
        public TMP_FontAsset font;
        public float fontSize;
    }

    public Character[] Characters;

    private void Awake()
    {

        DialogueRunner.AddCommandHandler("ChangeCharacter", ChangeCharacter);
        DialogueRunner.AddCommandHandler("ChangeCharAnimation", ChangeCharAnimation);
    }

    public void StartConversation(YarnProgram script, Sprite portrait, RuntimeAnimatorController controller)
    {
        //set up dialogue ui events, sprites, etc
        DialogueRunner.Clear();
        DialogueRunner.Add(script);
        Portrait.sprite = portrait;

        if(controller != null)
        {
            Portrait.GetComponent<Animator>().runtimeAnimatorController = controller;
        }
        HandManager.DropAll();
        MovementManager.LockCamera();
        DialogueRunner.StartDialogue("Start");
        InDialogue = true;

    }

    public void StopConversation()
    {
        Portrait.GetComponent<Animator>().runtimeAnimatorController = null;
        MovementManager.UnlockCamera();
        HandManager.HoldRadio();
        InDialogue = false;
    }

    public void ChangeCharacter(string[] parameters)
    {
        string name = parameters[0];
        int option = -1;

        if(parameters.Length > 1)
        {
            string optionStr = parameters[1];

            option = int.Parse(optionStr);

        }

        
        if (string.IsNullOrEmpty(name))
        {
            Debug.Log("Change Character called with empty name");
            return;        
        }

        Debug.Log(Characters.Length);

        var character = Characters.FirstOrDefault(chars => chars.name.ToLower() == name.ToLower());

        if(character.name == null)
        {
            Debug.Log(name + " was not a recognized name by the dialogue manager");
            return;
        }
       
        if(option == -1)
        {
            Speech.color = character.color;
            Speech.font = character.font;
            Speech.fontSize = character.fontSize;
        }
        else
        {
            OptionButtons[option].color = character.color;
            OptionButtons[option].font = character.font;
            OptionButtons[option].fontSize = character.fontSize;

        }
    }

    public void ChangeCharAnimation(string[] parameters)
    {
        string name = parameters[0];
        
        var animator = Portrait.GetComponent<Animator>();
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
                animator.SetBool(parameter.name, false);
        }

        animator.SetBool(name, true);

    }
}
