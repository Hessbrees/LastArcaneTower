using Assets.Scripts.SpellS;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffect_AOE : BaseSpellEffect
{
    private readonly float radius;

    private List<ISpellEffect> spellEffects;

    public SpellEffect_AOE(float radius, params ISpellEffect[] spellEffects)
    {
        this.radius = radius;
        this.spellEffects = new List<ISpellEffect>();
        this.spellEffect = SpellEffects.AOE;
        SetSpellEffects(spellEffects);
    }

    public void SetSpellEffects(params ISpellEffect[] spellEffects)
    {
        foreach (var item in spellEffects)
        {
            if (item is SpellEffect_AOE)
                continue;

            this.spellEffects.Add(item.MapToAOE_Child());
        }
    }

    public override void TriggerSpellEffect(Collision2D collision, Vector2 direction, SpellTypes spellType)
    {
        ApplyEffects(collision, direction, spellType);
    }

    public override void TriggerSpellEffect(Collider2D collision, Vector2 direction, SpellTypes spellType)
    {
        ApplyEffects(collision, direction, spellType);
    }

    private void ApplyEffects(Collision2D collision, Vector2 direction, SpellTypes spellType)
    {
        var enemies = Physics2D.OverlapCircleAll(collision.transform.position, this.radius, LayerMask.GetMask(Settings.EnemyLayerName));
        foreach (Collider2D enemy in enemies)
        {
            foreach (var spellEffect in this.spellEffects)
            {
                spellEffect.TriggerSpellEffect(enemy, direction, spellType);
            }
        }
    }

    private void ApplyEffects(Collider2D collision, Vector2 direction, SpellTypes spellType)
    {
        var enemies = Physics2D.OverlapCircleAll(collision.transform.position, this.radius, LayerMask.GetMask(Settings.EnemyLayerName));
        foreach (Collider2D enemy in enemies)
        {
            foreach (var spellEffect in this.spellEffects)
            {
                spellEffect.TriggerSpellEffect(enemy, direction, spellType);
            }
        }
    }

    public override ISpellEffect MapToAOE_Child()
    {
        return null;
    } 
}



