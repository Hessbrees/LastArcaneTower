using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour
{
    private AudioMixer audioMixer;
    private Slider slider;
    const string mixerMusic = "MusicVolume";

    private void Awake()
    {
        audioMixer = Resources.Load<AudioMixer>("Mixer");
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(VolumeChange);
    }
    void VolumeChange(float sliderValue)
    {
        if (sliderValue == 0f) sliderValue = 0.0001f;
        audioMixer.SetFloat(mixerMusic, Mathf.Log10(sliderValue) * 20);
    }
}
