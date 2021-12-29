using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSceneTutorial : MonoBehaviour
{
    private ObjectiveManager _objectiveManager;

    public static bool firstTimeInCameraScene = true;
    public static bool CheckedPeach = false;
    public GameObject peachTrigger;

    // Start is called before the first frame update
    void Start()
    {
        _objectiveManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<ObjectiveManager>();

        if (firstTimeInCameraScene)
        {
            StartCoroutine(_objectiveManager.WaitThenUpdateProgress(0.5f, 5)); //"find the peach tree"
            firstTimeInCameraScene = false;
        }
        if(!CheckedPeach)
            peachTrigger.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!CheckedPeach && (peachTrigger == null || !peachTrigger.activeSelf))
        {
            CheckedPeach = true;
        }
    }
}
