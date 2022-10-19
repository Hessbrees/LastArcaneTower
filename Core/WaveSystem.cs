using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    public delegate void OnSpawnChangeFillValueDelegate();
    public event OnSpawnChangeFillValueDelegate OnSpawnChangeFillValueEvent;
    public delegate void OnEndSpawningChangeDelegate();
    public event OnEndSpawningChangeDelegate OnEndSpawningChangeEvent;

    GameManager gameManager;
    [SerializeField] MonstersList_SO enemyTypeList;
    [SerializeField] LevelData_SO levelWavesSO;
    [SerializeField] ScenesSettings_SO scenesSettingSO;
    [SerializeField] GameObject wavesProgressObj;
    private WavesProgressSlider wavesProgressBar;
    private float nextSpawnTime = 0;
    private Vector3 LeftSpawn = new Vector3(11f, 0, 0);
    private Vector3 RightSpawn = new Vector3(-11f, 0, 0);

    private int index = 0;
    private bool isGameOver = false;
    private bool isGameWin = false;

    private int levelIndex = 0;


    private void Awake()
    {
        wavesProgressBar = wavesProgressObj.GetComponent<WavesProgressSlider>();
        levelWavesSO = scenesSettingSO.levelData_SO;
        for (int j = 0; j < levelWavesSO.wavesMonsters.Length; j++)
        {
            levelWavesSO.wavesMonsters[j].isSpawningNow = false;
        }
        isGameOver = false;
    }

    public void SetLevelIndex(int level) => levelIndex = level;

    public float GetTimeToSpawnNextWaves(int waveIndex, bool max)
    {
        if (max) waveIndex = levelWavesSO.wavesMonsters.Length;

        int waveCounter = 0;

        float finalTime = 0;

        foreach (var waves in levelWavesSO.wavesMonsters)
        {
            if (waveCounter > waveIndex) return finalTime;
            float time = 0;
            foreach (var wave in waves.waveMonsters)
            {
                float tempTime = wave.amountOfMonsters * wave.timeBetweenNewMonster;
                if (tempTime > time) time = tempTime;
            }
            finalTime += time;
            waveCounter++;
        }
        return finalTime;
    }
    public int GetAmountOfWaves() => levelWavesSO.wavesMonsters.Length -1;
    void GetSpawnTime()
    {
        if (OnSpawnChangeFillValueEvent != null) OnSpawnChangeFillValueEvent();
    }
    void StarSpriteEasingScale()
    {
        if (OnEndSpawningChangeEvent != null) OnEndSpawningChangeEvent();
    }


    IEnumerator WaveLogic(int wavesIndex)
    {
        if(index < levelWavesSO.wavesMonsters.Length -1) index++;
        bool isSpawningNow = false;
        int currentWaveIndex = 0;

        while (true)
        {
            WaveMonster waveMonster = levelWavesSO.wavesMonsters[wavesIndex].waveMonsters[currentWaveIndex];
            if (levelWavesSO.wavesMonsters[wavesIndex].isSpawningNow)
            {

                if (!isSpawningNow)
                {
                    StartCoroutine(SpawnWave(currentWaveIndex, wavesIndex));
                    isSpawningNow = true;
                }

                // change list waves index value
                if (wavesIndex < levelWavesSO.wavesMonsters.Length - 1 &&
                    currentWaveIndex == levelWavesSO.wavesMonsters[wavesIndex].waveMonsters.Length - 1)
                {
                    break;
                }

                // change list current wave index value
                if (currentWaveIndex < levelWavesSO.wavesMonsters[wavesIndex].waveMonsters.Length - 1 && isSpawningNow)
                {
                    isSpawningNow = false;
                    currentWaveIndex++;
                }
                else if (currentWaveIndex == levelWavesSO.wavesMonsters[wavesIndex].waveMonsters.Length - 1 &&
                    wavesIndex == levelWavesSO.wavesMonsters.Length - 1)
                {
                    // All enemies are spawned
                    StarSpriteEasingScale();
                    StartCoroutine(WinTheGame());
                    break;
                }
                yield return new WaitForSeconds(waveMonster.timeBetweenNewWave);
            }
            yield return null;
        }
    }
    IEnumerator WinTheGame()
    {
        DifficultyToStars DTS = new DifficultyToStars(GameManager.Instance.stageDifficulty);
        while (true)
        {
            if (transform.childCount == 0)
            {
                yield return new WaitWhile(() => wavesProgressBar.waveSlider.value < 1);
                yield return new WaitForSeconds(2f);
                
                GameManager.Instance.ChangeStarsAmount(levelIndex, DTS.stars);
                GameManager.Instance.GameWin();
                
                break;
            }
            yield return null;
        }
    }
    public int GetStarsAmountInthisLevel() => new DifficultyToStars(GameManager.Instance.stageDifficulty).stars;

    IEnumerator SpawnWave(int thisWaveIndex, int wavesIndex)
    {
        WaveMonster waveMonster = levelWavesSO.wavesMonsters[wavesIndex].waveMonsters[thisWaveIndex];
        float spawningTime = levelWavesSO.wavesMonsters[wavesIndex].waveMonsters[thisWaveIndex].timeBetweenNewMonster;
        int i = 0;
        while (i < waveMonster.amountOfMonsters)
        {
            Spawn(waveMonster);
            yield return new WaitForSeconds(spawningTime);
            i++;
        }
    }
    public void EndTheGame()
    {
        isGameOver = true;
        StopAllCoroutines();
        DestroyAllMonters();
    }

    public void StartTheGame()
    {
        //Reload all elements
        isGameOver = false;
        nextSpawnTime = 0;
        index = 0;
        levelWavesSO = scenesSettingSO.levelData_SO;
        for (int j = 0; j < levelWavesSO.wavesMonsters.Length; j++)
        {
            levelWavesSO.wavesMonsters[j].isSpawningNow = false;
        }
    }

    private void DestroyAllMonters()
    {
        foreach (Transform child in transform) GameObject.Destroy(child.gameObject);
    }
    void Spawn(WaveMonster monster)
    {
        if (GetMonsterType(monster.monster) == null) return;
        GameObject spawnEnemyRight = null;
        GameObject spawnEnemyLeft = null;

        if (monster.waveLocationSpawn == WaveLocationSpawns.Right)
        {
            spawnEnemyRight = Instantiate(GetMonsterType(monster.monster)) as GameObject;
        }
        else if (monster.waveLocationSpawn == WaveLocationSpawns.Left)
        {
            spawnEnemyLeft = Instantiate(GetMonsterType(monster.monster)) as GameObject;
        }
        else if (monster.waveLocationSpawn == WaveLocationSpawns.Both)
        {
            spawnEnemyLeft = Instantiate(GetMonsterType(monster.monster)) as GameObject;
            spawnEnemyRight = Instantiate(GetMonsterType(monster.monster)) as GameObject;
        }
        else
        {
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                spawnEnemyRight = Instantiate(GetMonsterType(monster.monster)) as GameObject;
            }
            else if (rand == 1)
            {
                spawnEnemyLeft = Instantiate(GetMonsterType(monster.monster)) as GameObject;
            }
            else return;
        }

        if(spawnEnemyLeft != null)
        {
            spawnEnemyLeft.transform.parent = transform;
            spawnEnemyLeft.transform.localPosition = LeftSpawn + MonsterStartingPosition(monster); ;
            spawnEnemyLeft.transform.localScale = GetMonsterType(monster.monster).transform.localScale;
            if (spawnEnemyLeft.GetComponent<EnemyControl>() != null)
            {
                spawnEnemyLeft.GetComponent<EnemyControl>().playerStartingVector = -1;
                spawnEnemyLeft.GetComponent<EnemyControl>().SetDirection();
            }
            else if (spawnEnemyLeft.GetComponent<FlyEnemyControl>() != null)
            {
                spawnEnemyLeft.GetComponent<FlyEnemyControl>().playerStartingVector = -1;
                spawnEnemyLeft.GetComponent<FlyEnemyControl>().SetDirection();
            }
        }

        if (spawnEnemyRight != null)
        {
            spawnEnemyRight.transform.parent = transform;
            spawnEnemyRight.transform.localPosition = RightSpawn + MonsterStartingPosition(monster);
            spawnEnemyRight.transform.localScale = GetMonsterType(monster.monster).transform.localScale;
            if (spawnEnemyRight.GetComponent<EnemyControl>() != null)
            {
                spawnEnemyRight.GetComponent<EnemyControl>().playerStartingVector = 1;
                spawnEnemyRight.GetComponent<EnemyControl>().SetDirection();
            }
            else if (spawnEnemyRight.GetComponent<FlyEnemyControl>() != null)
            {
                spawnEnemyRight.GetComponent<FlyEnemyControl>().playerStartingVector = 1;
                spawnEnemyRight.GetComponent<FlyEnemyControl>().SetDirection();
            }
        }
    }

    // Find your monster script with enum
    public GameObject GetMonsterType(MonsterTypes enemyType)
    {
        foreach (var enemyIndex in enemyTypeList.monsterList)
        {
            if (enemyIndex.monsterEnum == enemyType) return enemyIndex.monster;
        }

        return null;
    }

    public void OnClickSpawnWave()
    {
        if (levelWavesSO.wavesMonsters[index].isSpawningNow == false)
        {
            GetSpawnTime();
            levelWavesSO.wavesMonsters[index].isSpawningNow = true;
            StartCoroutine(WaveLogic(index));
        }
    }
    // Monster starting y position spawn
    private Vector3 MonsterStartingPosition(WaveMonster monster) 
    {
        if (monster.monster == MonsterTypes.Orc) return new Vector3(0, -2.5f, 0);
        if (monster.monster == MonsterTypes.Cyclop) return new Vector3(0, 3f, 0);
        if (monster.monster == MonsterTypes.Garg) return new Vector3(0, 0, 0);
        return new Vector3(0, 0, 0);
    }
}