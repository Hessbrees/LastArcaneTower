using UnityEngine;

[DefaultExecutionOrder(2)]
public class Spell_FireBall : SpellLogic
{
    [SerializeField] private float projectileSpeed;

    private Rigidbody2D childRB;
    private SpellMonoBehaviour parentSpell;
    private Vector2 playerSpellSource;

    private void Awake()
    {
        parentSpell = GetComponent<SpellMonoBehaviour>();

        childRB = parentSpell.Child;
        childRB.isKinematic = true;

        playerSpellSource = PlayerManager.Instance.SpellSource.position;
    }

    public override void ResetSpell(float delay) {}

    public override void ThrowSpell(Vector2 direction)
    {
        var newProjectileGO = GameObject.Instantiate(childRB.gameObject,
            playerSpellSource,
            Quaternion.identity,
            parentSpell.transform);

        var newProjectileRB = newProjectileGO.GetComponent<Rigidbody2D>();
        newProjectileRB.position = playerSpellSource;
        newProjectileRB.isKinematic = false;
        newProjectileRB.transform.rotation = SpellMonoBehaviour.GetRotationTowardsDirection(direction);
        AddForce(direction, newProjectileRB);

        GameObject.Destroy(newProjectileGO, 2f);
    }

    private void AddForce(Vector2 direction, Rigidbody2D newProjectileRB)
    {
        var force = new Vector2(direction.x * projectileSpeed, direction.y * projectileSpeed);
        newProjectileRB.AddForce(force);
    }
}
