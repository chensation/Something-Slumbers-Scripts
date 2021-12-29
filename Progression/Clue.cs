using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Clue : MonoBehaviour
{
    public GameObject LockIcon;
    public GameObject text;
    // Start is called before the first frame update

    public void Lock()
    {
        LockIcon.SetActive(true);
        text.SetActive(false);
    }

    public void Unlock()
    {
        LockIcon.SetActive(false);
        text.SetActive(true);
    }

    public void SetClue(string clue)
    {
        text.GetComponent<TextMeshProUGUI>().text = clue;
    }
}
