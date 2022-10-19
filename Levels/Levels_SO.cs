using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Levels_SO", menuName = "Scriptable Object/Levels")]
public class Levels_SO : ScriptableObject
{
    [SerializeField] public List<LevelData_SO> levels;
}
