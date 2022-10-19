using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenChangeFade : MonoBehaviour
{
    private CanvasGroup faderCanvasGroup;
    private bool isFading;
    private Image faderImage = null;
    private float fadeDuration = 1f;
    private void Start()
    {
        faderCanvasGroup = GetComponent<CanvasGroup>();
        faderImage = GetComponent<Image>();

        faderImage.color = new Color(0, 0, 0, 1);
    }
    
    public void Fading(float finalAlpha)
    {
        if (isFading == true) isFading = false;
        StartCoroutine(Fade(finalAlpha));
    }
    
    IEnumerator Fade(float finalAlpha)
    {
        isFading = true;
        faderCanvasGroup.blocksRaycasts = true;

        faderCanvasGroup.alpha = 1f;

        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha ) && isFading == true)
        {
            if (faderCanvasGroup.alpha != finalAlpha)
            {
                faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);
            }
                yield return null;
        }

        isFading = false;
        faderCanvasGroup.blocksRaycasts = false;
    }


}
