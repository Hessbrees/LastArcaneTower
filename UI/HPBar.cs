using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPBar : MonoBehaviour
{
    private TextMeshProUGUI HPtextValue;
    private Slider hpSlider;
    [SerializeField] HPMP_SO hPMP_SO;
    void Start()
    {
        HPtextValue = GetComponentInChildren(typeof(TextMeshProUGUI)) as TextMeshProUGUI;
        hpSlider = GetComponentInChildren(typeof(Slider)) as Slider;
    }

    void Update()
    {
        HPtextValue.text = hPMP_SO.CurrentHP + "/" + hPMP_SO.MaxHP;
        hpSlider.value = SliderValueCalculate(hPMP_SO.CurrentHP, hPMP_SO.MaxHP);
    }

    float SliderValueCalculate(float currentHP, float maxHP) => currentHP / maxHP;

}
