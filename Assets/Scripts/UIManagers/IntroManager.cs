using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IntroManager : MonoBehaviour
{
    public GameObject[] numbers;

    public GameObject numbersTransform;
    public GameObject holdTransform;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    private void Start()
    {
        StartCoroutine(StartNumbersRoutine());
    }

    IEnumerator StartNumbersRoutine()
    {
        yield return new WaitForSeconds(.1f);
        numbersTransform.GetComponent<RectTransform>().DORotate(Vector3.zero, .3f).SetEase(Ease.OutBack);
        numbersTransform.GetComponent<CanvasGroup>().DOFade(1, .3f);

        yield return new WaitForSeconds(.2f);
    
        int count = 0;

        while (count < numbers.Length)
        {
            numbers[count].GetComponent<RectTransform>().DOLocalMoveY(0, .5f);
            numbers[count].GetComponent<CanvasGroup>().DOFade(1, .5f);

            numbers[count].GetComponent<RectTransform>().DOScale(1f, .3f).SetEase(Ease.OutBounce).OnComplete(()=>
                numbers[count].GetComponent<RectTransform>().DOScale(.5f,.3f).SetEase(Ease.InBack));
            yield return new WaitForSeconds(1.5f);
            
            count++;
            numbers[count-1].GetComponent<RectTransform>().DOLocalMoveY(75f, .5f);
            yield return new WaitForSeconds(.1f);
        }

        numbersTransform.GetComponent<CanvasGroup>().DOFade(0, .5f).OnComplete(() =>
            {
                numbersTransform.transform.parent.gameObject.SetActive(false);
                gameManager.StartGameFNC();
                holdTransform.GetComponent<CanvasGroup>().DOFade(1f, .5f);
            });
    }
}