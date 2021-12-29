using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class CatFirstEncounter : MonoBehaviour
{
    public GameObject Cat;
    public GameObject PartyClue;
    public ObjectiveManager objectiveManager;
    public static int CurrentTrigger = 0;

    public static bool AlreadyExploded = false;
    public DialogueRunner DialogueRunner;

    public Animator PortraitAnimator;
    private Animator _catAnimator;

    // Start is called before the first frame update
    void Start()
    {
        _catAnimator = Cat.GetComponent<Animator>();
        if (CurrentTrigger == 0)
        {
            var catNpc = Cat.GetComponent<NPC>();
            Cat.GetComponent<InteractiveObject>().OnClick.AddListener(catNpc.StartConversation);
            DialogueRunner.AddCommandHandler("ExplodeCat", ExplodeCat);

        }
        else if(CurrentTrigger == 1)
        {
            ExplodeCat(null);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!_catAnimator.GetBool("headless") &&
            PortraitAnimator.isActiveAndEnabled && PortraitAnimator.parameterCount == 4 
            && PortraitAnimator.GetBool("explode"))
        {
            Cat.GetComponent<Animator>().SetBool("headless", true);
        }
    }

    public void ExplodeCat(string[] parameters)
    {
        _catAnimator.SetBool("headless", true);
        CurrentTrigger = 1;
        Cat.GetComponent<OnHoverHint>().Enabled = false;
        var catObj = Cat.GetComponent<InteractiveObject>();

        catObj.Text = "Yikes, don't think I can talk to him anymore.";
        catObj.OnClick.RemoveAllListeners();
        PartyClue.SetActive(true);

        if (!AlreadyExploded)
        {
            objectiveManager.UpdateProgress(12);
        }

        AlreadyExploded = true;

    }
}
