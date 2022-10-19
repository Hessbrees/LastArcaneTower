using Assets.Scripts.SpellS;
using UnityEngine;

[CreateAssetMenu(fileName = "SpellStats_SO_", menuName = "Scriptable Object/SpellStats")]
public class SpellStats_SO : ScriptableObject
{
    public string spellName;
    public string description;
    public int dmg;
    public int manaCost;
    public float cooldown;
    public int hp;
    public bool hasChild;

    [HideInInspector] public SpellTypes SpellType => spellUpgrades_SO.spellType;
    [HideInInspector] public SpellTreeTypes SpellTreeType => spellUpgrades_SO.spellTreeType;

    [SerializeField] public SpellUpgrades_SO spellUpgrades_SO;
}
