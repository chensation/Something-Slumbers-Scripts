using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radio : MonoBehaviour
{
    public Transform Needle;
    public RectTransform NeedleSpace;
    private float _needleLeftBoundary;
    private float _needleRightBoundary;
    private float _totalNeedleDistance;

    public Transform Nob;

    [System.NonSerialized]
    public float Frequency = 0; //between 0-1
    
    private int _scale = 10;

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] v = new Vector3[4];
        NeedleSpace.GetWorldCorners(v);
        _needleLeftBoundary = v[0].x;
        _needleRightBoundary = v[2].x;

        var netDist = _needleRightBoundary - _needleLeftBoundary;
        var tenPercent = netDist * 0.1f;
        _needleLeftBoundary = _needleLeftBoundary + tenPercent;
        _needleRightBoundary = _needleRightBoundary - tenPercent;
        _totalNeedleDistance = netDist * 0.8f;
    }

    // Update is called once per frame
    void Update()
    {
        MoveNeedle();
    }
    
    void MoveNeedle()
    {
        var deltaDist = Input.mouseScrollDelta.y * _scale;
        if (Needle.position.x + deltaDist < _needleRightBoundary && Needle.position.x + deltaDist > _needleLeftBoundary)
        {
            Needle.Translate(deltaDist, 0, 0);
            Nob.Rotate(0, 0, -deltaDist);

            Frequency = (Needle.position.x - _needleLeftBoundary) / _totalNeedleDistance;
        }
    }

}
