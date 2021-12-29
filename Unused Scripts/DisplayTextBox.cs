using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayTextBox : MonoBehaviour
{
    public TextMeshProUGUI ContainedText;
    private GameObject _textBox;
    // Start is called before the first frame update
    void Start()
    {
        _textBox = transform.GetChild(0).gameObject;   
    }

    // Update is called once per frame
    void Update()
    {
        if(!string.IsNullOrWhiteSpace(ContainedText.text) && !_textBox.activeSelf)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if(string.IsNullOrWhiteSpace(ContainedText.text) && _textBox.activeSelf)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
