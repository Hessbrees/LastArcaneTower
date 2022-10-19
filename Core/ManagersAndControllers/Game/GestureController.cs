using System.Collections;
using UnityEngine;

[RequireComponent(typeof(GestureRecognizer))]

public class GestureController : MonoBehaviour
{
    private GestureRecognizer gestureRecognizer;
    private TrailController trailController;

    private bool specialSpellModeActive;

    public GestureRecognizer GestureRecognizer => gestureRecognizer;

    public bool SpecialSpellModeActive => specialSpellModeActive;

    private void Awake() => trailController = transform.Find("TrailController").GetComponent<TrailController>();

    void Start() => gestureRecognizer = GetComponent<GestureRecognizer>();

    public void OnGestureRead(string recognizedSpellName)
    {
        if (recognizedSpellName == Settings.GestureErrorMessage)
        {
            // Some error handling stuff xd
        }
        else
        {
            InputManager.Instance.OnGestureRecognized(recognizedSpellName);
            StartCoroutine(DelaySpecialSpellModeDeactivation(0.2f));
        }

        trailController.Clear();
    }

    public void OnEndTouch(Vector2 position, float time)
    {
        if (!SpecialSpellModeActive)
            return;

        trailController.OnEndTouch(position, time);
        GestureRecognizer.OnEndTouch(position, time);
    }

    public void OnStartTouch(Vector2 position, float time)
    {
        if (!SpecialSpellModeActive)
            return;
        trailController.OnStartTouch(position, time);
        GestureRecognizer.OnStartTouch(position, time);
    }

    public void ToggleSpecialSpellMode() => specialSpellModeActive = !specialSpellModeActive;

    private IEnumerator DelaySpecialSpellModeDeactivation(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        ToggleSpecialSpellMode();
    }
}
