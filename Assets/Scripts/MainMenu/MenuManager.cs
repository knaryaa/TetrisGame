using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Transform mainMenu, settingsMenu;

    [SerializeField] private AudioSource musicSource;

    [SerializeField] private Slider musicBackSlider, fxSlider;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            musicBackSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            musicBackSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }

        fxSlider.value = 1;
    }

    public void OpenSettingsMenu()
    {
        mainMenu.GetComponent<RectTransform>().DOLocalMoveX(-1200, .5f);
        settingsMenu.GetComponent<RectTransform>().DOLocalMoveX(0, .5f);
    }

    public void CloseSettingsMenu()
    {
        mainMenu.GetComponent<RectTransform>().DOLocalMoveX(0, .5f);
        settingsMenu.GetComponent<RectTransform>().DOLocalMoveX(1200, .5f);
    }

    public void GamePlayFNC()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void ChangeBackVolumeFNC()
    {
        musicSource.volume = musicBackSlider.value;
        PlayerPrefs.SetFloat("musicVolume", musicBackSlider.value);
    }

    public void ChangeFXVolumeFNC()
    {
        PlayerPrefs.SetFloat("FXVolume",fxSlider.value);
    }
}
