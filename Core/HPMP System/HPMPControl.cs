
using UnityEngine;

public class HPMPControl : MonoBehaviour, IDamagable
{
    [SerializeField] private HPMP_SO hpMP_SO;

    private HPMPControlLogic hpMpControlLogic;
    private float currentHP;
    private bool isPlayer;
    private bool playerDead;

    public HPMP_SO HPMP_SO => hpMP_SO;
    public bool PlayerDead => playerDead;

    private void Start()
    {
        hpMP_SO.CurrentHP = hpMP_SO.MaxHP;
        hpMpControlLogic = new HPMPControlLogic(hpMP_SO.MaxHP);
        currentHP = hpMP_SO.MaxHP;

        CheckIfPlayer();
    }

    private void CheckIfPlayer()
    {
        var playerScript = GetComponent<PlayerManager>();
        if (playerScript == null)
            isPlayer = false;
        else
            isPlayer = true;
    }

    public void TakeDamage(float damage)
    {
        currentHP = hpMpControlLogic.currentHP;
        hpMpControlLogic.TakeDamage(damage);

        if (currentHP > 0) hpMP_SO.CurrentHP = currentHP;
        else hpMP_SO.CurrentHP = 0;

        if (hpMpControlLogic.dead)
        {
            Die();
        }
    }

    public float GetCurrentHP()
    {
        return currentHP;
    }

    public void GodMode()
    {
        hpMpControlLogic.currentHP = int.MaxValue;
    }

    [ContextMenu("Trigger Death")]
    void Die()
    {
        if (isPlayer)
        {
            if (playerDead)
                return;

            playerDead = true;
            PlayerManager.Instance.Death();
            return;
        }

        Destroy(gameObject);
    }
}

