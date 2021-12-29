using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BBGun : MonoBehaviour
{
    public AudioClip FireSound;
    public InteractiveTextContainer ThoughtBubble;
    private AudioManager _audioManager;
    // Start is called before the first frame update
    void Start()
    {
        _audioManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !DialogueManager.InDialogue 
            && !PeachManager.InUI && !ThoughtBubble.DialogueMode && !CameraZoomEntry.ZoomReady)
        {
            FireGun();
        }    
    }

    public void FireGun()
    {
        _audioManager.PlaySound(FireSound);
    }


}
