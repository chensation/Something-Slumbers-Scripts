using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditMovement : MonoBehaviour
{

    public int BoundaryPercentage = 25;
    public int Speed = 5;

    public int TopBound;
    public int BottomBound;

    private int _bottomPanTrigger;
    private int _topPanTrigger;
    private int _totalPanRange;


    // Start is called before the first frame update
    void Start()
    {
        _bottomPanTrigger = (int)((BoundaryPercentage / 100.0) * Screen.height);
        _topPanTrigger = Screen.height - _bottomPanTrigger;
        _totalPanRange = _bottomPanTrigger;

    }

    // Update is called once per frame
    void Update()
    {
        Pan();

    }

    public void Pan()
    {
        
        if (Input.mousePosition.y > _topPanTrigger && transform.position.y < TopBound) //move up
        {
            float speedMultiplier = (Input.mousePosition.y - _topPanTrigger) / _totalPanRange;

            transform.Translate(Vector3.up * Time.deltaTime * Speed * speedMultiplier);

        }

        else if (Input.mousePosition.y < _bottomPanTrigger && transform.position.y > BottomBound) //move down
        {
            float speedMultiplier = (_totalPanRange - Input.mousePosition.y) / _totalPanRange;
            transform.Translate(-Vector3.up * Time.deltaTime * Speed * speedMultiplier);
        }


    }
}
