using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DeerFirstEncounter : MonoBehaviour
{
    public GameObject Deer;
    public int RunningSpeed = 1;
    public InteractiveTextContainer ThoughtBubble;
    public ObjectiveManager objectiveManager;
    public PeachManager PeachManager;
    public YarnProgram FinishedDialogue;
    
    public Thoughts RadioDontWorkThought;
    public Thoughts FirstShotThought;
    public BBGun Gun;

    public Transform[] DeerHidePositions = new Transform[4];
    public Transform[] DeerStandPositions = new Transform[4];
    public static int CurrentArea = 0;

    public static bool CompletedChase = false;
    public static int CurrentTrigger = 0;

    private bool _movingDeer = false;
    private Vector3 _newDeerPos;

    public const string CLUE = "The Deer is concerned about its attire, something about needing to look good for an party.";
    public DialogueRunner DialogueRunner;


    // Start is called before the first frame update
    void Start()
    {
        Deer.transform.position = DeerStandPositions[CurrentArea].position;

        if (CurrentTrigger < 2)
        {
            Deer.GetComponent<InteractiveObject>().OnClick.AddListener(RadioDontWork);
        }
        else if(CurrentTrigger >= 3) {
            var deerNpc = Deer.GetComponent<NPC>();
            Deer.GetComponent<InteractiveObject>().OnClick.AddListener(deerNpc.StartConversation);
            Deer.GetComponent<Animator>().SetBool("has_antler", true);
            DialogueRunner.AddCommandHandler("FinishDeerConvo", FinishDeerConvo);


        }

        if (CurrentTrigger == 4)
        {
            Deer.GetComponent<NPC>().ScriptToLoad = FinishedDialogue;
        }

        if (HandManager.Currently.Equals(HandManager.state.HoldingBBGun))
        {

            if (CurrentTrigger == 1)
            {

                Deer.GetComponent<InteractiveObject>().OnClick.RemoveAllListeners();
                Deer.GetComponent<NPC>().EnableRadioHint = false;
                Deer.GetComponent<OnHoverHint>().Enabled = false;

                SetUpGunCollider();

                var hint = gameObject.AddComponent<OnHoverHint>() as OnHoverHint;
                hint.HintText = "Yeah this will work, I probably shouldn't shoot at him though.";
                hint.ThoughtBubble = ThoughtBubble;
            }
            else if (CurrentTrigger == 2)
            {
                
                SetUpGunCollider();
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_movingDeer)
        {
            Deer.transform.position = Vector3.MoveTowards(Deer.transform.position, _newDeerPos, RunningSpeed* Time.deltaTime);

            if (Vector3.Distance(Deer.transform.position, _newDeerPos) < 0.001f)
            {
                _movingDeer = false;
                CurrentArea = (CurrentArea + 1) % 4;
                TeleportDeer(CurrentArea);
            }
        }
    }

    public void RadioDontWork()
    {
        StartCoroutine(RadioMonolog());
    }

    private IEnumerator RadioMonolog()
    {
        CurrentTrigger = 1;
        yield return StartCoroutine(EnterMonologue(RadioDontWorkThought));
        objectiveManager.UpdateProgress(8); //"Get the Deer's Attention"

    }

    private void OnMouseDown()
    {
        if(HandManager.Currently.Equals(HandManager.state.HoldingBBGun))
        {
            if (CurrentTrigger == 2)
                StartCoroutine(ChaseSequence());
            else if (CurrentTrigger == 1)
                StartCoroutine(FirstShot());
        }
    }

    private IEnumerator FirstShot()
    {
        CurrentTrigger = 2;
        Gun.FireGun();
        Destroy(GetComponent<OnHoverHint>());
        StartCoroutine(ChaseSequence());
        yield return StartCoroutine(EnterMonologue(FirstShotThought));
        objectiveManager.UpdateProgress(9); //"Stop the Deer From Running Away"

    }

    private IEnumerator EnterMonologue(Thoughts thought)
    {
        Deer.GetComponent<OnHoverHint>().Enabled = false;

        MovementManager.LockCamera();
        ThoughtBubble.Think(thought);

        yield return new WaitUntil(() => ThoughtBubble.DialogueMode == false);

        MovementManager.UnlockCamera();
        Deer.GetComponent<OnHoverHint>().Enabled = true;

    }

    private void MoveDeerPosition(int posIndex)
    {
        if(posIndex == 1 || posIndex == 2)
        {
            Deer.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            Deer.transform.eulerAngles = Vector3.zero;

        }
        _movingDeer = true;
        _newDeerPos = DeerHidePositions[posIndex].position;
    }

    private void TeleportDeer(int posIndex)
    {
        Deer.GetComponent<Animator>().SetBool("startled", false);
        
        Deer.transform.position = DeerStandPositions[posIndex].position;
        SetUpGunCollider();

    }

    private void SetUpGunCollider()
    {
        var deerCollider = gameObject.AddComponent<CircleCollider2D>() as CircleCollider2D;
        deerCollider.radius = 7;
        transform.position = Deer.transform.position;
    }

    private IEnumerator ChaseSequence()
    {
        Destroy(GetComponent<CircleCollider2D>());
        Deer.GetComponent<OnHoverHint>().Enabled = false;

        var deerAnimator = Deer.GetComponent<Animator>();
        deerAnimator.SetBool("startled", true);

        yield return new WaitUntil(() => deerAnimator.GetCurrentAnimatorStateInfo(0).IsName("deer_run"));
        MoveDeerPosition(CurrentArea);
    }

    public void GetAntler() {
        CurrentTrigger = 3;

        string hintText = "I think he stopped running now, maybe I can talk to him.";
        ThoughtBubble.ChangeText(hintText);
        var hint = gameObject.AddComponent<OnHoverHint>() as OnHoverHint;
        hint.HintText = hintText;
        hint.ThoughtBubble = ThoughtBubble;

        objectiveManager.UpdateProgress(10); //"Speak to the Deer"

        Deer.GetComponent<Animator>().SetBool("has_antler", true);
        DialogueRunner.AddCommandHandler("FinishDeerConvo", FinishDeerConvo);

    }

    public void FinishDeerConvo(string[] parameters)
    {
        CurrentTrigger = 4;
        Deer.GetComponent<NPC>().ScriptToLoad = FinishedDialogue;
        objectiveManager.UpdateProgress(11); //"View the New CLUE at the Peach"
        PeachManager.UnlockClue(CLUE);
    }
}
