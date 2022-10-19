using Assets.Scripts.SpellS;
using System.Collections;
using TMPro;
using UnityEngine;

public class UI_GameController : MonoBehaviour
{
    private UI_Manager uI_Manager;

    private GameObject devModeCanvas;
    private GameObject damageTextPrefab;
    private GameObject specialSpellModeCanvas;
    private TMP_Text specialSpellModeTitle;

    private string originalSpecialSpellModeTitle = "Special Spell Mode";

    private void Awake()
    {
        devModeCanvas = transform.Find("GameEffects").Find("DevModeCanvas").gameObject;
        damageTextPrefab = devModeCanvas.transform.Find("DamageText").gameObject;
        //specialSpellModeCanvas = transform.Find("SpecialSpellMode").Find("SpecialSpellModeCanvas").gameObject;   
        //specialSpellModeTitle = specialSpellModeCanvas.transform.Find("Title").GetComponent<TMP_Text>();
    }

    void Start()
    {
        uI_Manager = UI_Manager.Instance;

        EnemyHPMPControl.OnDOT_Damage += ShowDamageText;
        EnemyHPMPControl.OnProjectileDamage += ShowDamageText;
        EnemyHPMPControl.OnAOE_Damage += ShowDamageText;

        EnemyHPMPControl.OnKnockback += ShowEffectText;
    }
   
    public void OnGestureNotRecognized()
    {
        //specialSpellModeTitle.text = Settings.GestureErrorMessage;
        //StartCoroutine(RefreshTextAfterDelay(1f));
    }

    public void OnGestureRecognizedWithSpellName(string spellName)
    {
        //specialSpellModeTitle.text = spellName;
        //StartCoroutine(SpecialSpellModeDeactivationDelay(1f));
    }

    private IEnumerator SpecialSpellModeDeactivationDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        specialSpellModeTitle.text = originalSpecialSpellModeTitle;
        specialSpellModeCanvas.SetActive(false);
    }

    private IEnumerator RefreshTextAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        specialSpellModeTitle.text = originalSpecialSpellModeTitle;
    }

    private void ShowEffectText(Vector2 force, Rigidbody2D rb, SpellTypes spellType, SpellEffects spellEffect)
    {
        if (uI_Manager.ShowDamageText)
            InstantiateSpellEffectText(damageTextPrefab, rb, force, spellType, spellEffect);
    }

    private void ShowDamageText(int damage, Rigidbody2D rb, SpellTypes spellType, SpellEffects spellEffect)
    {
        if (uI_Manager.ShowDamageText)
            InstantiateSpellEffectText(damageTextPrefab, damage, rb, spellType, spellEffect);
    }

    private void InstantiateSpellEffectText(GameObject prefab, int damage, Rigidbody2D rb, SpellTypes spellType, SpellEffects spellEffect)
    {
        var newDamageTextPrefab = Instantiate(prefab, rb.position, Quaternion.identity, devModeCanvas.transform);
        ApplyTextJump(newDamageTextPrefab);
        ChangeText(damage, spellType, newDamageTextPrefab, spellEffect);
        Destroy(newDamageTextPrefab, 0.5f);
    }

    private void InstantiateSpellEffectText(GameObject prefab, Rigidbody2D rb, Vector2 force, SpellTypes spellType, SpellEffects spellEffect)
    {
        var newDamageTextPrefab = Instantiate(prefab, rb.position, Quaternion.identity, devModeCanvas.transform);
        ApplyTextJump(newDamageTextPrefab);
        ChangeText(spellType, newDamageTextPrefab, spellEffect);
        Destroy(newDamageTextPrefab, 0.5f);
    }

    private void ChangeText(int damage, SpellTypes spellType, GameObject damageText, SpellEffects spellEffect)
    {
        switch (spellEffect)
        {
            case SpellEffects.Damage:
                damageText.GetComponent<TMP_Text>().text = $"-{damage}HP (Spell {spellEffect})";
                break;
            default:
                damageText.GetComponent<TMP_Text>().text = $"-{damage} HP ({spellEffect})";
                break;
        }
    }

    private void ChangeText(SpellTypes spellType, GameObject damageText, SpellEffects spellEffect)
    {
        damageText.GetComponent<TMP_Text>().text = $"{spellEffect}";
    }

    private void ApplyTextJump(GameObject newDamageTextPrefab)
    {
        newDamageTextPrefab.GetComponent<Rigidbody2D>().isKinematic = false;
        newDamageTextPrefab.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 100));
    }
}
