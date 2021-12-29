using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class TextBox : MonoBehaviour
{
    private TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void updateText(string newText)
    {
        text.SetText(newText);
    }

    public void ChangeTextColor(Color color)
    {
        text.color = color;
    }

    public void ChangeFont(TMP_FontAsset font)
    {
        text.font = font;
    }
}
