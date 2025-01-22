using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public PlayerInput Input;

    public Action<Vector2> OnTouchStarted;
    public Action<Vector2> OnTouching;
    public Action<Vector2> OnTouchEnded;

    private InputAction OnPress;
    private InputAction OnPositionChange;

    private void Awake()
    {
        OnPress = Input.actions.FindAction("Pressed");
        OnPositionChange = Input.actions.FindAction("Moving");
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    private void OnTouch(InputAction.CallbackContext context)
    {
        Vector2 CurrentTouchPosition = Mouse.current.position.value;

        OnTouchStarted?.Invoke(CurrentTouchPosition);
    }

    private void OnTouchingDelta(InputAction.CallbackContext context)
    {
        Vector2 CurrentTouchPosition = context.ReadValue<Vector2>();

        OnTouching?.Invoke(CurrentTouchPosition);
    }

    private void OnEndTouch(InputAction.CallbackContext context)
    {
        Vector2 CurrentTouchPosition = Mouse.current.position.value;

        OnTouchEnded?.Invoke(CurrentTouchPosition);
    }

    private void SubscribeToEvents()
    {
        OnPress.started += OnTouch;
        OnPositionChange.performed += OnTouchingDelta;
        OnPress.canceled += OnEndTouch;
    }

    private void UnsubscribeFromEvents()
    {
        OnPress.started -= OnTouch;
        OnPositionChange.performed -= OnTouchingDelta;
        OnPress.canceled -= OnEndTouch;
    }
}