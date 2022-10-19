using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
public class DifficultyLevelManager : MonoBehaviour
{
    private int levelIndex = 0;
    private TextMeshProUGUI levelText;
    private WaveSystem waveSystem;

    private void Awake()
    {
        levelText = transform.Find("Table").Find("Level").GetComponent<TextMeshProUGUI>();
        waveSystem = GameManager.Instance.waveSystem;
    }
    public void SetLevelIndex(int index)
    {
        levelIndex = index;
        levelText.text = "Level: " + levelIndex.ToString();
        waveSystem.SetLevelIndex(index);
    }

    public void OnCLickStartLevel()
    {
        UI_Manager.Instance.LoadLevel(levelIndex);
    }
}
public class DifficultyScaling
{
    private float _maxHP { get; set; }
    private float _damage { get; set; }
    private float _speed { get; set; }
    private float _regHP { get; set; }
    private int _minGold { get; set; }
    private int _maxGold { get; set; }
    private float _timeBetweenAtack { get; set; }
    
    public float maxHP { get { return _maxHP; } }
    public float damage { get { return _damage; } }
    public float speed { get { return _speed; } }
    public float regHP { get { return _regHP; } }
    public int minGold { get { return _minGold; } }
    public int maxGold { get { return _maxGold; } }
    public float timeBetweenAtack { get { return _timeBetweenAtack; } }
    public DifficultyScaling(Difficulty _difficulty, EnemyBehaviour_SO _enemyBehaviour_SO)
    {
        if (_difficulty == Difficulty.easy)
        {
            _maxHP = Mathf.RoundToInt(0.7f * _enemyBehaviour_SO.maxHP);
            _damage = Mathf.RoundToInt(0.7f * _enemyBehaviour_SO.damage);
            _speed = (float)Math.Round((double)0.7f * _enemyBehaviour_SO.movementSpeeed, 2);
            _minGold = Mathf.RoundToInt(0.5f * _enemyBehaviour_SO.minGoldAward);
            _maxGold = Mathf.RoundToInt(0.5f * _enemyBehaviour_SO.maxGoldAward);
            _regHP = (float)Math.Round((double)0.7f * _enemyBehaviour_SO.regHP, 2);
            _timeBetweenAtack= (float)Math.Round((double)0.7f * _enemyBehaviour_SO.timeBetweenAtack, 2);
        }
        else if (_difficulty == Difficulty.normal)
        {
            _maxHP = _enemyBehaviour_SO.maxHP;
            _damage = _enemyBehaviour_SO.damage;
            _speed = _enemyBehaviour_SO.movementSpeeed;
            _minGold = _enemyBehaviour_SO.minGoldAward;
            _maxGold = _enemyBehaviour_SO.maxGoldAward;
            _regHP = _enemyBehaviour_SO.regHP;
            _timeBetweenAtack = _enemyBehaviour_SO.timeBetweenAtack;
        }
        else if (_difficulty == Difficulty.hard)
        {
            _maxHP = Mathf.RoundToInt(1.7f * _enemyBehaviour_SO.maxHP);
            _damage = Mathf.RoundToInt(1.7f * _enemyBehaviour_SO.damage);
            _speed = (float)Math.Round((double)1.3f * _enemyBehaviour_SO.movementSpeeed, 2);
            _minGold = Mathf.RoundToInt(2.2f * _enemyBehaviour_SO.minGoldAward);
            _maxGold = Mathf.RoundToInt(2.2f * _enemyBehaviour_SO.maxGoldAward);
            _regHP = (float)Math.Round((double)1.3f * _enemyBehaviour_SO.regHP, 2);
            _timeBetweenAtack= (float)Math.Round((double)1.3f * _enemyBehaviour_SO.timeBetweenAtack, 2);
        }
    }
    private int[][] _amountOfMonsters { get; set; }
    public int[][] amountOfMonsters { get { return _amountOfMonsters; } }

    public DifficultyScaling(Difficulty _difficulty, LevelData_SO _levelData_SO)
    {
        if (_difficulty == Difficulty.easy)
        {
            for (int i = 0; i < _levelData_SO.wavesMonsters.Length; i++)
                for (int j = 0; j < _levelData_SO.wavesMonsters[i].waveMonsters.Length; j++)
                {
                    _amountOfMonsters[i][j] = Mathf.RoundToInt( 0.7f * _levelData_SO.wavesMonsters[i].waveMonsters[j].amountOfMonsters);
                }
        }
        if(_difficulty == Difficulty.normal)
        {
            for (int i = 0; i < _levelData_SO.wavesMonsters.Length; i++)
                for (int j =0; j < _levelData_SO.wavesMonsters[i].waveMonsters.Length; j++)
                {
                    _amountOfMonsters[i][j] = Mathf.RoundToInt(1f * _levelData_SO.wavesMonsters[i].waveMonsters[j].amountOfMonsters);
                }
        }
        if(_difficulty == Difficulty.hard)
        {
            for (int i = 0; i < _levelData_SO.wavesMonsters.Length; i++)
                for (int j = 0; j < _levelData_SO.wavesMonsters[i].waveMonsters.Length; j++)
                {
                    _amountOfMonsters[i][j] = Mathf.RoundToInt(1.3f * _levelData_SO.wavesMonsters[i].waveMonsters[j].amountOfMonsters);
                }
        }
    }
}

public class DifficultyToStars
{
    public int stars { get {return _stars; } }
    private int _stars;
    public DifficultyToStars(Difficulty difficulty)
    {
        if (difficulty == Difficulty.easy) _stars = 1;
        else if (difficulty == Difficulty.normal) _stars = 2;
        else if (difficulty == Difficulty.hard) _stars = 3;
    }  
}

public enum Difficulty
{
    easy,
    normal,
    hard
}
