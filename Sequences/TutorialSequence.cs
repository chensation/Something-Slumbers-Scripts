using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSequence : MonoBehaviour
{
    public Transform cabin;
    private bool _moveCabin = false;
    private Vector3 _cabinDest;

    private List<InteractiveObject> _items = new List<InteractiveObject>();

    //keep track of how many times the player have clicked on the door
    private int _doorClickCount = 0;

    //keep track of item tutorial progress
    private bool[] _itemChecked = new bool[] { false, false, false, false, false };
    private bool _allItemChecked = false;

    public InteractiveTextContainer ThoughtContainer;
    public ObjectiveManager objectiveManager;
    public Animator TransitionController;

    // Start is called before the first frame update
    void Start()
    {
        //get ui elements
        if(ThoughtContainer == null)
            ThoughtContainer = GameObject.FindGameObjectWithTag("ThoughtBubble").GetComponent<InteractiveTextContainer>();
        if (objectiveManager == null)
                objectiveManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectiveManager>();
        
        //get all interactive elements and disable them
        var allInteractives = GameObject.FindGameObjectsWithTag("InteractiveObject");

        foreach (var interactive in allInteractives)
        {
            var temp = interactive.GetComponent<InteractiveObject>();
            temp.Interactive = false;
            _items.Add(temp);

        }

        //enable the door sequence
        var door = _items.Find(item => item.gameObject.name == "door");
        door.OnClick.AddListener(TriggerDoor);
        door.Interactive = true;

        //disable full cup of coffee until coffee is made 
        var fullCup = _items.Find(item => item.gameObject.name == "full cup");
        fullCup.gameObject.SetActive(false);


        _cabinDest = cabin.transform.position + new Vector3(-4, 0, 0);
    }

    private void Update()
    {
        if (_moveCabin)
        {
            cabin.position = Vector3.MoveTowards(cabin.position, _cabinDest, Time.deltaTime * 1.5f);
            
            if (Vector3.Distance(cabin.position, _cabinDest) < 0.001f)
            {
                _moveCabin = false;
            }
        }
    }

    //the first player sequence of knocking on the door
    public void TriggerDoor()
    {
        switch (_doorClickCount)
        {
            case 0:
                ThoughtContainer.ChangeText("I don't want to go outside.");
                break;
            case 1:
                ThoughtContainer.ChangeText("What was that about a peach?");
                break;
            case 2:
                ThoughtContainer.ChangeText("I'm on vacation, I don't want to do anything.");
                break;
            default:
                ThoughtContainer.ChangeText("I might, if I pour myself a cup of coffee from the coffee pot...");

                //should be pour coffee now
                StartCoroutine(objectiveManager.WaitThenUpdateProgress(1, 1));
                
                //disable the door
                InteractiveObject door = _items.Find(item => item.gameObject.name == "door");
                door.Interactive = false;
                door.Deselect();
                door.OnClick.RemoveAllListeners();

                //enable the coffee stuff
                var emptyCup = _items.Find(item => item.gameObject.name == "empty cup");
                var coffeePot = _items.Find(item => item.gameObject.name == "coffee pot");
                var fullCup = _items.Find(item => item.gameObject.name == "full cup");
                fullCup.OnClick.AddListener(CheckDoorAgain);

                emptyCup.Interactive = true;
                coffeePot.Interactive = true;
                fullCup.Interactive = true;

                break;
        }
        _doorClickCount++;

    }

    public void CheckDoorAgain()
    {
        var emptyCup = _items.Find(item => item.gameObject.name == "empty cup");
        var coffeePot = _items.Find(item => item.gameObject.name == "coffee pot");
        emptyCup.Interactive = false;
        coffeePot.Interactive = false;

        var fullCup = _items.Find(item => item.gameObject.name == "full cup");
        fullCup.OnClick.RemoveListener(CheckDoorAgain);

        //should be on Go Outside to Investigate the Peach again
        StartCoroutine(objectiveManager.WaitThenUpdateProgress(1, 2));

        //allow player interact with door again, this time for the item tutorial
        InteractiveObject door = _items.Find(item => item.gameObject.name == "door");
        door.Interactive = true;
        door.Text = "Uhhh, actually, I lied. I'd rather \"investigate\" my cabin.";

        door.OnClick.AddListener(BeginItemTutorial);
    }

    public void BeginItemTutorial()
    {
        //should be on Investigate the Room
        StartCoroutine(objectiveManager.WaitThenUpdateProgress(1, 3));

        InteractiveObject door = _items.Find(item => item.gameObject.name == "door");
        door.OnClick.RemoveAllListeners();

        var bbGun = _items.Find(item => item.gameObject.name == "bb gun");
        bbGun.Interactive = true;
        
        var fishingRod = _items.Find(item => item.gameObject.name == "fishing rod");
        fishingRod.Interactive = true;
        
        var radio = _items.Find(item => item.gameObject.name == "radio");
        radio.Interactive = true;
        
        var camera = _items.Find(item => item.gameObject.name == "camera");
        camera.Interactive = true;

        var photoAlbum = _items.Find(item => item.gameObject.name == "photo album");
        photoAlbum.Interactive = true;
    }

    public void CheckItemTutorial(int itemIndex)
    {

        if (!_allItemChecked)
        {

            //set this item to be already checked   
            _itemChecked[itemIndex] = true;

            int numItemChecked = 0;
            foreach(var item in _itemChecked)
            {
                if (item) numItemChecked++;
            }

            if(numItemChecked < 5)
            {
                InteractiveObject door = _items.Find(item => item.gameObject.name == "door");
                door.Text = "There's got to be something else that I can \"investigate\" and waste time with...";
            }
            else
            {
                _allItemChecked = true;
                StartCoroutine(EndTutorialSequence());
            }

        }

    }

    IEnumerator EndTutorialSequence()
    {
        yield return new WaitForSeconds(2);
        //should be on investigate peach with camera now
        StartCoroutine(objectiveManager.WaitThenUpdateProgress(1, 4));

        InteractiveObject door = _items.Find(item => item.gameObject.name == "door");
        door.Text = "I think I'll just use my camera thanks";

        _moveCabin = true;
        
        var man = _items.Find(item => item.gameObject.name == "man");
        man.Text = "Fine, but I’m still not gonna go out. I’ll just check outside with the camera instead.";
        ThoughtContainer.ChangeText(man.Text);
        man.Interactive = true;

        var camera = _items.Find(item => item.gameObject.name == "camera");
        camera.Text = null;
        camera.OnClick.RemoveAllListeners();
        var comp = camera.gameObject.AddComponent<CameraEquipment>();
        comp.TransitionController = TransitionController;
        camera.OnClick.AddListener(comp.PeekThrough);
    }

}
