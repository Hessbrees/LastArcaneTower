using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(2)]
public class TrailController : MonoBehaviour
{
    [SerializeField] private float defaultTrailLifeTime;
    [SerializeField] private float specialSpellModeTrailLifeTime;

    private Coroutine trailCoroutine;
    private TrailRenderer trailRenderer;

    private void Update() =>  SetTrailLifeTime();

    private void Start() => trailRenderer = GetComponent<TrailRenderer>();

    public void Clear() => trailRenderer.Clear();

    private void SetTrailLifeTime()
    {
        if (InputManager.Instance.GestureController.SpecialSpellModeActive)
            trailRenderer.time = specialSpellModeTrailLifeTime;
        else
            trailRenderer.time = defaultTrailLifeTime;
    }

    public void OnEndTouch(Vector2 position, float time) =>  ToggleTrail(false);
 
    public void OnStartTouch(Vector2 position, float time) => ToggleTrail(true);

    private void ToggleTrail(bool active)
    {
        trailRenderer.enabled = active;
        if (active)
            trailCoroutine = StartCoroutine(Trail());
        else if (!active && trailCoroutine != null)
            StopCoroutine(trailCoroutine);
    }

    private IEnumerator Trail()
    {
        while (true)
        {
            transform.position = InputManager.Instance.PrimaryPosition();
            yield return null;
        }
    }
}
