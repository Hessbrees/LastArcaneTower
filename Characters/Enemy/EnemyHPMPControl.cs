using Assets.Scripts.SpellS;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyHPMPControl : MonoBehaviour
{
    public delegate void DOT_DamageEvent(int damage, Rigidbody2D rb, SpellTypes spellType, SpellEffects spellEffect);
    public static DOT_DamageEvent OnDOT_Damage;
    public delegate void ProjectileDamageEvent(int damage, Rigidbody2D rb, SpellTypes spellType, SpellEffects spellEffect);
    public static ProjectileDamageEvent OnProjectileDamage;
    public delegate void KnockbackEvent(Vector2 force, Rigidbody2D rb, SpellTypes spellType, SpellEffects spellEffect);
    public static KnockbackEvent OnKnockback;
    public delegate void AOE_DamageEvent(int damage, Rigidbody2D rb, SpellTypes spellType, SpellEffects spellEffect);
    public static AOE_DamageEvent OnAOE_Damage;

    [SerializeField] public EnemyBehaviour_SO enemyBehaviour;
    private Animator enemyAnimator;
    private GoldAwardSystem goldSystem;
    private EnemyControl enemyControl;
    private FlyEnemyControl flyEnemyControl;

    private EnemySFXManager enemySFX;
    private Slider HPSlider;
    //Animation States
    private string hurt = "Hurt";
    public bool IsDOT_Active { get; private set; } = false;

    private Coroutine DOT_Effect;
    private Rigidbody2D enemyRB;
    [NonSerialized] public float currentHP;
    private bool isDying = false;

    DifficultyScaling enemy;
    private void Awake()
    {
        enemySFX = GetComponentInChildren(typeof(EnemySFXManager)) as EnemySFXManager;
        enemy = new DifficultyScaling(GameManager.Instance.stageDifficulty, enemyBehaviour);
        currentHP = enemy.maxHP;
    }
    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyRB = GetComponent<Rigidbody2D>();
        goldSystem = GetComponent<GoldAwardSystem>();
        enemyControl = GetComponent<EnemyControl>();
        flyEnemyControl = GetComponent<FlyEnemyControl>();

        HPSlider = GetComponentInChildren(typeof(Slider)) as Slider;
        HPSliderValueChange();
    }
    private void HPSliderValueChange()
    {
        if (currentHP > 0)
            HPSlider.value = currentHP / enemy.maxHP;
        else HPSlider.value = 0;
    }
    public float GetCurrentHP() => currentHP;

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        HPSliderValueChange();

        // Get damage animation effects
        if (currentHP <= 0)
        {
            if (!isDying)
            {
                Dying();
                isDying = true;
            }
        }
        else if (damage > 0.3 * enemy.maxHP)
        {
            ChangeToHurtAnimation();
        }
    }

    public void TakeAOE_Damage(int damage, SpellEffects spellEffect, SpellTypes spellType)
    {
        TakeDamage(damage);
        if (OnAOE_Damage != null) OnAOE_Damage(damage, this.enemyRB, spellType, spellEffect);
    }

    public void TakeKnockback(Vector2 force, SpellTypes spellType, SpellEffects spellEffect)
    {
        enemyRB.AddForce(force);
        if (OnKnockback != null) OnKnockback(force, this.enemyRB, spellType, spellEffect);
    }

    public void TakeProjectileDamage(int damage, SpellTypes spellType, SpellEffects spellEffect)
    {
        TakeDamage(damage);
        if (OnProjectileDamage != null) OnProjectileDamage(damage, this.enemyRB, spellType, spellEffect);
    }

    public void TakeDOT_Damage(float duration, float interval, int damagePerInterval, SpellTypes spellType, SpellEffects spellEffect)
    {
        if (IsDOT_Active) StopCoroutine(DOT_Effect);
        IsDOT_Active = true;
        DOT_Effect = StartCoroutine(DOT_Damage(duration, interval, damagePerInterval, spellType, spellEffect));
    }

    private IEnumerator DOT_Damage(float duration, float interval, int damagePerInterval, SpellTypes spellType, SpellEffects spellEffect)
    {
        int counter = 0;
        while (counter < duration)
        {
            yield return new WaitForSecondsRealtime(interval);
            TakeDamage(damagePerInterval);
            if (OnDOT_Damage != null) OnDOT_Damage(damagePerInterval, this.enemyRB, spellType, spellEffect);
            counter++;
        }
        IsDOT_Active = false;
    }

    private void Dying()
    {
        if (enemyControl != null)
            enemyControl.StopAllCoroutinesAfterDeath();
        else if (flyEnemyControl != null)
            flyEnemyControl.StopAllCoroutinesAfterDeath();
    }
    public void ChangeToHurtAnimation()
    {
        enemyAnimator.Play(hurt, -1, 0);
        enemySFX.hitAudioPlay();
    }
    void DestroyGameObject()
    {
        goldSystem.GetGold();

        StartCoroutine(DestroyGameObjectAfterEndSFX());
    }

    IEnumerator DestroyGameObjectAfterEndSFX()
    {

        while (enemySFX.IsDeathAudioPlaying())
        {
            yield return null;
        }

        GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>().OnMuteSFXChange -= enemySFX.MuteAllAudio;

        Destroy(gameObject);
    }

}

