using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StarMenuTextValue : MonoBehaviour
{
    private TextMeshProUGUI textGoldValue;
    private void Awake()
    {
        textGoldValue = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        VariableChangeHandler();
    }

    private void VariableChangeHandler()
    {
        int value = GameManager.Instance.playerData.GetStarsAmount();
        textGoldValue.text = value.ToString();
    }

}
