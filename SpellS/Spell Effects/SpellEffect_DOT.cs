using Assets.Scripts.SpellS;
using UnityEngine;

public class SpellEffect_DOT : BaseSpellEffect
{
    private readonly int damagePerInterval;
    private readonly float interval;
    private readonly int duration;

    public SpellEffect_DOT(int damagePerInterval, float interval, int duration)
    {
        this.damagePerInterval = damagePerInterval;
        this.interval = interval;
        this.duration = duration;
        this.spellEffect = SpellEffects.DOT;
    }

    public override ISpellEffect MapToAOE_Child()
    {
        var aOE_Child = new SpellEffect_DOT((int)(damagePerInterval * BaseSpellEffect.damageModifier), interval, duration);
        aOE_Child.SetAsAOE_Child();
        return aOE_Child;
    }

    public override void TriggerSpellEffect(Collision2D collision, Vector2 direction, SpellTypes spellType)
    {
        var hpMpControl = collision.gameObject.GetComponent<EnemyHPMPControl>();
        Damage(hpMpControl, collision, spellType);
    }

    public override void TriggerSpellEffect(Collider2D collision, Vector2 direction, SpellTypes spellType)
    {
        var hpMpControl = collision.gameObject.GetComponent<EnemyHPMPControl>();
        Damage(hpMpControl, collision, spellType);
    }

    private void Damage(EnemyHPMPControl enemy, Collider2D collision, SpellTypes spellType)
    {
        var rb = collision.GetComponent<Rigidbody2D>();
        enemy.TakeDOT_Damage(this.duration, this.interval, this.damagePerInterval, spellType, this.SpellEffect);

    }

    private void Damage(EnemyHPMPControl enemy, Collision2D collision, SpellTypes spellType)
    {
        var rb = collision.transform.GetComponent<Rigidbody2D>();
        enemy.TakeDOT_Damage(this.duration, this.interval, this.damagePerInterval, spellType, this.SpellEffect);
    }
}



