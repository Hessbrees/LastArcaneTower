using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(2)]
public class SpecialSpell_Vaporize : SpellLogic
{
    [SerializeField] private float projectileSpeed;

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

    public override void ResetSpell(float delay) => StartCoroutine(DelayReset(5f));

    public override void ThrowSpell(Vector2 position)
    {
        spellRB.position = new Vector2(0, -5);
        spellRB.isKinematic = false;
        spellRB.AddForce(Vector2.up * projectileSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision) => parentSpell.OnSpellHit(collision, collision.transform.position);

    private IEnumerator DelayReset(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        spellRB.velocity = Vector2.zero;
        spellRB.position = resetPosition;
        spellRB.isKinematic = true;
    }
}
