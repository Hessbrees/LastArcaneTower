using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : Singleton<UI_Manager>, IPlayerLoader
{
    public delegate void UI_ActivatedEvent();
    public event UI_ActivatedEvent OnUI_Activated;

    public delegate void UI_DeactivatedEvent();
    public event UI_DeactivatedEvent OnUI_Deactivated;

    private UI_GameController uI_GameController;
    private UI_MenuController uI_MenuController;

    private GameObject menuParticleEffect;

    private PlayerManager player;

    private Button musicButton;
    private Transform menuMusicButtonLocation;
    private Transform gameSceneUIMusicButtonLocation;

    private bool is_UI_Active;

    [Header("Text pop-up when enemy gets hit")]
    [SerializeField] private bool showDamageAndEffectsOnHit;

    public PlayerManager Player => player;
    public bool Is_UI_Active => is_UI_Active;
    public bool ShowDamageText => showDamageAndEffectsOnHit;
    public GraphicRaycaster GameSceneUIGraphicRaycaster => uI_MenuController.GameSceneUIGraphicRaycaster;

    protected override void Awake()
    {
        base.Awake();

        is_UI_Active = true;
        uI_GameController = transform.Find("UI_GameController").GetComponent<UI_GameController>();
        uI_MenuController = transform.Find("UI_MenuController").GetComponent<UI_MenuController>();
        menuParticleEffect = transform.Find("MainMenu PE").gameObject;

        GameManager.Instance.GameSceneLoaded += OnGameSceneLoaded;
        GameManager.Instance.GameSceneUnloaded += OnGameSceneUnloaded;
    }

    public void Set_UI_Active(bool isActive)
    {
        is_UI_Active = isActive;
        if (!isActive)
        {
            if (OnUI_Deactivated != null) OnUI_Deactivated();
        }
        else
        {
            if (OnUI_Activated != null) OnUI_Activated();
        }
    }

    public void OnGameSceneUnloaded()
    {
        Set_UI_Active(true);
        uI_MenuController.OnGameSceneUnloaded();
    }

    public void OnGameSceneLoaded()
    {
        Set_UI_Active(false);
        uI_MenuController.OnGameSceneLoaded();
    }

    public void LoadLevel(LevelData_SO levelData) => GameManager.Instance.LoadGameScene(levelData);

    public void OnGestureRecognizedWithSpellName(string specialSpellName)
    {
        uI_MenuController.OnGestureRecognized();
        //uI_GameController.OnGestureRecognizedWithSpellName(specialSpellName);
    }

    // Game controller should be deleted or something
    public void OnGestureNotRecognized()
    {
        //uI_GameController.OnGestureNotRecognized();
    }

    public void LoadPlayer() => player = PlayerManager.Instance;

    public void ToggleSpecialSpellMode() => GameManager.Instance.ToggleSpecialSpellMode();

    public void ToggleUpgradeMenu() => uI_MenuController.ToggleUpgradeMenu();

    public void ToggleMapMenu() => uI_MenuController.ToggleMapMenu();

    public void ToggleMainMenu() => uI_MenuController.ToggleMainMenu();

    public void ToggleGameSceneMenu() => uI_MenuController.ToggleGameSceneMenu();

    public void ToggleGameSceneUI() => uI_MenuController.ToggleGameSceneUI();

    public void ToggleGameWinMenu() => uI_MenuController.ToggleGameWinMenu();

    public void ToggleGameLoseMenu() => uI_MenuController.ToggleGameLoseMenu();

    public void ToggleMenuParticleEffect() => menuParticleEffect.SetActive(!menuParticleEffect.activeInHierarchy);

    public void UpdateProjectileSpellCooldownUI(float spellCD, float maxSpellCD) => uI_MenuController.UpdateProjectileSpellCooldownUI(spellCD, maxSpellCD);

    public void UpdateWallSpellCooldownUI(float spellCD, float maxSpellCD) => uI_MenuController.UpdateWallSpellCooldownUI(spellCD, maxSpellCD);

    public void UpdateSkyDropSpellCooldownUI(float spellCD, float maxSpellCD) => uI_MenuController.UpdateSkyDropSpellCooldownUI(spellCD, maxSpellCD);

    public void UpdateSpecialSpellCooldownUI(float spellCD, float maxSpellCD) => uI_MenuController.UpdateSpecialSpellCooldownUI(spellCD, maxSpellCD);
    public void LoadLevel(int index) => uI_MenuController.LoadLevel(index);

    public List<Button> GetMusicButtons() => uI_MenuController.GetMusicButtons();
}
