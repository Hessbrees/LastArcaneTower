using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSpriteChangeSFX : MonoBehaviour
{
    private Button audioButton;
    private Sprite musicOFF;
    private Sprite musicON;

    private GameObject audioManager;
    private AudioManager audioManagerScript;

    private bool isOff;
    private void Awake()
    {
        audioButton = GetComponent<Button>();

        audioManager = GameObject.FindGameObjectWithTag("Audio");
        audioManagerScript = audioManager.GetComponent<AudioManager>();

        musicON = Resources.Load<Sprite>("Art/button_music");
        musicOFF = Resources.Load<Sprite>("Art/button_music_off");
        isOff = false;
    }

    private void Start()
    {
        audioButton.image.sprite = musicON;
        audioButton.onClick.AddListener(ChangeSprite);
    }

    void ChangeSprite()
    {
        if (isOff)
        {
            audioButton.image.sprite = musicON;
            audioManagerScript.UnmuteSFX();
            isOff = false;
        }
        else if (!isOff)
        {
            audioButton.image.sprite = musicOFF;
            audioManagerScript.MuteSFX();
            isOff = true;
        }
    }

}
