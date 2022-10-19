using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(2)]
public class SpellAnimationLogic : MonoBehaviour
{
    [SerializeField] private List<AnimationClip> clips;

    private SpellMonoBehaviour parentSpell;
    private Animator animator;

    private bool isAnimationPlaying;

    public bool IsAnimationPlaying => isAnimationPlaying;

    private void Awake()
    {
        parentSpell = GetComponent<SpellMonoBehaviour>();
        animator = parentSpell.Animator;
    }

    public float ProcessSpellAnimation()
    {
        if (isAnimationPlaying)
            return 0;

        if (clips == null || clips.Count == 0)
            return 0;

        isAnimationPlaying = true;
        animator.SetBool("spellHitBool", true);
        StartCoroutine(DelayAnimationEnd(clips[1].length));

        return clips[1].length;
    }

    private IEnumerator DelayAnimationEnd(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        animator.SetBool("spellHitBool", false);
        isAnimationPlaying = false;
    }
}
