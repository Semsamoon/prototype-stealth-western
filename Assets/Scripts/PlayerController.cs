using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PlayerController : CharacterController
{
    private PlayerInput _playerInput;
    private Vector2 _inputDirection;

    protected override void Awake()
    {
        base.Awake();
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        if (_playerInput == null) return;
        _playerInput.actions["Move"].performed += OnMove;
        _playerInput.actions["Move"].canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        if (_playerInput == null) return;
        _playerInput.actions["Move"].performed -= OnMove;
        _playerInput.actions["Move"].canceled -= OnMoveCanceled;
    }

    private void Update()
    {
        UpdateState();
        MoveCharacter();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _inputDirection = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _inputDirection = Vector2.zero;
    }

    private void MoveCharacter()
    {
        _movement.Move(_inputDirection);
    }
}