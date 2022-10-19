using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WavesProgressSlider : MonoBehaviour
{

    [SerializeField] private GameObject skullSpritePrefab;
    [SerializeField] private GameObject starSpritePrefab;
    private RectTransform rt;
    [NonSerialized] public Slider waveSlider;
    WaveSystem waveSystem;
    float newFillPosition;
    float timeToCompleteLevel = 0;
    float timer = 0;
    bool startTimer = false;
    bool isEasing = false;
    bool waitForMe = false;
    int spawnedIndex = -1;
    int nextIndex = 0;
    int easingID = 0;
    private GameObject SkullParentObj;
    float[] skullTime;
    private void Awake()
    {
        rt = transform as RectTransform;
        waveSlider = GetComponent(typeof(Slider)) as Slider;
        waveSystem = GameObject.FindGameObjectWithTag("GameManager").GetComponentInChildren<WaveSystem>();
        SkullParentObj = transform.Find("SkullsSprites").gameObject;

    }
    private void OnEnable()
    {
        timer = 0;
        waveSlider.value = 0;
        timeToCompleteLevel = waveSystem.GetTimeToSpawnNextWaves(0, true);
        waveSystem.OnSpawnChangeFillValueEvent += CalculateVectorPosition;
        waveSystem.OnEndSpawningChangeEvent += StarEasingAnimation;
        CreateWaveSkullSprites();

    }
    private void OnDisable()
    {
        waveSystem.OnSpawnChangeFillValueEvent -= CalculateVectorPosition;
        nextIndex = 0;
        StopAllCoroutines();
        LeanTween.cancelAll();
        foreach (Transform child in SkullParentObj.transform) GameObject.Destroy(child.gameObject);
    }

    void CreateWaveSkullSprites()
    {
        skullTime = new float[waveSystem.GetAmountOfWaves() + 2];
        int numberOfWaves = waveSystem.GetAmountOfWaves();
        spawnedIndex = 0;
        var newSkull2 = Instantiate(skullSpritePrefab) as GameObject;
        newSkull2.transform.parent = transform.Find("SkullsSprites");
        newSkull2.transform.localPosition = new Vector3(-rt.rect.width / 2, 0, 0);
        newSkull2.transform.localScale = skullSpritePrefab.transform.localScale;
        skullTime[spawnedIndex] = 0;
        for (int i = 0; i < numberOfWaves; i++)
        {
            spawnedIndex++;
            float getNewTime = waveSystem.GetTimeToSpawnNextWaves(i, false);
            float calculateNewPosition = (getNewTime / timeToCompleteLevel) * rt.rect.width;
            Vector3 position = new Vector3(calculateNewPosition - rt.rect.width / 2, 0, 0);

            var newSkull = Instantiate(skullSpritePrefab) as GameObject;
            newSkull.transform.parent = transform.Find("SkullsSprites");
            newSkull.transform.localPosition = position;
            newSkull.transform.localScale = skullSpritePrefab.transform.localScale;
            skullTime[spawnedIndex] = getNewTime / timeToCompleteLevel;
        }
        spawnedIndex++;
        var newStar = Instantiate(starSpritePrefab) as GameObject;
        newStar.transform.parent = transform.Find("SkullsSprites");
        newStar.transform.localPosition = new Vector3(rt.rect.width / 2, 0, 0);
        newStar.transform.localScale = starSpritePrefab.transform.localScale;
        skullTime[spawnedIndex] = 1;
    }
    void CalculateVectorPosition()
    {
        bool startTimer = true;
        if (startTimer) SpriteAnimation(nextIndex);
    }
    void SpriteAnimation(int index)
    {
        if (LeanTween.isTweening()) isEasing = false;
        else isEasing = true;

        if (index < SkullParentObj.transform.childCount)
        {
            LeanTween.scale(SkullParentObj.transform.GetChild(index).gameObject, new Vector3(1.5f, 1.5f), 3f).setEaseInBounce();

        }

        if (isEasing)
        {
            if (index + 1 < skullTime.Length)
            {
                easingID = LeanTween.value(skullTime[index], skullTime[index + 1],
                      (skullTime[index + 1] - skullTime[index]) * timeToCompleteLevel).setOnUpdate(IncreaseValue).uniqueId;
            }
        }
        else
        {
            LeanTween.cancel(easingID);
            waveSlider.value = skullTime[index];

            if (index + 1 < skullTime.Length)
            {
                easingID = LeanTween.value(skullTime[index], skullTime[index + 1],
                      (skullTime[index + 1] - skullTime[index]) * timeToCompleteLevel).setOnUpdate(IncreaseValue).uniqueId;
            }
        }

        if (index + 1 < skullTime.Length - 1) nextIndex++;

    }
    void StarEasingAnimation() => StartCoroutine(WaitToEnd());
    IEnumerator WaitToEnd()
    {
        yield return new WaitWhile(() => waveSlider.value <= 0.99f);
        LeanTween.scale(SkullParentObj.transform.GetChild(spawnedIndex).gameObject, new Vector3(1.5f, 1.5f), 3f).setEaseInBounce();
    }
    void IncreaseValue(float value)
    {
        waveSlider.value = value;
        if (waveSlider.value == 1) LeanTween.scale(SkullParentObj.transform.GetChild(SkullParentObj.transform.childCount - 1).gameObject, new Vector3(1.5f, 1.5f), 3f).setEaseInBounce();
    }


}
