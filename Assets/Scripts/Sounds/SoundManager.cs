using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private AudioSource[] soundEffects;
    [SerializeField] private AudioSource[] vocalClips;
    
    private AudioClip randomMusicClip;

    public bool isMusicPlay = true;
    public bool isEffectPlay = true;

    public IconOnOffManager musicIcon;
    public IconOnOffManager fxIcon;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        randomMusicClip = RandomClipSelect(musicClips);
        BackgroundMusicPlay(randomMusicClip);
    }

    public void PlayVocalEffect()
    {
        if(isEffectPlay)
        {
            AudioSource source = vocalClips[Random.Range(0, vocalClips.Length)];

            source.Stop();
            source.Play();
        }
    }
    
    public void PlaySoundEffect(int whichSound)
    {
        if (isEffectPlay && whichSound < soundEffects.Length)
        {
            soundEffects[whichSound].volume = PlayerPrefs.GetFloat("FXVolume");
            soundEffects[whichSound].Stop();
            soundEffects[whichSound].Play();
        }
    }

    AudioClip RandomClipSelect(AudioClip[] clips)
    {
        AudioClip randomClip = clips[Random.Range(0, clips.Length)];
        return randomClip;
    }

    public void BackgroundMusicPlay(AudioClip musicClip)
    {
        if (!musicClip || !musicSource || !isMusicPlay)
        {
            return;
        }

        musicSource.volume = PlayerPrefs.GetFloat("musicVolume");
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    void MusicUpdateFNC()
    {
        if (musicSource.isPlaying != isMusicPlay)
        {
            if (isMusicPlay)
            {
                randomMusicClip = RandomClipSelect(musicClips);
                BackgroundMusicPlay(randomMusicClip);
            }
            else
            {
                musicSource.Stop();
            }
        }
    }

    public void MusicOnOffFNC()
    {
        isMusicPlay = !isMusicPlay;
        MusicUpdateFNC();
        musicIcon.IconOnOffFNC(isMusicPlay);
    }

    public void FXOnOffFNC()
    {
        isEffectPlay = !isEffectPlay;
        
        fxIcon.IconOnOffFNC(isEffectPlay);
    }
}
