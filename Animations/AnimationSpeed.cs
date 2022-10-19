using UnityEngine;

public class AnimationSpeed : MonoBehaviour
{
    [SerializeField] protected float animationSpeed;

    protected Animator animator;

    protected virtual void Awake() => animator = GetComponent<Animator>();

    protected virtual void Start() => SetAnimationSpeed();

    [ContextMenu("Set Animation Speed")]
    protected void SetAnimationSpeed() => animator.speed = animationSpeed;
}
