using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public AudioMixerGroup _mixer;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;    
    [SerializeField] private Slider _effectsSlider;  

    private void Start()
    {
        _musicSlider.onValueChanged.AddListener(call => { ChangeMusicVolume(); });
        _effectsSlider.onValueChanged.AddListener(call => { ChangeEffectsVolume(); });
        _masterSlider.onValueChanged.AddListener(call => { ChangeMasterVolume(); });
    }

    public void ChangeMusicVolume()
    {
        _mixer.audioMixer.SetFloat(name: "MasterVolume", value: Mathf.Lerp(-80, 0, _musicSlider.value));
    }

    public void ChangeEffectsVolume()
    {
        _mixer.audioMixer.SetFloat(name: "EffectsVolume", value: Mathf.Lerp(-80, 0, _effectsSlider.value));
    }

    public void ChangeMasterVolume()
    {
        _mixer.audioMixer.SetFloat(name: "MasterVolume", value: Mathf.Lerp(-80, 0, _masterSlider.value));
    }
}