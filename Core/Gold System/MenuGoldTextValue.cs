using UnityEngine;
using TMPro;

public class MenuGoldTextValue : MonoBehaviour
{
    private TextMeshProUGUI textGoldValue;
    private void Awake()
    {
        textGoldValue = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        VariableChangeHandler(GameManager.Instance.playerData.GetGoldAmount());   
    }
    private void OnEnable()
    {
        GameManager.Instance.playerData.OnGoldChangeEvent += VariableChangeHandler;
    }
    private void OnDisable()
    {
        GameManager.Instance.playerData.OnGoldChangeEvent -= VariableChangeHandler;
    }

    private void VariableChangeHandler(int value)
    {
        textGoldValue.text = value.ToString();
    }

}
