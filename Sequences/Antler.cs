using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Antler : MonoBehaviour
{
    public GameObject Deer;
    public DeerFirstEncounter FirstEncounterScript;
    public static bool Fell = false;

    // Start is called before the first frame update
    void Start()
    {
        if(DeerFirstEncounter.CurrentTrigger >= 3){
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Fell)
        {
            GetComponent<Animator>().SetBool("fall", true);
            if (Vector2.Distance(Deer.transform.position, transform.position) < 2f)
            {
                FirstEncounterScript.GetAntler();
                Destroy(gameObject);
            }
        }
    }

    private void OnMouseDown()
    {
        if (HandManager.Currently.Equals(HandManager.state.HoldingBBGun))
        {
            Fell = true;
        }
    }
}
