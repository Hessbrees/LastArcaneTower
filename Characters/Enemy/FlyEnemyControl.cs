using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FlyEnemyControl : MonoBehaviour
{
    [NonSerialized] public int playerStartingVector = 1;
    private Rigidbody2D rigidbody2D;
    private EnemySFXManager enemySFX;
    [SerializeField] private EnemyBehaviour_SO enemyBehaviour;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 0.5f;
    private Animator enemyAnimator;
    [Header("Default base")]
    [SerializeField] private LayerMask baseLayer;
    private EnemyHPMPControl enemyHPMPControl;
    private Slider HPSlider;

    private float nextRegTime = 1f;
    private string attackAnimation = "Attack";
    private string walkAnimation = "Walk";
    private string flyAnimation = "Fly";
    private string death = "Death";
    private bool isAtackAnimationEnd = true;
    private bool isAtackingNow = false;
    private bool isWalking = true;
    private bool isRegenerating = false;
    private int easingFallDownID;
    private bool timeForWalkAnimation = false;

    DifficultyScaling enemy;
    void Start()
    {
        enemySFX = GetComponentInChildren(typeof(EnemySFXManager)) as EnemySFXManager;
        rigidbody2D = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        enemyHPMPControl = GetComponent<EnemyHPMPControl>();
        HPSlider = GetComponentInChildren(typeof(Slider)) as Slider;
        isAtackingNow = false;
        isWalking = true;
        timeForWalkAnimation = false;
        HPSlider.transform.localScale = new Vector2(HPSlider.transform.localScale.x * playerStartingVector, HPSlider.transform.localScale.y);

        enemyAnimator.SetBool("timeForWalkAnimation", false);

        enemy = new DifficultyScaling(GameManager.Instance.stageDifficulty, enemyBehaviour);

        StartCoroutine(Fly());
        StartCoroutine(RegenerationHP());
        StartCoroutine(Atack());
    }
    private void Update()
    {
        if (enemyHPMPControl.GetCurrentHP() < 0.7 * enemy.maxHP && !timeForWalkAnimation)
        {
            rigidbody2D.gravityScale = 1;
            timeForWalkAnimation = true;
            var easingFallDownID = LeanTween.value(2f, 4f, 0.2f).setOnUpdate(FallDown).uniqueId;
        }
        else if(isAtackingNow && !timeForWalkAnimation)
        {
            rigidbody2D.gravityScale = 1;
            timeForWalkAnimation = true;
        }
    }
    IEnumerator Atack()
    {
        while (true)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, baseLayer);

            if (hitEnemies.Length > 0)
            {
                ChangeToAtackAnimation();
                isAtackingNow = true;
                isWalking = false;

                yield return new WaitUntil(() => isAtackAnimationEnd);
                yield return new WaitForSeconds(0.1f);
                isAtackAnimationEnd = false;

                foreach (var enemies in hitEnemies)
                {
                    enemies.GetComponent<IDamagable>().TakeDamage(enemy.damage);
                }

                yield return new WaitForSeconds(enemy.timeBetweenAtack);
            }
            else isAtackingNow = false;

            yield return null;
        }
    }
    void WaitForFinishAtackAnimation() => isAtackAnimationEnd = true;
    IEnumerator RegenerationHP()
    {
        if (!isRegenerating)
        {
            isRegenerating = true;
            while (true)
            {
                // To prevent reach more than max hp
                if (enemyHPMPControl.currentHP < enemy.maxHP)
                {
                    enemyHPMPControl.currentHP += enemy.regHP;
                    if (enemyHPMPControl.currentHP > enemy.maxHP) enemyHPMPControl.currentHP = enemy.maxHP;
                }
                yield return new WaitForSeconds(nextRegTime);
            }
        }
        isRegenerating = false;
    }
    IEnumerator Fly()
    {
        rigidbody2D.gravityScale = 0;
        while (true)
        {
            if (!isAtackingNow)
            {
                ChangeToFlyAnimation();

                Vector2 EnemyVelocity = new Vector2(enemy.speed * playerStartingVector, rigidbody2D.velocity.y);

                rigidbody2D.velocity = EnemyVelocity;
            }
            if (timeForWalkAnimation == true) break;
            yield return null;
        }
        enemyAnimator.SetBool("timeForWalkAnimation",true);
        
        StartCoroutine(Run());
    }
    void FallDown(float value)
    {
        Vector2 EnemyVelocity = new Vector2(-value* playerStartingVector, rigidbody2D.velocity.y);
        rigidbody2D.velocity = EnemyVelocity;
    }
    IEnumerator Run()
    {
        while (true)
        {
            if(!LeanTween.isTweening(easingFallDownID))
            if (!isAtackingNow)
            {
                ChangeToWalkAnimation();

                Vector2 EnemyVelocity = new Vector2(0.5f *enemy.speed * playerStartingVector, rigidbody2D.velocity.y);

                rigidbody2D.velocity = EnemyVelocity;

            }
            yield return null;
        }
    }
    public void SetDirection()
    {
        if (playerStartingVector < 0)
        {
            transform.localScale = new Vector2(-1 * transform.localScale.x, transform.localScale.y);
        }
    }

    void ChangeToAtackAnimation()
    {
        enemyAnimator.Play(attackAnimation, -1, 0);
        enemySFX.atackAudioPlay();
    }
    void ChangeToWalkAnimation()
    {
        if (!isWalking) enemyAnimator.Play(walkAnimation);
        isWalking = true;
    }
    void ChangeToFlyAnimation()
    {
        if (!isWalking) enemyAnimator.Play(flyAnimation);
        isWalking = true;
    }
    public void StopAllCoroutinesAfterDeath()
    {
        StopAllCoroutines();
        enemySFX.DisableHurtSFXAfterDeath();
        enemySFX.deathAudioPlay();
        enemyAnimator.Play(death);
    }

}

