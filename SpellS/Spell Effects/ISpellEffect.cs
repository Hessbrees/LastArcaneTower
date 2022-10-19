using Assets.Scripts.SpellS;
using UnityEngine;

public interface ISpellEffect
{
    public SpellEffects SpellEffect { get; }
    public bool IsAOE_Child { get;  }

    public void TriggerSpellEffect(Collision2D collision, Vector2 direction, SpellTypes spellType);
    public void TriggerSpellEffect(Collider2D collision, Vector2 direction, SpellTypes spellType);

    public ISpellEffect MapToAOE_Child();
}