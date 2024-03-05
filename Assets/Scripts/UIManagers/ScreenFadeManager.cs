using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScreenFadeManager : MonoBehaviour
{
    public float startAlpha = 1f;
    public float endAlpha = 0f;

    public float waitTime = 0f;
    public float fadeTime = 1f;

    private void Start()
    {
        GetComponent<CanvasGroup>().alpha = startAlpha;

        StartCoroutine((FadeRoutineFNC()));
    }

    IEnumerator FadeRoutineFNC()
    {
        yield return new WaitForSeconds(waitTime);

        GetComponent<CanvasGroup>().DOFade(endAlpha, fadeTime);
    }
}
