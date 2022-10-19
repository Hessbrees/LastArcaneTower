using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextLogoEffects : MonoBehaviour
{

    private int characterDirectionY;
    private void OnEnable()
    {
        TextEffectChange();
    }
    void TextEffectChange()
    {
        //foreach (Transform o in transform)
            StartCoroutine(ShakeEffect(transform));
    }

    IEnumerator ShakeEffect(Transform gameObj)
    {
        if (Random.Range(0, 2) == 1) characterDirectionY = 1;
        else characterDirectionY = -1;

        float movementScale = 0.03f;
        
        // speed = movementScale * randDirectionY * TimeValuePerSecond(100/12f)
        // 5/s = 0.02 * 30 *8.3[s]
        
        float startPosY = gameObj.transform.localPosition.y;

        while (true)
        {

            int randDirectionY = Random.Range(20, 30);

            for (int j = 0; j <= randDirectionY; j++)
            {
                gameObj.transform.localPosition += new Vector3(0, j * characterDirectionY * movementScale, 0);

                yield return new WaitForSeconds(0.12f);
                if (gameObj.transform.localPosition.y > startPosY + 10)
                {
                    if (characterDirectionY > 0)
                    {
                        characterDirectionY = -1;
                        yield return new WaitForSeconds(1f);
                    }
                }
                if (gameObj.transform.localPosition.y < startPosY - 10)
                {
                    if (characterDirectionY < 0)
                    {
                        characterDirectionY = 1;
                        yield return new WaitForSeconds(1f);
                    }
                }
            }


        }

    }



}
