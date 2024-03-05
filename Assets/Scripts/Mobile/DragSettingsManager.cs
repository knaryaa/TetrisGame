using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSettingsManager : MonoBehaviour
{
    private GameManager gameManager;
    private TouchManager touchManager;

    public Slider touchSlider;
    public Slider swipeSlider;
    public Slider speedSlider;

    private void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        touchManager = GameObject.FindObjectOfType<TouchManager>();

        if (touchSlider != null)
        {
            touchSlider.value = 100;
            touchSlider.minValue = 50;
            touchSlider.maxValue = 150;
        }
        
        if (swipeSlider != null)
        {
            swipeSlider.value = 50;
            swipeSlider.minValue = 20;
            swipeSlider.maxValue = 250;
        }
        
        if (speedSlider != null)
        {
            speedSlider.value = 0.15f;
            speedSlider.minValue = 0.05f;
            speedSlider.maxValue = 0.5f;
        }
        
    }

    public void UpdateSettingsPanel()
    {
        if (touchSlider != null && touchManager != null)
        {
            touchManager.minDragDistance = (int)touchSlider.value;
        }

        if (swipeSlider != null && touchManager != null)
        {
            touchManager.minSwipeDistance = (int)swipeSlider.value;
        }
        if (speedSlider != null && gameManager != null)
        {
            gameManager.minTouchTime = speedSlider.value;
        }
    }
}
