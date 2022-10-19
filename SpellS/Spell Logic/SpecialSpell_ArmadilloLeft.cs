using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(2)]
public class SpecialSpell_ArmadilloLeft : SpellLogic
{
    protected enum Y_Axis_GrowthDirection
    {
        Up,
        Down
    }

    private float projectileSpeed = 500;
    private float duration = 10;
    private float y_Min = -1f;
    private float y_Max = 1f;

    private Rigidbody2D projectileRB;
    private Transform parentSpell;
    private Vector2 playerSpellSource;

    private bool isSpellActive;
    private Y_Axis_GrowthDirection y_Axis_GrowthDirection;
    private float y_Axis_SprayDirection;
    protected float x_Axis_SprayDirection;
    private float timeCounter;

    protected virtual void Awake()
    {
        var thisSpellMonoBehaviour = GetComponent<SpellMonoBehaviour>();

        projectileRB = thisSpellMonoBehaviour.Child;
        projectileRB.isKinematic = true;

        parentSpell = thisSpellMonoBehaviour.transform;
        playerSpellSource = PlayerManager.Instance.SpellSource.position;

        SetInitialDurationAndSprayDirection();

        x_Axis_SprayDirection = -1f;
    }

    protected void SetInitialDurationAndSprayDirection()
    {
        y_Axis_SprayDirection = y_Min;
        timeCounter = duration;
    }

    protected void Update()
    {
        if (isSpellActive)
        {
            timeCounter -= 1f * Time.deltaTime;
            SetGrowthDirection();
            Increment_Decrement_Y_Axis_SprayDirection();
        }
    }

    public override void ThrowSpell(Vector2 direction)
    {
        SetInitialDurationAndSprayDirection();
        StartCoroutine(SprayProjectiles());
    }

    public override void ResetSpell(float delay) { }

    protected IEnumerator SprayProjectiles()
    {
        isSpellActive = true;
        while (timeCounter > 0)
        {
            yield return new WaitForSecondsRealtime(0.1f);

            var newProjectileGO = GameObject.Instantiate(projectileRB.gameObject,
                  playerSpellSource,
                  projectileRB.transform.rotation,
                  parentSpell);

            var newProjectileRB = newProjectileGO.GetComponent<Rigidbody2D>();

            newProjectileRB.position = playerSpellSource;
            newProjectileRB.isKinematic = false;

           newProjectileGO.transform.rotation = SpellMonoBehaviour
                .GetRotationTowardsDirection(new Vector2(x_Axis_SprayDirection, y_Axis_SprayDirection));

            AddForce(newProjectileRB);

            GameObject.Destroy(newProjectileGO, 2f);
        }

        isSpellActive = false;
    }

    private void AddForce(Rigidbody2D newProjectileRB)
    {
        var force = new Vector2(x_Axis_SprayDirection * projectileSpeed, y_Axis_SprayDirection * projectileSpeed);
        newProjectileRB.AddForce(force);
    }

    protected void Increment_Decrement_Y_Axis_SprayDirection()
    {
        if (y_Axis_GrowthDirection == Y_Axis_GrowthDirection.Up)
            y_Axis_SprayDirection += 5 * Time.deltaTime;
        else
            y_Axis_SprayDirection -= 5 * Time.deltaTime;
    }

    protected void SetGrowthDirection()
    {
        if (y_Axis_SprayDirection >= y_Max)
            y_Axis_GrowthDirection = Y_Axis_GrowthDirection.Down;
        else if (y_Axis_SprayDirection <= y_Min)
            y_Axis_GrowthDirection = Y_Axis_GrowthDirection.Up;
    }
}
