using System.Collections;
using UnityEditor;
using UnityEngine;

[DefaultExecutionOrder(2)]
public class Spell_Meteor : SpellLogic
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float explosionDamageRadius;

    private SpellMonoBehaviour parentSpell;
    private Rigidbody2D spellRB;
    private Vector2 resetPosition;

    private void Awake()
    {
        parentSpell = GetComponent<SpellMonoBehaviour>();
        spellRB = GetComponent<Rigidbody2D>();
        spellRB.isKinematic = true;

        resetPosition = GameManager.Instance.SpellSpawn.position;
    }

    public override void ResetSpell(float delay)
    {
        StartCoroutine(DelayReset(delay));
    }

    public override void ThrowSpell(Vector2 position)
    {
        spellRB.isKinematic = false;
        spellRB.position = new Vector2(position.x, 10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (parentSpell != null)
            parentSpell.TriggerSpellEffects(collision, transform.position);

        if (collision.gameObject.layer == LayerMask.NameToLayer(Settings.GroundLayerName) && parentSpell != null)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionDamageRadius);

            foreach (var enemy in hitEnemies)
            {
                parentSpell.TriggerSpellEffects(enemy, Vector2.zero);
            }

            parentSpell.OnSpellHit(collision, transform.position);
        }
    }

    private IEnumerator DelayReset(float delay)
    {
        spellRB.velocity = Vector2.zero;
        spellRB.isKinematic = true;
        yield return new WaitForSecondsRealtime(delay);
        spellRB.position = resetPosition;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.forward, explosionDamageRadius);
    }
#endif
}
