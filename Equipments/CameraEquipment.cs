using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEquipment : MonoBehaviour
{
    public Animator TransitionController;
    public void PeekThrough()
    {
        StartCoroutine(TransitionThenChangeScene());
    }

    public IEnumerator TransitionThenChangeScene()
    {
        TransitionController.SetBool("start", true);
        yield return new WaitForSeconds(1);
        GameManager.ChangeScene((int)GameManager.Scene.Camera);

    }
}
