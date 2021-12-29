using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeScript : MonoBehaviour
{
    public InteractiveObject EmptyCup, CoffeePot, FullCup;
    
    //make some coffee :)
    public void MakeCoffee()
    {

        CoffeePot.Deselect();

        FullCup.gameObject.SetActive(true);
    }
}
