using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldAwardSystem : MonoBehaviour
{
    [SerializeField] ScenesSettings_SO scenesSettings;
    [SerializeField] EnemyBehaviour_SO enemyBehaviour;
    DifficultyScaling enemy;
    private void Start()
    {
        enemy = new DifficultyScaling(GameManager.Instance.stageDifficulty, enemyBehaviour);
    }
    public void GetGold()
    {
        scenesSettings.gold += CalculateAmountOfGold();
    }

    int CalculateAmountOfGold()
    {
        return Random.Range(enemy.minGold, enemy.maxGold);
    }

}
