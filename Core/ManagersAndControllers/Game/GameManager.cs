using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[DefaultExecutionOrder(-1)]
public class GameManager : Singleton<GameManager>
{
    #region Events
    public delegate void SpecialSpellModeTriggeredEvent();
    public SpecialSpellModeTriggeredEvent SpecialSpellModeTriggered;

    public delegate void SceneLoadedEvent();
    public SceneLoadedEvent GameSceneLoaded;
    public delegate void SceneUnloadedEvent();
    public SceneUnloadedEvent GameSceneUnloaded;

    public delegate void GestureRecognizedWithSpellNameEvent(string spellName);
    public event GestureRecognizedWithSpellNameEvent GestureRecognizedWithSpellName;
    public delegate void GestureRecognizedEvent();
    public event GestureRecognizedEvent GestureRecognized;

    public delegate void GestureNotRecognizedEvent();
    public event GestureNotRecognizedEvent GestureNotRecognized;
    #endregion
    [NonSerialized] public  WaveSystem waveSystem;
    [NonSerialized] public AudioManager audioManager;
    [NonSerialized] public PlayerProgressDataManager playerData; 
    [NonSerialized] public Difficulty stageDifficulty;

    private PlayerDataController playerDataController;

    [SerializeField] private bool devMode;
    [SerializeField] private bool godMode;

    private Transform spellSpawn;
    private Transform projectileSpellSource;

    private bool specialSpellModeActive;

    string waveSystemName = "WaveSystem";
    string gameSceneName = "GameScene";
    string gameSceneDevName = "GameScene_Dev";
    ScenesSettings_SO scenesSettings_SO;

    private DifficultyLevelManager difficultyLevelManager;

    public bool DevMode => devMode;
    public bool GodMode => godMode;
    public bool SpecialSpellModeActive => specialSpellModeActive; 
    public Transform SpellSpawn => spellSpawn;
    public void ChangeDifficulty(Difficulty difficulty)
    {
        stageDifficulty = difficulty;
    }
    public Transform ProjectileSpellSource => projectileSpellSource;

    protected override void Awake()
    {
        base.Awake();
        playerDataController = transform.Find("PlayerDataController").GetComponent<PlayerDataController>();
        audioManager = transform.Find("Audio").GetComponent(typeof(AudioManager)) as AudioManager;

        spellSpawn = transform.Find("Spell Spawn").transform;
        projectileSpellSource = transform.Find("Projectile Spell Source").transform;

        scenesSettings_SO = ResourceLoader.LoadScenesSettings_SO();

        SpellUpgrade.OnUpgrade += Upgrade;
        GameSceneLoaded += LoadPlayerForManagers;

        waveSystem = transform.Find(waveSystemName).GetComponent<WaveSystem>(); ;
        playerData = GetComponent<PlayerProgressDataManager>();
    }
     
    public void OnGestureNotRecognized()
    {
        if (GestureNotRecognized != null) GestureNotRecognized();
    }

    public void OnGestureRecognizedWithSpellName(string recognizedGesture)
    {
        UI_Manager.Instance.OnGestureRecognizedWithSpellName(recognizedGesture);

        if (GestureRecognized != null) GestureRecognized();
        if (GestureRecognizedWithSpellName != null) GestureRecognizedWithSpellName(recognizedGesture);
    }

    public void SetSpecialSpellMode(bool active) => specialSpellModeActive = active;

    [ContextMenu("Load Game Scene")]
    public void LoadGameScene(LevelData_SO wave)
    {
        UI_Manager.Instance.ToggleMenuParticleEffect(); // Turn off menu effects
        audioManager.TurnOffMenuMusic();
        audioManager.TurnOnBattleMusic();
        Scene scene = SceneManager.GetSceneByName(DevMode ? gameSceneDevName : gameSceneName);
        if (scene == null) Debug.Log("Scene name error SceneControl");
        SceneManager.LoadSceneAsync(DevMode ? gameSceneDevName : gameSceneName, LoadSceneMode.Additive);

        scenesSettings_SO.levelData_SO = wave;
        waveSystem.StartTheGame();
        if (GameSceneLoaded != null) GameSceneLoaded();
        
        //reset temp. gold
        playerData.ResetSOGoldAmount(scenesSettings_SO);
    }

    [ContextMenu("Unload Game Scene")]
    public void UnloadGameScene()
    {
        UI_Manager.Instance.ToggleMenuParticleEffect(); // Turn on menu effects
        audioManager.TurnOffBattleMusic();
        audioManager.TurnOnMenuMusic();
        Scene scene = SceneManager.GetSceneByName(gameSceneName);
        if (scene == null) Debug.Log("Scene name error GameManager");

        SceneManager.UnloadSceneAsync(gameSceneName);
        waveSystem.EndTheGame();
        if (GameSceneUnloaded != null) GameSceneUnloaded();

        //Save temp. gold
        playerData.ChangeGoldAmount(scenesSettings_SO);
        playerData.SaveGame();
    }
    [ContextMenu("Restart Game Scene")]
    public void RestartGameScene()
    {
        UnloadGameScene();
        LoadGameScene(scenesSettings_SO.levelData_SO);
    }

    public void ToggleSpecialSpellMode() => InputManager.Instance.ToggleSpecialSpellMode();

    public List<Button> GetMusicButtons() => UI_Manager.Instance.GetMusicButtons();

    public void GameWin()
    {
        UnloadGameScene();
        UI_Manager.Instance.ToggleGameWinMenu();
    }

    public void ChangeStarsAmount(int index, int levelStars) => playerData.ChangeStarsAmount(index, levelStars);
    private void Upgrade() => playerDataController.Upgrade();

    private IEnumerator DelayPlayerLoading(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        var playerLoaders = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerLoader>();
        foreach (var item in playerLoaders)
        {
            item.LoadPlayer();
        }
    }

    private void LoadPlayerForManagers() => StartCoroutine(DelayPlayerLoading(0.2f));
}
