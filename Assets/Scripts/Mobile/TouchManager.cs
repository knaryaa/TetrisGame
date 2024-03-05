using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Serialization;

public class TouchManager : MonoBehaviour
{
    public delegate void TouchEventDelegate(Vector2 swipePos);

    public static event TouchEventDelegate DragEvent;
    public static event TouchEventDelegate SwipeEvent;
    public static event TouchEventDelegate TapEvent;

    private Vector2 touchMovement;

    [Range(50, 250)] public int minDragDistance = 100;
    
    [Range(20, 250)] public int minSwipeDistance = 50;

    private float clickMaxTime = 0f;
    public float screenClickTime = .1f;

    

    void ClickFNC()
    {
        if (TapEvent != null)
        {
            TapEvent(touchMovement);
        }
    }

    void SwipeFNC()
    {
        if (DragEvent != null)
        {
            DragEvent(touchMovement);
        }
    }

    void SwipeEndFNC()
    {
        if (SwipeEvent != null)
        {
            SwipeEvent(touchMovement);
        }
    }


    string SwipeViewFNC(Vector2 swipeMovement)
    {
        string direction = "";
        
        if(Mathf.Abs(swipeMovement.x)> Mathf.Abs(swipeMovement.y))
        {
            direction = (swipeMovement.x >= 0) ? "right" : "left";
        }
        else
        {
            direction = (swipeMovement.y >= 0) ? "up" : "down";
        }
        return direction;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                touchMovement = Vector2.zero;
                clickMaxTime = Time.time + screenClickTime; 
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                touchMovement += touch.deltaPosition;

                if (touchMovement.magnitude > minDragDistance)
                {
                    SwipeFNC();
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (touchMovement.magnitude > minSwipeDistance)
                {
                    SwipeEndFNC();
                }
                else if(Time.time < clickMaxTime)
                {
                    ClickFNC();
                }
            }
        }
    }
}
