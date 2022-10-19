using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCheck : MonoBehaviour
{
    private GameObject fadeObject;
    private ScreenChangeFade screenFade;

    void Awake()
    {
        fadeObject = GameObject.FindGameObjectWithTag("Fade");
        screenFade = fadeObject.GetComponent<ScreenChangeFade>();
    }

    private void OnDisable()
    {
       screenFade.Fading(0);
    }
}
