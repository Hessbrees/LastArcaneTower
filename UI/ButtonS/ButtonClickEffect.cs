using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonClickEffect : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    private Button button;
    private Vector3 buttonStartingScale;
    private Color startingColor;
    void Start()
    {
        button = GetComponent<Button>();
        startingColor = GetComponent<Image>().color;
        buttonStartingScale = button.transform.localScale;
    }

    void buttonChange()
    {
        Vector3 vectorScale = buttonStartingScale * 1.2f;
        button.transform.localScale = vectorScale;
        button.image.color = new Color32(200, 200, 200, 255) ; 
    }
    void buttonBackToNormal()
    {
        button.transform.localScale = buttonStartingScale;
        button.image.color = startingColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonChange();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonBackToNormal();
    }
}
