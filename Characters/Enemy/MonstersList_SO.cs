using UnityEngine;

[CreateAssetMenu(fileName = "MonstersList_SO_", menuName = "Scriptable Object/MonstersList")]
public class MonstersList_SO : ScriptableObject
{
    public Monster_SO[] monsterList;
}

[System.Serializable]
public class Monster_SO
{
    [Header("Monster type Enum")]
    public MonsterTypes monsterEnum;
    [Header("Monster type Prefab File")]
    public GameObject monster;
}
