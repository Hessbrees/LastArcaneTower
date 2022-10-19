using System;

[Serializable]
public class SpellUpgrade 
{
    public delegate void UpgradeEvent();
    public static UpgradeEvent OnUpgrade;

    public SpellEffects spellUpgradeType;
    public string description;
    public int maxUpgradeLevel;
    public int currentUpgradeLevel;
    public int upgradeValuePerLevel;
    public bool unlocked;

    public void Upgrade()
    {
        if (currentUpgradeLevel == maxUpgradeLevel)
            return;

        if (!unlocked)
            unlocked = true;

        currentUpgradeLevel++;

        if (OnUpgrade != null) OnUpgrade();
    }
}
