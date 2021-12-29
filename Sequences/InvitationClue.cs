using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvitationClue : MonoBehaviour
{
    public Thoughts clueThought;
    public InteractiveTextContainer ThoughtBubble;
    public PeachManager PeachManager;
    public ObjectiveManager objectiveManager;
    public GameObject Cat;
    public static bool CheckedOut = false;

    public const string CLUE = "The cat had some sort of invitation...I can make out the words \"ONE EON\" on there.";
    // Start is called before the first frame update
    void Start()
    {
        if (CheckedOut)
        {
            this.GetComponent<InteractiveObject>().Text = "Yep, they fell out of that cat's head alright";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerClue()
    {
        if (!CheckedOut)
        {
            StartCoroutine(ThinkThenUpdateObj());
        }


    }

    private IEnumerator ThinkThenUpdateObj()
    {
        Cat.GetComponent<OnHoverHint>().Enabled = false;
        MovementManager.LockCamera();

        ThoughtBubble.Think(clueThought);

        yield return new WaitUntil(() => ThoughtBubble.DialogueMode == false);

        Cat.GetComponent<OnHoverHint>().Enabled = true;
        MovementManager.UnlockCamera();
        objectiveManager.UpdateProgress(11);
        PeachManager.UnlockClue(CLUE);

        this.GetComponent<InteractiveObject>().Text = "Yep, they fell out of that cat's head alright";
        CheckedOut = true;

    }
}
