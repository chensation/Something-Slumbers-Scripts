using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum Scene
    {
        Undefined=-1,
        Title=0,
        IntroCutscene=1,
        Tutorial=2,
        Camera=3,
        Cabin=4
    }

    public static Scene CurrentScene;

    public static bool AllowExitGame = true;

    public static bool Paused = false;

    public GameObject PauseMenu;
    public AudioSource BackgroundMusic;
    
    // Start is called before the first frame update
    void Start()
    {
        CurrentScene = (Scene)SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        if (AllowExitGame && !CurrentScene.Equals(Scene.Camera) && Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenu != null)
            {
                if (!Paused)
                {

                    Pause();
                }
                else
                {
                    Unpause();

                }
            }
            else
            {
                ExitGame();

            }
        }
    }

    public static void ExitGame()
    {
        Application.Quit();
    }

    public static void StartGame()
    {
        ChangeScene((int)GameManager.Scene.IntroCutscene);
    }

    public static void ChangeScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
        CameraZoomEntry.ZoomReady = false;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        InteractiveObject.UniversalInteractable = false;
        BackgroundMusic.Stop();
    }

    public void Unpause()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        InteractiveObject.UniversalInteractable = true;
        BackgroundMusic.Play();
    }
}
