using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData_SO_", menuName = "Scriptable Object/LevelData")]
public class LevelData_SO : ScriptableObject
{
    public WavesMonster[] wavesMonsters;
}

[System.Serializable]
public struct WavesMonster
{
    [Header("Spawn monsters now!")]
    public bool isSpawningNow;
    [Header("Current wave")]
    public WaveMonster[] waveMonsters;
}
[System.Serializable]
public struct WaveMonster
{
    [Header("Monster type")]
    public MonsterTypes monster;
    [Header("Amount")]
    public int amountOfMonsters;
    [Header("Time between next monster spawn (s)")]
    public float timeBetweenNewMonster;
    [Header("Time to spawn this wave (s)")]
    public float timeBetweenNewWave;
    [Header("Wave spawn location")]
    public WaveLocationSpawns waveLocationSpawn;
}