using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    public float swipeAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown() {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(firstTouchPosition);
    }

    private void OnMouseUp() {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }

    void CalculateAngle() {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x);
        swipeAngle *= 180/Mathf.PI;
        Debug.Log(swipeAngle);
    }
}
