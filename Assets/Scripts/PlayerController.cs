using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PlayerController : CharacterController
{
    [SerializeField] private Animator _barrelAnimator;
    [SerializeField] private GameObject _barrelDestruction;
    [SerializeField] private GameController _gameController;

    private PlayerHealth _health;

    private enum State
    {
        Moving,
        Stealth
    }

    private State _currentState;
    private PlayerInput _playerInput;
    private Vector2 _inputDirection;

    protected override void Awake()
    {
        base.Awake();
        _playerInput = GetComponent<PlayerInput>();
        _health = GetComponent<PlayerHealth>();
        _health.OnDeath += OnDeath;
        _currentState = State.Stealth;
        _gameController.OnGameOver += OnGameOver;
    }

    private void OnDeath()
    {
        _animator.SetTrigger("Die");
        _barrelAnimator.gameObject.SetActive(false);
        _barrelDestruction.SetActive(true);
        _currentState = State.Moving;
        _movement.Move(Vector2.zero);
        enabled = false;
        _gameController.OnGameOver -= OnGameOver;
        _gameController.Lose();
    }

    private void OnGameOver()
    {
        _animator.SetBool("IsMoving", false);
        _currentState = State.Moving;
        _movement.Move(Vector2.zero);
        enabled = false;
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

    private void UpdateState()
    {
        if (_currentState == State.Moving) HandleMovingState();
        else HandleStealthState();
    }

    private void HandleMovingState()
    {
        if (_movement.IsMoving()) return;
        TransitionToStealth();
    }

    private void HandleStealthState()
    {
        if (!_movement.IsMoving()) return;
        TransitionToMoving();
    }

    private void TransitionToMoving()
    {
        _currentState = State.Moving;
        _animator.SetBool("IsMoving", true);
        _barrelAnimator.SetBool("IsMoving", true);
    }

    private void TransitionToStealth()
    {
        _currentState = State.Stealth;
        _animator.SetBool("IsMoving", false);
        _barrelAnimator.SetBool("IsMoving", false);
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

    public bool IsInStealth()
    {
        return _currentState == State.Stealth;
    }
}