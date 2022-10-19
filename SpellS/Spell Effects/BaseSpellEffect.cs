using Assets.Scripts.SpellS;
using UnityEngine;

public abstract class BaseSpellEffect : ISpellEffect
{
    public const float damageModifier = 0.5f;
    public const float knockbackStrengthModifier = 0.5f;

    protected SpellEffects spellEffect;
    protected bool isAOE_Child;

    public bool IsAOE_Child { get => isAOE_Child; protected set => isAOE_Child = value; }
    public SpellEffects SpellEffect { get => spellEffect; protected set => spellEffect = value; }

    public abstract ISpellEffect MapToAOE_Child();
    public abstract void TriggerSpellEffect(Collision2D collision, Vector2 direction, SpellTypes spellType);

    public abstract void TriggerSpellEffect(Collider2D collision, Vector2 direction, SpellTypes spellType);

    protected virtual void SetAsAOE_Child()
    {
        this.isAOE_Child = true;
    }

}



