using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpecialSpellsList_SO_", menuName = "Scriptable Object/SpecialSpellsList")]
public class SpecialSpellsList_SO : ScriptableObject
{
    [SerializeField] public List<GameObject> specialSpells;
}
