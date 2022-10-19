using Assets.Scripts.SpellS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(1)]
[RequireComponent(typeof(SpellAnimationLogic))]
public class SpellMonoBehaviour : MonoBehaviour
{
    public delegate void SpellHitEvent();
    public static SpellHitEvent SpellHit;

    public delegate void SpecialSpellThrownEvent();
    public static SpecialSpellThrownEvent SpecialSpellThrown;

    [Header("Spell stuff")]
    [SerializeField] private SpellStats_SO spellStats_SO;
    private SpellUpgrades_SO spellUpgrades_SO;
    private SpellLogic spellLogic;
    private SpellEffectsLogic spellEffectsLogic;
    private SpellCooldownLogic spellCooldownLogic;

    [SerializeField] private Rigidbody2D child;

    [Header("Particle systems")]
    [SerializeField] private List<ParticleSystem> onHitParticlesList;

    private SpellAnimationLogic spellAnimationLogic;
    private Animator animator;

    private Vector2 position;

    public static SpellMonoBehaviour parentProjectileSpell;
    public SpellStats_SO SpellStats_SO => spellStats_SO;
    public bool CooldownActive => spellCooldownLogic.CooldownActive;
    public ISpell SpellLogic => spellLogic;
    public SpellEffectsLogic SpellEffectsLogic => spellEffectsLogic;
    public Rigidbody2D Child => child;
    public List<ParticleSystem> OnHitParticlesList { get => onHitParticlesList; set => onHitParticlesList = value; }
    public Animator Animator => animator;

    private void Awake() => InitializeSpellData();

    private void Start() => StartCoroutine(LoadPlayer(1f));

    private void Update() => CheckCooldownTime();

    private void OnEnable()
    {
        if (spellStats_SO.SpellType == SpellTypes.Special)
            GameManager.Instance.GestureRecognizedWithSpellName += ThrowSpecialSpell;

        SpellUpgrade.OnUpgrade += spellEffectsLogic.SetSpellEffects;
    }

    private void OnDisable()
    {
        if (spellStats_SO.SpellType == SpellTypes.Special)
            GameManager.Instance.GestureRecognizedWithSpellName -= ThrowSpecialSpell;

        SpellUpgrade.OnUpgrade -= spellEffectsLogic.SetSpellEffects;
    }

    public void OnSpellHit(Collider2D collision, Vector2 particlesTriggerPosition)
    {
        Vector3 positionFromPlayer = GetPositionRelativeToPlayer(collision);
        TriggerSpellEffects(collision, positionFromPlayer);

        if (SpellStats_SO.SpellType == SpellTypes.Projectile)
            return;

        if (!spellAnimationLogic.IsAnimationPlaying)
        {
            var animLength = spellAnimationLogic.ProcessSpellAnimation();
            spellLogic.ResetSpell(animLength);

            if (OnHitParticlesList != null)
            {
                foreach (var onHitParticles in OnHitParticlesList)
                {
                    var ps = Instantiate(onHitParticles, particlesTriggerPosition, Quaternion.identity);
                    Destroy(ps.gameObject, ps.main.duration);
                }
            }
        }
    }

    public void TriggerSpellEffects(Collider2D collision, Vector3 positionFromPlayer)
    {
        spellEffectsLogic.TriggerSpellEffects(collision, spellStats_SO.SpellType, positionFromPlayer);
    }

    public void InitializeSpellData()
    {
        gameObject.layer = LayerMask.NameToLayer(Settings.SpellLayerName);

        spellUpgrades_SO = spellStats_SO.spellUpgrades_SO;
        spellEffectsLogic = new SpellEffectsLogic(spellStats_SO, spellUpgrades_SO);
        spellCooldownLogic = new SpellCooldownLogic(spellStats_SO.cooldown);
        spellLogic = GetComponent<SpellLogic>();

        if (spellStats_SO.SpellType == SpellTypes.Projectile)
            return;

        spellAnimationLogic = GetComponent<SpellAnimationLogic>();
        animator = transform.Find("Sprite").GetComponent<Animator>();
    }

    public void ThrowSpell(Vector2 position, Vector2 playerSpellSource)
    {
        if (spellStats_SO.SpellType == SpellTypes.Special
            || spellCooldownLogic.CooldownActive)
            return;


        this.position = position;

        if (spellStats_SO.SpellType == SpellTypes.Projectile)
            spellLogic.ThrowSpell(InputManager.Instance.SwipeDirection);
        else
            spellLogic.ThrowSpell(position);

        ProcessCooldown();
    }

    public void ThrowSpecialSpell(string spellName)
    {
        if (spellCooldownLogic.CooldownActive)
            return;

        spellLogic.ThrowSpell(this.position);
        ProcessCooldown();
        if (SpecialSpellThrown != null) SpecialSpellThrown();
    }

    private void CheckCooldownTime()
    {
        if (spellStats_SO.SpellType == SpellTypes.Projectile)
            UI_Manager.Instance.UpdateProjectileSpellCooldownUI(spellCooldownLogic.CurrentCooldown, spellStats_SO.cooldown);
        if (spellStats_SO.SpellType == SpellTypes.Wall)
            UI_Manager.Instance.UpdateWallSpellCooldownUI(spellCooldownLogic.CurrentCooldown, spellStats_SO.cooldown);
        if (spellStats_SO.SpellType == SpellTypes.Skydrop)
            UI_Manager.Instance.UpdateSkyDropSpellCooldownUI(spellCooldownLogic.CurrentCooldown, spellStats_SO.cooldown);
        if (spellStats_SO.SpellType == SpellTypes.Special)
            UI_Manager.Instance.UpdateSpecialSpellCooldownUI(spellCooldownLogic.CurrentCooldown, spellStats_SO.cooldown);

        spellCooldownLogic.CheckCooldownTime();
    }

    private void ProcessCooldown() => spellCooldownLogic.ProcessCooldown();

    private IEnumerator LoadPlayer(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
    }

    public static Vector3 GetPositionRelativeToPlayer(Collider2D collision) => collision.transform.position - PlayerManager.Instance.transform.position;

    public static Vector3 GetPositionRelativeToPlayer(Collision2D collision) => collision.transform.position - PlayerManager.Instance.transform.position;

    public static Quaternion GetRotationTowardsDirection(Vector2 direction) => Quaternion.LookRotation(Vector3.forward, direction);
}
