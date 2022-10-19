using UnityEngine;

[RequireComponent(typeof(HPMPControl))]
[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class PlayerManager : Singleton<PlayerManager>
{
    UI_Manager uI_Manager;

    public delegate void PlayerDeathEvent();
    public event PlayerDeathEvent PlayerDeath;

    [SerializeField] private Player_SO player_SO;
    [SerializeField] private PlayerSprites_SO playerSprites_SO;

    private SpellController spellController;
    private SpriteRenderer towerSprite;
    private Transform spellSource;
    private Transform wallSpellSpawnPoint;
    private Transform relativeInputPoint;

    HPMPControl hPMPControl;
    private float currentHP;
    private float maxHP;
    private bool playerDead;

    public Player_SO Player_SO => player_SO;
    public bool IsPlayerDead => playerDead;
    public bool SpecialSpellModeActive => InputManager.Instance.SpecialSpellModeActive;
    public Transform SpellSource => spellSource;
    public Transform WallSpellSpawnPoint  => wallSpellSpawnPoint;
    public Transform RelativeInputPoint => relativeInputPoint; 

    protected override void Awake()
    {
        base.Awake();
        player_SO = ResourceLoader.LoadPlayer_SO();
        playerSprites_SO = ResourceLoader.LoadPlayerSprites_SO();
        spellSource = transform.Find("SpellSource");
        wallSpellSpawnPoint = transform.Find("WallSpellFloorPoint");
        relativeInputPoint = transform.Find("RelativeInputPoint");

        hPMPControl = GetComponent<HPMPControl>();
        towerSprite = transform.Find("TowerSprite").GetComponent<SpriteRenderer>();
        spellController = transform.Find("SpellController").GetComponent<SpellController>();

        towerSprite.sprite = playerSprites_SO.sprites[0];
        maxHP = hPMPControl.HPMP_SO.MaxHP;

        PlayerDeath += OnPlayerDeath;
    }

    private void Start() => OnPlayerLoaded();

    private void OnEnable()
    {
        GameManager.Instance.GestureRecognizedWithSpellName += spellController.OnGestureRecognized;
        PlayerDeath += UI_Manager.Instance.ToggleGameLoseMenu;
        InputManager.Instance.SwipeUp += spellController.OnSwipeUp;
        InputManager.Instance.SwipeDown += spellController.OnSwipeDown;
        InputManager.Instance.ProjectileSwipe += spellController.OnProjectileSwipe;
        InputManager.Instance.SwipeUp += spellController.OnSwipeUp;
    }

    private void OnDisable()
    {
        GameManager.Instance.GestureRecognizedWithSpellName -= spellController.OnGestureRecognized;
        PlayerDeath -= UI_Manager.Instance.ToggleGameLoseMenu;
        InputManager.Instance.SwipeUp -= spellController.OnSwipeUp;
        InputManager.Instance.SwipeDown -= spellController.OnSwipeDown;
        InputManager.Instance.ProjectileSwipe -= spellController.OnProjectileSwipe;
        InputManager.Instance.SwipeUp -= spellController.OnSwipeUp;
    }

    private void Update()
    {
        SpriteChange();
        GodMode();
        //Debug.Log(SpecialSpellModeActive);
    }

    public void Death()
    {
        playerDead = true;
        if (PlayerDeath != null) PlayerDeath();
    }

    private void SpriteChange()
    {
        currentHP = hPMPControl.GetCurrentHP();

        if (currentHP >= 0.7 * maxHP) towerSprite.sprite = playerSprites_SO.sprites[0];
        else if (currentHP >= 0.4 * maxHP) towerSprite.sprite = playerSprites_SO.sprites[1];
        else towerSprite.sprite = playerSprites_SO.sprites[2];
    }

    private void GodMode()
    {
        if (GameManager.Instance.GodMode)
            hPMPControl.GodMode();
    }

    private void OnPlayerLoaded()
    {
        playerDead = false;
        spellController.OnPlayerLoaded();
    }

    private void OnPlayerRespawn() => playerDead = false;

    private void OnPlayerDeath() => spellController.OnPlayerDeath();
}

