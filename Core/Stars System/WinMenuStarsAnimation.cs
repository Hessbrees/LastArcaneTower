using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinMenuStarsAnimation : MonoBehaviour
{
    private Sprite starSprite0;
    private Sprite starSprite1;
    private Sprite starSprite2;
    private Sprite starSprite3;

    private int levelStars;

    private Image levelSprite;
    private GameObject gameManager;
    private WaveSystem waveSystem;
    private GameObject star1;
    private GameObject star2;
    private GameObject star3;
    private bool isAnimating = false;
    private int uniqueID;

    private void Awake()
    {
        starSprite0 = Resources.Load<Sprite>("Art/Stars/star_1");
        starSprite1 = Resources.Load<Sprite>("Art/Stars/star_2");
        starSprite2 = Resources.Load<Sprite>("Art/Stars/star_3");
        starSprite3 = Resources.Load<Sprite>("Art/Stars/star_4");

        gameManager = GameObject.FindGameObjectWithTag("GameManager").gameObject;
        waveSystem = gameManager.transform.Find("WaveSystem").GetComponent<WaveSystem>();

        star1 = transform.Find("starAnimation").gameObject;
        star2 = transform.Find("starAnimation2").gameObject;
        star3 = transform.Find("starAnimation3").gameObject;
    }

    private void OnEnable()
    {
        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);

        levelSprite = transform.Find("Stars").GetComponent<Image>();
        levelStars = waveSystem.GetStarsAmountInthisLevel();
        StartCoroutine(StarsSequence(levelStars));

    }
    IEnumerator StarsSequence(int levelStars)
    {

        ChangeStarSprite(0);
        if (levelStars >= 1)
            StartCoroutine(starAnimation(-140, 70, 1, star1));
        yield return new WaitWhile(() => isAnimating);
        if (levelStars >= 2)
            StartCoroutine(starAnimation(140, 70, 2, star2));
        yield return new WaitWhile(() => isAnimating);
        if (levelStars >= 3)
            StartCoroutine(starAnimation(0, 125, 3, star3));
        yield return new WaitWhile(() => isAnimating);

    }
    private void ChangeStarSprite(int levelStars)
    {
        if (levelStars == 0) levelSprite.sprite = starSprite0;
        else if (levelStars == 1) levelSprite.sprite = starSprite1;
        else if (levelStars == 2) levelSprite.sprite = starSprite2;
        else if (levelStars == 3) levelSprite.sprite = starSprite3;
    }

    IEnumerator starAnimation(float p1, float p2, int levelStars, GameObject star)
    {
        LeanTween.cancelAll();
        isAnimating = true;
        star.SetActive(true);
        float r = 500;
        float theta = 0;
        int k = 1;
        int maxk = 800;
        while (true)
        {
            if (!LeanTween.isTweening(uniqueID))
            {
                // distanse 
                if (k < 0.3 * maxk) r = 0.995f * r;
                //else if (k < 0.6 * maxk) r = 0.99f * r;
                //else if (k < 0.7 * maxk) r = 0.96f * r;
                else if (k < 0.9 * maxk) r = 0.98f * r;
                else r = 0.96f * r;

                // degree 
                theta = (k * 0.5f * Mathf.PI) / 20;

                SpiralCalculation(r, theta,k,maxk, star, p1, p2);
                k++;

                if (k > maxk) break;
            }

            yield return null;
        }
        LeanTween.cancelAll();
        Vector3 newVector = new Vector3(p1, p2);
        star.transform.localPosition = newVector;
        star.transform.localRotation = new Quaternion(0, 0, 0, 0);
        isAnimating = false;
    }

    private void SpiralCalculation(float r, float theta,int k,int maxk, GameObject star, float p1, float p2)
    {
        float y = Mathf.Sin(theta) * r;
        float x = Mathf.Cos(theta) * r;

        Vector3 newVector = new Vector3(x + p1, y + p2);
        star.transform.localPosition = newVector;
        uniqueID = LeanTween.moveLocal(star, newVector, 0.006f).uniqueId;
        //LeanTween.rotateLocal(star, new Vector3(0,0,Mathf.Abs(10* 12 * counter)), 1f / (Mathf.Log10(2 * counter + 1) + 2f));

    }

}
