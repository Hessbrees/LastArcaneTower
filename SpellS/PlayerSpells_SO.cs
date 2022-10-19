using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSpells_SO", menuName = "Scriptable Object/PlayerSpells")]
public class PlayerSpells_SO : ScriptableObject
{
    [SerializeField] public SpellMonoBehaviour projectileSpell;
    [SerializeField] public SpellMonoBehaviour skydropSpell;
    [SerializeField] public SpellMonoBehaviour wallSpell;
    [SerializeField] public List<SpellMonoBehaviour> availableSpecialSpells;
}
