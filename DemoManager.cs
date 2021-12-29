using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoManager : MonoBehaviour
{
    public GameObject DeduceButton;
    public GameObject DemoEndButton;

    public InteractiveTextContainer ThoughtBubble;
    public Thoughts DemoEndThought;

    public static bool ViewedEndWarning = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PeachManager.UnlockedClueCount >= 2)
        {
            DeduceButton.GetComponent<Button>().interactable = true;
        }

        if (ViewedEndWarning)
        {
            DeduceButton.SetActive(false);
            DemoEndButton.SetActive(true);
        }

        DemoEndButton.GetComponent<Button>().interactable = !ThoughtBubble.DialogueMode;
    }

    public void EndDemo()
    {
        if (ViewedEndWarning)
        {
            GameManager.ChangeScene(5);
        }
        else
        {
            ThoughtBubble.Think(DemoEndThought);
            ViewedEndWarning = true;
        }
    }


}
