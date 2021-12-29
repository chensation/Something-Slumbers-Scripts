using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveCamera : MonoBehaviour
{
    public int BoundaryPercentage = 25;
    public int Speed = 5;
    public AudioSource MoveAudio;
    public AudioSource TakePhotoAudio;
    public InteractiveTextContainer ThoughtBubble;
    private int _leftPanTrigger;
    private int _rightPanTrigger;
    private int _totalPanRange;

    private (float, float) _boundary = (int.MinValue, int.MaxValue);

    private Vector3 _destination;
    private bool _moveToDest = false;

    public static bool Panning = false;
    public static bool Zooming = false;

    private NPCManager _npcManager;

    public Animator TransitionController;
    // Start is called before the first frame update
    void Start()
    {

        GetComponent<Camera>().transparencySortMode = TransparencySortMode.Orthographic;

        _leftPanTrigger = (int)((BoundaryPercentage / 100.0) * Screen.width);
        _rightPanTrigger = Screen.width - _leftPanTrigger;
        _totalPanRange = _leftPanTrigger;

        _npcManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<NPCManager>();

        StartCoroutine(InitializePosition());



    }

    IEnumerator InitializePosition()
    {
        if (MovementManager.Boundaries.Count == 0)
        {
            yield return new WaitForSeconds(0.2f);

        }


        _boundary = MovementManager.GetLastBoundary();
        transform.position = MovementManager.GetLastPosition();
    }

    // Update is called once per frame
    void Update()
    {
        Pan();
        Zoom();
        TakePhoto();

        if (!Zooming && !Panning && !MovementManager.CamLocked && Input.GetMouseButtonUp(1)) //right click
        {
            ReturnToPrevLocation();
        }

        if (!Zooming && !Panning && !MovementManager.CamLocked && Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(ExitCamera());
        }

        if ((Panning || Zooming) && !MovementManager.CamLocked)
        {
            if (!MoveAudio.isPlaying)
            {
                MoveAudio.Play();
            }
        }
        else if(MoveAudio.isPlaying)
        {
            MoveAudio.Stop();
        }
    }

    public void Pan()
    {
        if (!MovementManager.CamLocked)
        {
            if (Input.mousePosition.x > _rightPanTrigger && transform.position.x < _boundary.Item2) //move to the right
            {
                float speedMultiplier = (Input.mousePosition.x - _rightPanTrigger) / _totalPanRange;

                transform.Translate(Vector3.right * Time.deltaTime * Speed * speedMultiplier);
                Panning = true;
                
            }

            else if (Input.mousePosition.x < _leftPanTrigger && transform.position.x > _boundary.Item1) //move to the left
            {
                float speedMultiplier = (_totalPanRange - Input.mousePosition.x) / _totalPanRange;
                transform.Translate(-Vector3.right * Time.deltaTime * Speed * speedMultiplier);
                Panning = true;
            }
            else
            {
                Panning = false;
            }
        }

 
    }

    public void Zoom()
    {
        if (_moveToDest && !MovementManager.CamLocked)
        {
            Zooming = true;
            transform.position = Vector3.MoveTowards(transform.position, _destination, Speed * Time.deltaTime * 1.5f);

            if (Vector3.Distance(transform.position, _destination) < 0.001f)
            {
                Zooming = false;
                _moveToDest = false;
                _npcManager.UpdateClosestNPC();
            }
        }
    }

    public void TakePhoto()
    {
        if (!MovementManager.CamLocked && Input.GetKeyDown(KeyCode.Space))
        {
            TakePhotoAudio.Play();
            var filePath = Path.Combine(Application.persistentDataPath, PhotoAlbum.PhotoIndex+".png");
            ScreenCapture.CaptureScreenshot(filePath);
            Debug.Log("Saved to " + filePath);
            PhotoAlbum.SavePhoto(filePath);

            ThoughtBubble.ChangeText("I can view that photo in my photo album.");

        }
    }

    public void MoveToDestination(Vector3 dest, (float, float) newBound)
    {
        _destination = dest;
        MovementManager.SavePosition(transform.position);
        MovementManager.SaveBoundary(_boundary);
        _boundary = newBound;
        _moveToDest = true;
    }

    public void ReturnToPrevLocation()
    {
        _destination = MovementManager.GetLastPosition();
        _boundary = MovementManager.GetLastBoundary();
        _moveToDest = true;

    }

    public void SetPanBoundary(int left, int right)
    {
        _boundary = (left, right);
    }

    public IEnumerator ExitCamera()
    {
        MovementManager.SavePosition(transform.position);
        MovementManager.SaveBoundary(_boundary);
        HandManager.DropAll();

        TransitionController.SetBool("start", true);

        yield return new WaitForSeconds(1);
        GameManager.ChangeScene((int)GameManager.Scene.Cabin);
    }
}
