using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonPrompt : MonoBehaviour
{
    public TextMeshProUGUI Text;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetText(string text)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        Text.text = text;
    }

    public void Deactivate()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);

        }
    }
}
