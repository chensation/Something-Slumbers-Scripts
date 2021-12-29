using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{

    [System.NonSerialized]
    public NPC ClosestNPC;
    private List<NPC> _allNPCs = new List<NPC>();
    private Transform _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        var allObjs = GameObject.FindGameObjectsWithTag("InteractiveObject");

        foreach(GameObject obj in allObjs)
        {
            if(obj.GetComponent<NPC>() != null)
            {
                _allNPCs.Add(obj.GetComponent<NPC>());
            }
        }

        UpdateClosestNPC();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateClosestNPC()
    {
        if(_player != null)
        {
            NPC bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = _player.position;
            foreach (NPC potentialTarget in _allNPCs)
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }

            ClosestNPC = bestTarget;
        }
       
    }


}
