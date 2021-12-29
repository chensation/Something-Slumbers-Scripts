using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public static Stack<Vector3> VisitedPositions = new Stack<Vector3>();
    public static Stack<(float, float)> Boundaries = new Stack<(float, float)>();
    public static bool CamLocked = false;

    private Transform _player;
    
    public int InitialLeftBoundary;
    public int InitialRightBoundary;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if(VisitedPositions.Count == 0)
        {
            VisitedPositions.Push(_player.position);

        }

        if(Boundaries.Count == 0)
        {
            Boundaries.Push((InitialLeftBoundary, InitialRightBoundary));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SavePosition(Vector3 pos)
    {
        VisitedPositions.Push(pos);
    }

    public static void SaveBoundary((float, float) bound)
    {
        Boundaries.Push(bound);
    }

    public static Vector3 GetLastPosition()
    {
        if(VisitedPositions.Count > 1)
        {
            return VisitedPositions.Pop();
        }
        else
        {
            return VisitedPositions.Peek();
        }
    }

    public static (float, float) GetLastBoundary()
    {
        if (Boundaries.Count > 1)
        {
            return Boundaries.Pop();
        }
        else
        {
            return Boundaries.Peek();
        }
    }

    public static void LockCamera()
    {
        CamLocked = true;
    }

    public static void UnlockCamera()
    {
        CamLocked = false;
    }

    public static IEnumerator WaitThenUnlockCamera()
    {
        yield return new WaitForSeconds(0.2f);
        UnlockCamera();
    }
}
