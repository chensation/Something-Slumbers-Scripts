using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementProgressThenDie : MonoBehaviour
{
    public ObjectiveManager ObjectiveManager;

    public bool SpecifyObjective = false;
    public int ObjectiveIndex; 

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerCollider"))
        {
            if(SpecifyObjective)
                ObjectiveManager.UpdateProgress(ObjectiveIndex);
            else
                ObjectiveManager.IncrementProgress();
            Destroy(gameObject);
        }
    }
}
