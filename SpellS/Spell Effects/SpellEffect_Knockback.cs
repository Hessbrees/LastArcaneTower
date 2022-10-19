using Assets.Scripts.SpellS;
using UnityEngine;

public class SpellEffect_Knockback : BaseSpellEffect
{
    private readonly float knockbackStrength;

    private Vector2 force;

    public SpellEffect_Knockback(float knockbackStrength)
    {
        this.knockbackStrength = knockbackStrength;
        this.spellEffect = SpellEffects.Knockback;
    }

    public override ISpellEffect MapToAOE_Child()
    {
        var aOE_Child = new SpellEffect_Knockback((int)(knockbackStrength * BaseSpellEffect.knockbackStrengthModifier));
        aOE_Child.SetAsAOE_Child();
        return aOE_Child;
    }

    public override void TriggerSpellEffect(Collision2D collision, Vector2 direction, SpellTypes spellType)
    {
        force = new Vector2(direction.x * knockbackStrength, direction.y * knockbackStrength);
        collision.gameObject.GetComponent<EnemyHPMPControl>().TakeKnockback(force, spellType, this.SpellEffect);
    }

    public override void TriggerSpellEffect(Collider2D collision, Vector2 direction, SpellTypes spellType)
    {
        force = new Vector2(direction.x * knockbackStrength, direction.y * knockbackStrength);
        collision.gameObject.GetComponent<EnemyHPMPControl>().TakeKnockback(force, spellType, this.SpellEffect);
    }
}



