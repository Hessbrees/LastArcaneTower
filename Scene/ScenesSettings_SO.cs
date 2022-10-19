using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScenesSettings_SO_", menuName = "Scriptable Object/ScenesSetting")]
public class ScenesSettings_SO : ScriptableObject
{
    public LevelData_SO levelData_SO;
    public Sprite background;
    public int gold;
    public int levelScore;
}
