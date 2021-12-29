using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Thoughts : MonoBehaviour
{
    [Tooltip("/o\\")]
    public Color NormalTextColor = Color.white;
    [Tooltip("\\o/")]
    public Color StrangeTextColor;

    [Tooltip("-o-")]
    public Color NormalBackgroundColor = Color.white;
    [Tooltip("~O~")]
    public Color StrangeBackgroundColor;

    [Tooltip("_o_")]
    public TMP_FontAsset NormalFont;
    [Tooltip("^o^")]
    public TMP_FontAsset StrangeFont;

    public List<string> ListOfThoughts;
    private int _currIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    public string Increment()
    {
        if (_currIndex < ListOfThoughts.Count)
        {
            _currIndex++;
            return ListOfThoughts[_currIndex-1];
        }
        else
        {
            Reset();
            return null;
        }
    }

    public void Reset()
    {
        _currIndex = 0;
    }
}
