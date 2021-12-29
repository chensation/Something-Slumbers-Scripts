using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintShow : MonoBehaviour
{
    public GameObject Clue1;
    public GameObject Clue2;
    public GameObject Clue3;
    public GameObject Clue4;
    public GameObject Clue5;
    public GameObject Clue6;
    public GameObject Clue_1;
    public GameObject Clue_2;
    public GameObject Clue_3;
    public GameObject Clue_4;
    public GameObject Clue_5;
    public GameObject Clue_6;
    public Button DeduceButton;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S) && Clue1.activeSelf == false)
        {
            Clue_1.SetActive(false);
            Clue_2.SetActive(false);
            Clue_3.SetActive(false);
            Clue1.SetActive(true);
            Clue2.SetActive(true);
            Clue3.SetActive(true);

        }
        
        if (Input.GetKey(KeyCode.W) && Clue1.activeSelf == true)
        {
            Clue1.SetActive(false);
            Clue2.SetActive(false);
            Clue3.SetActive(false);
            Clue_1.SetActive(true);
            Clue_2.SetActive(true);
            Clue_3.SetActive(true);
            
        }

        if (Input.GetKey(KeyCode.D) && Clue4.activeSelf == false)
        {
            Clue_4.SetActive(false);
            Clue_5.SetActive(false);
            Clue_6.SetActive(false);
            Clue4.SetActive(true);
            Clue5.SetActive(true);
            Clue6.SetActive(true);

        }
        
        if(Input.GetKey(KeyCode.E) && Clue4.activeSelf == true)
        {
            Clue4.SetActive(false);
            Clue5.SetActive(false);
            Clue6.SetActive(false);
            Clue_4.SetActive(true);
            Clue_5.SetActive(true);
            Clue_6.SetActive(true);
          
        }

        if (Clue1.activeSelf == true && Clue2.activeSelf == true && Clue3.activeSelf == true)
        {
            DeduceButton.interactable = true;

        } else if (Clue4.activeSelf == true && Clue5.activeSelf == true && Clue6.activeSelf == true)
        {
            DeduceButton.interactable = true;
        }
        else
        {
            DeduceButton.interactable = false;
        }
    }
}
