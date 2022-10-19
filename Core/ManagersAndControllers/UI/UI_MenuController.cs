using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MenuController : MonoBehaviour
{
    #region Fields and props
    private Levels_SO levels_SO;
    private Player_SO player_SO;

    private GameObject mainMenu;
    private Button mainMenuPlayButton;
    private Button mainMenuMusicButton;

    private GameObject mapMenu;
    private Button mapMenuBackButton;
    private Button mapMenuMusicButton;
    private Button mapMenu_ToUpgradeMenu_Button;
    private Button mapMenu_ToStatsMenu_Button;
    private Button mapMenu_ToSettingMenu_Button;
    private Button mapMenu_levelButton_1;

    private DifficultyLevelManager difficultyLevelManager;

    private GameObject diffMenu;
    private Button diffMenuStartButton;
    private Button diffMenuBackButton;

    private GameObject upgradeMenu;
    private Button upgradeMenuMusicButton;
    private Button upgradeMenuBackButton;
    private TMP_Text upgradePointCounterText;

    private GameObject statsMenu;
    private Button statsMenuBackButton;

    private GameObject gameSceneMenu;
    private Button gameSceneMenuMusicButton;
    private Button gameSceneMenuBackToMenuButton;
    private Button gameSceneMenuSettingButton;

    private GameObject gameSceneUI;
    private Button gameSceneUIPauseButton;
    private GraphicRaycaster gameSceneUIGraphicRaycaster;

    private GameObject settingMenu;
    private Button settingMenuButton;

    private GameObject gameWinMenu;
    private Button gameWinMenuButton;

    private GameObject gameLoseMenu;
    private Button gameLoseBackToMenuButton;
    private Button gameLoseRestartButton;

    private GameObject cooldownsBar;
    private SpellCooldownsUI spellCooldownsUI;

    private GameObject specialSpellModeScreen;
    private Button specialSpellModeButton;

    public GraphicRaycaster GameSceneUIGraphicRaycaster => gameSceneUIGraphicRaycaster;
    #endregion


    private void Awake()
    {
        player_SO = ResourceLoader.LoadPlayer_SO();
        levels_SO = ResourceLoader.LoadLevels_SO();
        SpellUpgrade.OnUpgrade += UpdateUpgradePointsCounter;

        GetObjectsAndButtons();
        AddListenersToButtons();

        mainMenu.SetActive(true);

    }

    private void GetObjectsAndButtons()
    {
        mainMenu = transform.Find("MainMenu").gameObject;
        mainMenuPlayButton = mainMenu.transform.Find("PlayButton").GetComponent<Button>();

        mapMenu = transform.Find("MapMenu").gameObject;
        mapMenuBackButton = mapMenu.transform.Find("Table").Find("BackButton").GetComponent<Button>();
        mapMenu_ToUpgradeMenu_Button = mapMenu.transform.Find("UpgradeNavButton").GetComponent<Button>();
        mapMenu_ToStatsMenu_Button = mapMenu.transform.Find("StatsNavButton").GetComponent<Button>();
        mapMenu_ToSettingMenu_Button = mapMenu.transform.Find("SettingNavButton").GetComponent<Button>();
        mapMenu_levelButton_1 = mapMenu.transform.Find("Table").Find("Level (1)").GetComponent<Button>();

        diffMenu = transform.Find("DifficultyMenu").gameObject;
        diffMenuStartButton = diffMenu.transform.Find("Start").GetComponent<Button>();
        diffMenuBackButton = diffMenu.transform.Find("Table").Find("BackButton").GetComponent<Button>();
        difficultyLevelManager = diffMenu.GetComponent<DifficultyLevelManager>();

        upgradeMenu = transform.Find("UpgradeMenu").gameObject;
        upgradeMenuBackButton = upgradeMenu.transform.Find("UpgradeTree").Find("BackButton").GetComponent<Button>();
        upgradePointCounterText = upgradeMenu.transform.Find("UpgradeTree")
            .Find("UpgradePointsCounterText").GetComponent<TMP_Text>();

        statsMenu = transform.Find("StatsMenu").gameObject;
        statsMenuBackButton = statsMenu.transform.Find("Table").Find("BackButton").GetComponent<Button>();

        gameSceneMenu = transform.Find("GameSceneMenu").gameObject;
        gameSceneMenuBackToMenuButton = gameSceneMenu.transform.Find("Table").Find("BackToMenuButton").GetComponent<Button>();
        gameSceneMenuSettingButton = gameSceneMenu.transform.Find("Table").Find("SettingNavButton").GetComponent<Button>();

        gameSceneUI = transform.Find("GameSceneUI").gameObject;
        gameSceneUIPauseButton = gameSceneUI.transform.Find("PauseButton").GetComponent<Button>();
        gameSceneUIGraphicRaycaster = gameSceneUI.GetComponent<GraphicRaycaster>();

        settingMenu = transform.Find("SettingsMenu").gameObject;
        settingMenuButton = settingMenu.transform.Find("Table").Find("BackButton").GetComponent<Button>();

        gameWinMenu = transform.Find("GameWinUI").gameObject;
        gameWinMenuButton = gameWinMenu.transform.Find("Table").Find("BackToMenu").GetComponent<Button>();

        gameLoseMenu = transform.Find("GameLoseUI").gameObject;
        gameLoseBackToMenuButton = gameLoseMenu.transform.Find("Table").Find("BackToMenu").GetComponent<Button>();
        gameLoseRestartButton = gameLoseMenu.transform.Find("Table").Find("Restart").GetComponent<Button>();

        cooldownsBar = gameSceneUI.transform.Find("CooldownsBar").gameObject;
        spellCooldownsUI = cooldownsBar.GetComponent<SpellCooldownsUI>();

        specialSpellModeScreen = gameSceneUI.transform.Find("SpecialSpellModeScreen").gameObject;
        specialSpellModeButton = gameSceneUI.transform.Find("SpecialSpellModeButton").GetComponent<Button>();
    }

    private void AddListenersToButtons()
    {
        mainMenuPlayButton.onClick.AddListener(ToggleMainMenu);
        mainMenuPlayButton.onClick.AddListener(ToggleMapMenu);

        mapMenuBackButton.onClick.AddListener(ToggleMainMenu);
        mapMenuBackButton.onClick.AddListener(ToggleMapMenu);

        mapMenu_ToUpgradeMenu_Button.onClick.AddListener(ToggleUpgradeMenu);
        mapMenu_ToUpgradeMenu_Button.onClick.AddListener(ToggleMapMenu);

        mapMenu_ToStatsMenu_Button.onClick.AddListener(ToggleStatsMenu);
        mapMenu_ToStatsMenu_Button.onClick.AddListener(ToggleMapMenu);

        mapMenu_ToSettingMenu_Button.onClick.AddListener(ToggleSettingMenu);

        mapMenu_levelButton_1.onClick.AddListener(ToggleDiffMenu);
        mapMenu_levelButton_1.onClick.AddListener(delegate { difficultyLevelManager.SetLevelIndex(1); });

        diffMenuBackButton.onClick.AddListener(ToggleDiffMenu);
        diffMenuStartButton.onClick.AddListener(difficultyLevelManager.OnCLickStartLevel);
        diffMenuStartButton.onClick.AddListener(ToggleDiffMenu);

        upgradeMenuBackButton.onClick.AddListener(ToggleUpgradeMenu);
        upgradeMenuBackButton.onClick.AddListener(ToggleMapMenu);

        statsMenuBackButton.onClick.AddListener(ToggleStatsMenu);
        statsMenuBackButton.onClick.AddListener(ToggleMapMenu);

        gameSceneMenuBackToMenuButton.onClick.AddListener(ToggleGameSceneMenu);
        gameSceneMenuBackToMenuButton.onClick.AddListener(UnloadLevel);
        gameSceneMenuSettingButton.onClick.AddListener(ToggleSettingMenu);

        gameSceneUIPauseButton.onClick.AddListener(ToggleGameSceneMenu);

        settingMenuButton.onClick.AddListener(ToggleSettingMenu);
        gameWinMenuButton.onClick.AddListener(ToggleGameWinMenu);

        gameLoseBackToMenuButton.onClick.AddListener(ToggleGameLoseMenu);
        gameLoseBackToMenuButton.onClick.AddListener(UnloadLevel);

        gameLoseRestartButton.onClick.AddListener(ToggleGameLoseMenu);
        gameLoseRestartButton.onClick.AddListener(RestartLevel);

        specialSpellModeButton.onClick.AddListener(ToggleSpecialSpellMode);
    }

    public void OnGameSceneUnloaded()
    {
        mainMenu.SetActive(false);
        mapMenu.SetActive(true);
        upgradeMenu.SetActive(false);

        gameSceneMenu.SetActive(false);
        gameSceneUI.SetActive(false);
    }

    public void OnGameSceneLoaded()
    {
        mainMenu.SetActive(false);
        mapMenu.SetActive(false);
        upgradeMenu.SetActive(false);

        gameSceneMenu.SetActive(false);
        gameSceneUI.SetActive(true);
    }

    public List<Button> GetMusicButtons()
    {
        return new List<Button>() {
            mainMenuMusicButton,
            upgradeMenuMusicButton,
            mapMenuMusicButton,
            gameSceneMenuMusicButton };
    }

    public void ToggleSpecialSpellMode()
    {
        UI_Manager.Instance.ToggleSpecialSpellMode();
        specialSpellModeScreen.SetActive(!specialSpellModeScreen.activeInHierarchy);
    }

    public void OnGestureRecognized() => specialSpellModeScreen.SetActive(!specialSpellModeScreen.activeInHierarchy);

    public void ToggleUpgradeMenu() => upgradeMenu.SetActive(!upgradeMenu.activeInHierarchy);

    public void ToggleStatsMenu() => statsMenu.SetActive(!statsMenu.activeInHierarchy);

    public void ToggleMapMenu() => mapMenu.SetActive(!mapMenu.activeInHierarchy);
    public void ToggleDiffMenu() => diffMenu.SetActive(!diffMenu.activeInHierarchy);

    public void ToggleMainMenu() => mainMenu.SetActive(!mainMenu.activeInHierarchy);

    public void ToggleGameSceneMenu() => gameSceneMenu.SetActive(!gameSceneMenu.activeInHierarchy);

    public void ToggleGameSceneUI() => gameSceneUI.SetActive(!gameSceneUI.activeInHierarchy);

    public void ToggleSettingMenu() => settingMenu.SetActive(!settingMenu.activeInHierarchy);

    public void ToggleGameWinMenu() => gameWinMenu.SetActive(!gameWinMenu.activeInHierarchy);

    public void ToggleGameLoseMenu() => gameLoseMenu.SetActive(!gameLoseMenu.activeInHierarchy);

    public void UpdateUpgradePointsCounter() => upgradePointCounterText.text = $"Upgrade Points Available: {player_SO.upgradePointsAvailable}";

    public void UpdateProjectileSpellCooldownUI(float spellCD, float maxSpellCD) => spellCooldownsUI.GetProjectileSpellsCD(spellCD, maxSpellCD);

    public void UpdateWallSpellCooldownUI(float spellCD, float maxSpellCD) => spellCooldownsUI.GetpWallSpellsCD(spellCD, maxSpellCD);

    public void UpdateSkyDropSpellCooldownUI(float spellCD, float maxSpellCD) => spellCooldownsUI.GetSkyDropSpellsCD(spellCD, maxSpellCD);

    public void UpdateSpecialSpellCooldownUI(float spellCD, float maxSpellCD) => spellCooldownsUI.GetSpecialSpellsCD(spellCD, maxSpellCD);
    public void LoadLevel(int levelIndex) => UI_Manager.Instance.LoadLevel(levels_SO.levels[levelIndex]);

    private void UnloadLevel() => GameManager.Instance.UnloadGameScene();

    private void RestartLevel() => GameManager.Instance.RestartGameScene();
}
