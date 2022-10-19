using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinusButtonTrigger : EventTrigger, IPointerDownHandler,IPointerUpHandler
{
    //Audio
    private GameObject audioManager;
    private AudioManager audioManagerScript;

    private Slider musicSlider;
    private bool isPressed;
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        StartCoroutine(IncreaseValue());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
    }

    IEnumerator IncreaseValue()
    {
        while (isPressed)
        {
            audioManagerScript.GetAudioButtonClick();
            musicSlider.value -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
    }
    void Start()
    {
        musicSlider = GetComponentInParent<Slider>();
        musicSlider.interactable = false;

        //Audio
        audioManager = GameObject.FindGameObjectWithTag("Audio");
        audioManagerScript = audioManager.GetComponent<AudioManager>();

    }
}

