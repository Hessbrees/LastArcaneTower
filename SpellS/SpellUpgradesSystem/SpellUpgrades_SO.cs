using Assets.Scripts.SpellS;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellUpgrades_SO_", menuName = "Scriptable Object/SpellUpgrades")]
public class SpellUpgrades_SO : ScriptableObject
{
    public SpellTreeTypes spellTreeType;
    public SpellTypes spellType;
    [Space]
    [SerializeField] private List<SpellUpgrade> spellUpgrades;

    public List<SpellUpgrade> SpellUpgrades => spellUpgrades;

}
