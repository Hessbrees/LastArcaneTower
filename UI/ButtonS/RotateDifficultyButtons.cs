using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateDifficultyButtons : MonoBehaviour
{
    private GameObject easy;
    private GameObject normal;
    private GameObject hard;

    private GameObject starNormalLeft;
    private GameObject starNormalRight;

    private GameObject starHardLeft;
    private GameObject starHardRight;

    private Vector3 easyStartingPos = new Vector3(-180, -40, 0);
    private Vector3 normalStartingPos = new Vector3(0, 90, 0);
    private Vector3 HardStartingPos = new Vector3(180, -40, 0);

    private float globalTime = 1.5f;

    int index = 0;
    private void Awake()
    {
        easy = transform.Find("Easy").gameObject;
        normal = transform.Find("Normal").gameObject;
        hard = transform.Find("Hard").gameObject;

        starNormalRight = transform.Find("Table").Find("StarNormalRight").gameObject;
        starNormalLeft = transform.Find("Table").Find("StarNormalLeft").gameObject;
        starHardLeft = transform.Find("Table").Find("StarHardLeft").gameObject;
        starHardRight = transform.Find("Table").Find("StarHardRight").gameObject;

        StarsSetActive(false, starHardLeft);
        StarsSetActive(false, starHardRight);

        StarsSetActive(true, starNormalLeft);
        StarsSetActive(true, starNormalRight);
    }
    private void OnEnable()
    {
        index = 0;
        easy.transform.localPosition = easyStartingPos;
        normal.transform.localPosition = normalStartingPos;
        hard.transform.localPosition = HardStartingPos;
        GameManager.Instance.ChangeDifficulty(Difficulty.normal);

        StarsSetActive(false, starHardLeft);
        StarsSetActive(false, starHardRight);

        StarsSetActive(true, starNormalLeft);
        StarsSetActive(true, starNormalRight);
        
    }
    private void OnDisable()
    {
        LeanTween.cancelAll();
        StarsSetActive(false, starHardLeft);
        StarsSetActive(false, starHardRight);

        StarsSetActive(true, starNormalLeft);
        StarsSetActive(true, starNormalRight);

        Vector3 newScale = new Vector3(1, 1, 1);
        starHardLeft.transform.localScale = newScale;
        starHardRight.transform.localScale = newScale;
        starNormalLeft.transform.localScale = newScale;
        starNormalRight.transform.localScale = newScale;

    }
    public void OnLeftButtonClick()
    {
        // initial normal
        if (LeanTween.isTweening()) return;

        if (index == 0)
        {
            // to hard
            StarEasing(true, starHardLeft, starHardRight);
            Rotate(normal, easy, hard);

            GameManager.Instance.ChangeDifficulty(Difficulty.hard);
        }
        if (index == 1)
        {
            // to easy
            StarEasing(false, starHardLeft, starHardRight);
            StarEasing(false, starNormalLeft, starNormalRight);

            Rotate(hard, normal, easy);
            GameManager.Instance.ChangeDifficulty(Difficulty.easy);
        }
        if (index == 2)
        {
            //to normal
            StarEasing(true, starNormalLeft, starNormalRight);

            Rotate(easy, hard, normal);
            GameManager.Instance.ChangeDifficulty(Difficulty.normal);
        }

        index++;
        if (index == 3) index = 0;
    }

    private void StarEasing(bool active, GameObject starLeft, GameObject starRight)
    {
        if (active)
        {
            Vector3 newScale = new Vector3(1, 1, 1);
            StarsSetActive(true, starLeft);
            StarsSetActive(true, starRight);

            LeanTween.scale(starLeft, newScale, globalTime).setEaseInCirc();
            LeanTween.scale(starRight, newScale, globalTime).setEaseInCirc();

        }
        else if (!active)
        {
            Vector3 newScale = new Vector3(0.3f, 0.3f, 0.3f);
            LeanTween.scale(starLeft, newScale, globalTime).setEaseInCirc().setOnComplete(delegate { StarsSetActive(active, starLeft); }, active);
            LeanTween.scale(starRight, newScale, globalTime).setEaseInCirc().setOnComplete(delegate { StarsSetActive(active, starRight); }, active);
        }
    }
    private void StarsSetActive(bool active, GameObject starObj)
    {
        starObj.SetActive(active);
    }

    private void Rotate(GameObject firstPos, GameObject secondPos, GameObject thirdPos)
    {
        //normal => easy => hard
        Vector3 pos1 = new Vector3(-180, -40, 0);
        Vector3 pos2 = new Vector3(180, -40, 0);
        Vector3 pos3 = new Vector3(0, 90, 0);

        LeanTween.moveLocal(firstPos, pos1, globalTime).setEaseOutQuint();
        LeanTween.moveLocal(secondPos, pos2, globalTime - 0.3f).setEaseInBack();
        LeanTween.moveLocal(thirdPos, pos3, globalTime).setEaseOutQuint();
    }


}
