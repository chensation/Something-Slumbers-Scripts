using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    public enum state
    {
        Empty,
        HoldingRadio,
        HoldingBBGun
    }

    public static state Currently;

    private GameObject _radioUI = null;
    private GameObject _radio = null;

    private GameObject _bbGun = null;
    

    // Start is called before the first frame update
    void Start()
    {
        _radioUI = GameObject.FindGameObjectWithTag("RadioUI");
        _radio = GameObject.FindGameObjectWithTag("Radio");
  
        _radioUI?.SetActive(Currently.Equals(state.HoldingRadio));
        _radio?.SetActive(Currently.Equals(state.HoldingRadio));

        _bbGun = GameObject.FindGameObjectWithTag("BBGun");
        _bbGun?.SetActive(Currently.Equals(state.HoldingBBGun));


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void HoldRadio()
    {
        Currently = state.HoldingRadio;
    }

    public static void DropAll()
    {
        Currently = state.Empty;

    }

    public static void HoldBBGun()
    {
        Currently = state.HoldingBBGun;
    }
}
