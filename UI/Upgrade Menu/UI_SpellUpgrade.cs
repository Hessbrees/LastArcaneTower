using System.Linq;
using UnityEngine;
using TMPro;

public class UI_SpellUpgrade : MonoBehaviour
{
    [SerializeField] private Player_SO player_SO;
    [SerializeField] SpellUpgrades_SO spellUpgrades;
    [SerializeField] private SpellEffects spellUpgradeType;
    [SerializeField] private TMP_Text upgradeLevelText;
    [SerializeField] private GameObject lockedIcon;

    private SpellUpgrade spellUpgrade;
    private UI_Manager uI_Manager;


    public int CurrentUpgradeLevel => spellUpgrade.currentUpgradeLevel;
    public SpellUpgrade SpellUpgrade => spellUpgrade;

    void Start()
    {
        uI_Manager = UI_Manager.Instance;
     
        spellUpgrade = spellUpgrades.SpellUpgrades.FirstOrDefault(x => x.spellUpgradeType == this.spellUpgradeType);
        ResetValues();
        RefreshUpgradeLevelText();
        SetLockedIcon();
    }

    public void Upgrade()
    {
        if (player_SO.upgradePointsAvailable == 0)
            return;

        spellUpgrade.Upgrade();
        SetLockedIcon();
        RefreshUpgradeLevelText();
    }

    private void ResetValues()
    {
        spellUpgrade.currentUpgradeLevel = 0;
        spellUpgrade.unlocked = false;
    }

    private void SetLockedIcon()
    {
        if (!spellUpgrade.unlocked)
            lockedIcon.SetActive(true);
        else
            lockedIcon.SetActive(false);
    }

    public void RefreshUpgradeLevelText()
    {
        upgradeLevelText.text = spellUpgrade.currentUpgradeLevel.ToString();
    }
}
