using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(2)]
public class Spell_FireWall : SpellLogic, IDamagable
{
    private Rigidbody2D spellRB;
    private Vector2 resetPosition;
    private float maxHP;
    private float currentHP;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer(Settings.BaseLayerName);

        spellRB = GetComponent<Rigidbody2D>();
        maxHP = GetComponent<SpellMonoBehaviour>().SpellStats_SO.hp;
        currentHP = maxHP;
        resetPosition = GameManager.Instance.SpellSpawn.position;
    }

    public override void ResetSpell(float delay)
    {
        if (currentHP <= 0)
        {
            spellRB.position = resetPosition;
            currentHP = maxHP;
            InvokeOnSpellReset();
        }
    }

    public override void ThrowSpell(Vector2 position)
        => spellRB.position = new Vector2(position.x, PlayerManager.Instance.WallSpellSpawnPoint.position.y);

    public void TakeDamage(float damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
            ResetSpell(0);
    }
}
