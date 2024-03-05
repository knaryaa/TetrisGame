using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconOnOffManager : MonoBehaviour
{
    public Sprite openIcon;
    public Sprite closedIcon;

    private Image iconImg;

    public bool defaultIconState;

    private void Start()
    {
        iconImg = GetComponent<Image>();

        iconImg.sprite = (defaultIconState) ? openIcon : closedIcon;
    }

    public void IconOnOffFNC(bool iconState)
    {
        if (!iconImg || !openIcon || !closedIcon)
        {
            return;
        }

        iconImg.sprite = (iconState) ? openIcon : closedIcon;
    }
}
