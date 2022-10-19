using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(2)]
public class ProjectilePrefabLogic : MonoBehaviour
{
    private SpellMonoBehaviour parentSpell;
    private Animator animator;
    private Rigidbody2D rigidBody;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = transform.Find("Sprite").GetComponent<Animator>();
        parentSpell = GetComponentInParent<SpellMonoBehaviour>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetTrigger("spellHit");
        rigidBody.velocity = Vector3.zero;
        rigidBody.isKinematic = true;
        parentSpell.OnSpellHit(collision, transform.position);
        Destroy(gameObject, 0.5f);
    }
}
