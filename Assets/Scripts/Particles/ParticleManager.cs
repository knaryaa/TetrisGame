using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem[] allEffects;

    private void Start()
    {
        allEffects = GetComponentsInChildren<ParticleSystem>();
    }

    public void EffectPlayFNC()
    {
        foreach (ParticleSystem effect in allEffects)
        {
            effect.Stop();
            effect.Play();
        }
    }
}
