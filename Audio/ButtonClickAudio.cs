using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickAudio : MonoBehaviour
{
    private GameObject audioManager;
    private AudioManager audioManagerScript;

    private Button button;
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio");
        audioManagerScript = audioManager.GetComponent<AudioManager>();

        button = GetComponent<Button>();

        button.onClick.AddListener( () => audioManagerScript.GetAudioButtonClick());
  
    }

}
