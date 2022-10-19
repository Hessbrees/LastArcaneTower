using UnityEngine;

public class LevitatingRock : AnimationSpeed
{
    public enum RockAnimations
    {
        defaultAnim, 
        rockAnim1,
        rockAnim2,
        rockAnim3,
        rockAnim4,
        rockAnim5
    }

    [SerializeField] private RockAnimations anim;

    protected override void Awake() => animator = GetComponent<Animator>();

    protected override void Start()
    {
        animator.SetTrigger(anim.ToString());
        base.Start();
    }
}
