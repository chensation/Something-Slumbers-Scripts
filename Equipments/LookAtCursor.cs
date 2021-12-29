using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCursor : MonoBehaviour
{
    public Camera MainCamera;
    private Vector3 _mousePos = Vector3.zero;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _mousePos = MainCamera.ScreenToWorldPoint(Input.mousePosition);
        _mousePos = new Vector3(_mousePos.x, 20, _mousePos.z);
        transform.LookAt(_mousePos);
    }
}
