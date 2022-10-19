using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIGoldEarned : MonoBehaviour
{

    private GameObject audioManager;
    private AudioManager audioManagerScript;


    ScenesSettings_SO scenesSettings_SO;
    private TextMeshProUGUI textGoldValue;
    private float getGoldDuration= 5f;
    private bool isCounting;
    private bool isSFXRunning;
    private bool isFontSizeChanging;
    private float fontSizeOrginal;
    private int counter;
    private void Awake()
    {
        scenesSettings_SO = ResourceLoader.LoadScenesSettings_SO();
        textGoldValue = GetComponent<TextMeshProUGUI>();
        isCounting = false;

        //Audio

        audioManager = GameObject.FindGameObjectWithTag("Audio");
        audioManagerScript = audioManager.GetComponent<AudioManager>();

        fontSizeOrginal = textGoldValue.fontSize;
    }

    private void OnEnable()
    {
        if (isCounting == true) isCounting = false;
        audioManagerScript.GetAudioGameOverMusic();
        isSFXRunning = false;
        StartCoroutine(GetGoldPerTime());
    }

    IEnumerator GetGoldPerTime()
    {
        isCounting = true;
        float finalValue = 0;
        textGoldValue.text = "0";
        float getGoldSpeed = scenesSettings_SO.gold/getGoldDuration;
        while (!Mathf.Approximately( scenesSettings_SO.gold, finalValue) && isCounting == true)
        {
            finalValue = Mathf.MoveTowards(finalValue, scenesSettings_SO.gold, getGoldSpeed * Time.deltaTime);
            textGoldValue.text = Mathf.RoundToInt(finalValue).ToString();
            
            if(!isSFXRunning)
            {
                StartCoroutine(GoldSFX());
            }

            yield return null;
        }
        isCounting = false;
    }


    IEnumerator GoldSFX()
    {
        isSFXRunning = true;
        

        while(isCounting)
        {
            if(!isFontSizeChanging)
            StartCoroutine(FontSizeTempChange());

            audioManagerScript.GetAudioCoinSFX();

            yield return new WaitForSeconds(audioManagerScript.GetAudioCoinSFXLenght());
            
        }

        isSFXRunning = false;
    }

    IEnumerator FontSizeTempChange()
    {
        isFontSizeChanging = true;
        for(int i = 1; i <= 4; i++)
        {
            textGoldValue.fontSize = fontSizeOrginal  * (1f + 0.1f*i);
            yield return new WaitForSeconds(0.05f);
        }
        // back to normal
        for (int i = 4; i >= 0; i--)
        {
            textGoldValue.fontSize = fontSizeOrginal * (1f + 0.1f * i);
            yield return new WaitForSeconds(0.05f);
        }
        isFontSizeChanging = false;
    }

}
