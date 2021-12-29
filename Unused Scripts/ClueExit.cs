using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueExit : MonoBehaviour
{

    public GameObject PeachUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            PeachUI.SetActive(false);
            MovementManager.UnlockCamera();
        }
    }
}
