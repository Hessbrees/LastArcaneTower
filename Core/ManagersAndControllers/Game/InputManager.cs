using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SwipeDetection))]
public class InputManager : Singleton<InputManager>, IPlayerLoader
{
    public delegate void StartTouchEvent(Vector2 position, float time);
    public StartTouchEvent StartTouch;
    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent EndTouch;

    public delegate void SwipeDownEvent(Vector2 position);
    public event SwipeDownEvent SwipeDown;
    public delegate void SwipeUpEvent(Vector2 position);
    public event SwipeUpEvent SwipeUp;
    public delegate void ProjectileSwipeEvent(Vector2 position);
    public event ProjectileSwipeEvent ProjectileSwipe;

    public delegate void TouchHoldEvent();
    public event TouchHoldEvent TouchHold;

    public delegate void SpellThrowEvent(SpellTypes spellType, Vector2 direction);
    public SpellThrowEvent SpellThrow;

    private GestureController gestureController;

    private PlayerActions playerActions;
    private PlayerManager player;
    private Camera mainCamera;
    private SwipeDetection swipeDetection;

    private float startTouchTime;

    public GestureController GestureController => gestureController;
    public SwipeDetection SwipeDetection => swipeDetection;
    public bool SpecialSpellModeActive => gestureController.SpecialSpellModeActive;
    public Vector2 SwipeDirection => swipeDetection.GetSwipeDirection();

    public bool IsPlayerDeadOrNotLoaded
    {
        get
        {
            if (player == null || player.IsPlayerDead)
                return true;
            else
                return false;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
        playerActions = new PlayerActions();

        gestureController = transform.Find("GestureController").GetComponent<GestureController>();
        swipeDetection = GetComponent<SwipeDetection>();

        playerActions.Touch.TouchPress.started += ctx => OnStartTouch(ctx);
        playerActions.Touch.TouchPress.canceled += ctx => OnEndTouch(ctx);
        playerActions.Touch.TouchHold.performed += ctx => OnTouchHold(ctx);

        StartTouch += gestureController.OnStartTouch;
        EndTouch += gestureController.OnEndTouch;
    }

    private void OnEnable() => playerActions.Enable();

    private void OnDisable() => playerActions.Disable();

    public void ToggleSpecialSpellMode() => gestureController.ToggleSpecialSpellMode();

    public void OnGestureRecognized(string recognizedSpellName) =>
        GameManager.Instance.OnGestureRecognizedWithSpellName(recognizedSpellName);

    public Vector2 PrimaryPosition() => ScreenToWorld(mainCamera, Touchscreen.current.primaryTouch.position.ReadValue());

    public void LoadPlayer() => player = PlayerManager.Instance;

    public void OnTouchHold(InputAction.CallbackContext context)
    {
        if (UI_Manager.Instance.Is_UI_Active || gestureController.SpecialSpellModeActive || IsPlayerDeadOrNotLoaded)
            return;

        var position = ScreenToWorld(mainCamera, Touchscreen.current.primaryTouch.position.ReadValue());

        if (TouchHold != null) TouchHold();
    }

    public void OnEndTouch(InputAction.CallbackContext context)
    {
        if (UI_Manager.Instance.Is_UI_Active || IsPlayerDeadOrNotLoaded)
            return;

        if (TouchIsOverUI())
            return;

        var worldPosition = ScreenToWorld(mainCamera, Touchscreen.current.primaryTouch.position.ReadValue());

        if (EndTouch != null) EndTouch(worldPosition, (float)context.time);
    }

    public void OnStartTouch(InputAction.CallbackContext context)
    {
        if (UI_Manager.Instance.Is_UI_Active || IsPlayerDeadOrNotLoaded)
            return;

        if (TouchIsOverUI())
            return;

        startTouchTime = (float)context.time;

        StartCoroutine(SkipFirstTouchPosition());
    }

    public void OnSwipeUp(Vector2 position)
    {
        if (UI_Manager.Instance.Is_UI_Active || IsPlayerDeadOrNotLoaded)
            return;

        if (SwipeUp != null) SwipeUp(position);

        if (SpellThrow != null) SpellThrow(SpellTypes.Wall, position);
    }

    public void OnSwipeDown(Vector2 position)
    {
        if (UI_Manager.Instance.Is_UI_Active || IsPlayerDeadOrNotLoaded)
            return;

        if (SwipeDown != null) SwipeDown(position);

        if (SpellThrow != null) SpellThrow(SpellTypes.Skydrop, position);
    }

    public void OnProjectileSwipe(Vector2 position)
    {
        if (UI_Manager.Instance.Is_UI_Active || IsPlayerDeadOrNotLoaded)
            return;

        if (ProjectileSwipe != null) ProjectileSwipe(position);

        if (SpellThrow != null) SpellThrow(SpellTypes.Projectile, position);
    }

    public void ThrowSpecialSpell(SpellTypes spellType, Vector2 position)
    {
        if (UI_Manager.Instance.Is_UI_Active || IsPlayerDeadOrNotLoaded)
            return;

        if (SpellThrow != null) SpellThrow(SpellTypes.Special, position);
    }

    private bool TouchIsOverUI()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Input.GetTouch(0).position;
        List<RaycastResult> results = new List<RaycastResult>();
        UI_Manager.Instance.GameSceneUIGraphicRaycaster.Raycast(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    private IEnumerator SkipFirstTouchPosition()
    {
        yield return null;

        var worldPosition = ScreenToWorld(mainCamera, Touchscreen.current.primaryTouch.position.ReadValue());

        if (StartTouch != null) StartTouch(worldPosition, startTouchTime);
    }

    public static Vector2 ScreenToWorld(Camera camera, Vector3 position) => camera.ScreenToWorldPoint(position);
}
