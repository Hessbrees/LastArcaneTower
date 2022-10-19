
public class HPMPControlLogic
{
    public float currentHP;
    public bool dead;

    public HPMPControlLogic(float currentHP)
    {
        this.currentHP = currentHP;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        // Get damage animation effects
        if (currentHP <= 0) Die();
    }

    private void Die()
    {
        dead = true;
    }
}

