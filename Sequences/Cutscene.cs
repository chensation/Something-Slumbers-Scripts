using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class Cutscene : MonoBehaviour
{

    public VideoPlayer video;

    private void Start()
    {
        StartCoroutine(Transition());
    }
    IEnumerator Transition()
    {
        yield return new WaitUntil(() => video.isPlaying);

        yield return new WaitUntil(() => !video.isPlaying);

        GameManager.ChangeScene((int)GameManager.Scene.Tutorial);
   
    }
}
