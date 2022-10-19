using Assets.Scripts.SpellS;
using UnityEngine;

public class SpellEffect_Damage : BaseSpellEffect
{
    private readonly int damage;

    public int Damage => damage;

    public SpellEffect_Damage(int damage)
    {
        this.damage = damage;
        this.spellEffect = SpellEffects.Damage;
    }

    public override void TriggerSpellEffect(Collision2D collision, Vector2 direction, SpellTypes spellType)
    {
        var enemyHPMPControl = collision.gameObject.GetComponent<EnemyHPMPControl>();
        if (IsAOE_Child)
            enemyHPMPControl.TakeAOE_Damage(damage, this.SpellEffect, spellType);
        else
            enemyHPMPControl.TakeProjectileDamage(damage, spellType, this.SpellEffect);
    }

    public override void TriggerSpellEffect(Collider2D collision, Vector2 direction, SpellTypes spellType)
    {
        var enemyHPMPControl = collision.gameObject.GetComponent<EnemyHPMPControl>();
        if (IsAOE_Child)
            enemyHPMPControl.TakeAOE_Damage(damage, this.SpellEffect, spellType);
        else
            enemyHPMPControl.TakeProjectileDamage(damage, spellType, this.SpellEffect);
    }

    public override ISpellEffect MapToAOE_Child()
    {
        var aOE_Child = new SpellEffect_Damage((int)(damage * BaseSpellEffect.damageModifier));
        aOE_Child.SetAsAOE_Child();
        return aOE_Child;
    }
}



