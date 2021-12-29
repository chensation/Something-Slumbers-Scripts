using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverWriteZoomOut : MonoBehaviour
{
    public CameraZoomEntry EntryToOverwriteWith;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            //throw out old coords
            MovementManager.GetLastBoundary();
            MovementManager.GetLastPosition();

            //add new coords
            MovementManager.SaveBoundary(EntryToOverwriteWith.GetBoundary());
            MovementManager.SavePosition(EntryToOverwriteWith.transform.position);
        }
    }
}
