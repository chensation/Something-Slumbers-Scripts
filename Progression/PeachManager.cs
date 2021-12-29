using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PeachManager : MonoBehaviour
{
    public const int NumClues = 6;
    public GameObject PeachUI;
    public Button RecallButton;
    public Button DeduceButton;
    public GameObject Peach;
    public static bool FirstTime = true;
    private Thoughts _thought;

    private ObjectiveManager _objectiveManager;
    private AssetStore _assetStore;
    public InteractiveTextContainer ThoughtBubble;

    
    public Clue[] Clues = new Clue[NumClues];
    public static string[] ClueTexts = new string[NumClues]; 
    public static int UnlockedClueCount = 0;
    
    public static bool InUI = false;

    
    // Start is called before the first frame update
    void Start()
    {
        PeachUI.SetActive(InUI);

        _objectiveManager = GetComponent<ObjectiveManager>();
        _assetStore = GetComponent<AssetStore>();
        _thought = Peach.GetComponent<Thoughts>();

        for(int i = 0; i < UnlockedClueCount; i++)
        {
            Clues[i].Unlock();
            Clues[i].SetClue(ClueTexts[i]);
        }

        if (!FirstTime)
        {
            Peach.GetComponent<OnHoverHint>().HintText = "Here's the peach... I think I can recollect my thoughts here.";
        }

        DeduceButton.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (InUI)
        {
            if (!ThoughtBubble.DialogueMode && (Input.GetKey(KeyCode.Escape) || Input.GetMouseButtonUp(1)))
                CloseUI();
            if (!ThoughtBubble.DialogueMode)
                RecallButton.interactable = true;
        }   

        if(UnlockedClueCount == NumClues) 
        {
            DeduceButton.interactable = true;
        }

        
    }

    public void OpenUI()
    {
        InUI = true;
        PeachUI.SetActive(InUI);
        MovementManager.LockCamera();
        InteractiveObject.UniversalInteractable = false;

        Peach.GetComponent<OnHoverHint>().CloseHint();
        Peach.GetComponent<OnHoverHint>().Enabled = false;
        CursorManager.SetCursor(_assetStore.NormalCursor);
        
        if (FirstTime)
        {
            Recall();
        }

        if(UnlockedClueCount >= 2 && !DemoManager.ViewedEndWarning)
        {
            ThoughtBubble.ChangeText("Hey, I think I can deduce some stuff now.");
            _objectiveManager.UpdateProgress(13);
        }


    }

    public void CloseUI()
    {
        InUI = false;
        PeachUI.SetActive(InUI);
        Peach.GetComponent<OnHoverHint>().Enabled = true;
        InteractiveObject.UniversalInteractable = true;

        CursorManager.SetCursor(CursorManager.NormalCursor);
        
        if (ThoughtBubble.DialogueMode)
        {
            ThoughtBubble.StopThinking();
        }

        if (FirstTime)
        {
            _objectiveManager.UpdateProgress(7); //"Find Others to Speak to"
            FirstTime = false;
            Peach.GetComponent<OnHoverHint>().HintText = "Here's the peach... I think I can recollect my thoughts here.";
        }

        if(ObjectiveManager.ProgressTracker == 11 && UnlockedClueCount < 2) //LOOKED AT NEW CLUE
            _objectiveManager.UpdateProgress(7);

        StartCoroutine(MovementManager.WaitThenUnlockCamera());

    }

    public void Recall()
    {
        RecallButton.interactable = false;
        ThoughtBubble.Think(_thought);
    }

    public void UnlockClue(string clue)
    {

        Clues[UnlockedClueCount].Unlock();
        Clues[UnlockedClueCount].SetClue(clue);
        ClueTexts[UnlockedClueCount] = clue;
        UnlockedClueCount++;
    }
}
