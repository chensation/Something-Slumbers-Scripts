using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineManager : MonoBehaviour
{
    private List<InteractiveObject> _items = new List<InteractiveObject>();
    public static int NumOfSelected = 0;
    public static bool InCombineMode = false;

    // Start is called before the first frame update
    void Start()
    {
        var allInteractives = GameObject.FindGameObjectsWithTag("InteractiveObject");

        foreach(var interactive in allInteractives)
        {
            
            _items.Add(interactive.GetComponent<InteractiveObject>());
            
        }
    }

    private void Update()
    {
        if(InCombineMode && !InteractiveObject.UniversalInteractable)
        {
            ExitCombineMode();
            NumOfSelected = 0;
        }
    }
    public void CheckSelected(InteractiveObject obj)
    {
        if(!obj.Selected)
        {
            ExitCombineMode();
            obj.Deselect();
            NumOfSelected = 0;

        }

        if (NumOfSelected > 1)
        {
            if (obj.ObjectToCombineWith != null && obj.ObjectToCombineWith.Selected)
            {
                obj.CombinedAction.Invoke();
            }

            ExitCombineMode();
            DeselectAll();
        }

        
    }

    private void DeselectAll()
    {
        //clear all selections
        foreach (var item in _items)
        {
            item.Deselect();
        }

        NumOfSelected = 0;
    }

    public static void ExitCombineMode()
    {
        InCombineMode = false;
        CursorManager.SetCursor(CursorManager.NormalCursor);
    }
}
