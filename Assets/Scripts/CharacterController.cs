using UnityEngine;

public abstract class CharacterController : MonoBehaviour
{
    protected enum State
    {
        Moving,
        Stealth
    }

    protected State _currentState;
    protected CharacterMovement _movement;
    protected Animator _animator;

    protected virtual void Awake()
    {
        _movement = GetComponent<CharacterMovement>();
        _animator = GetComponent<Animator>();
        _currentState = State.Stealth;
    }

    protected void UpdateState()
    {
        if (_currentState == State.Moving) HandleMovingState();
        else HandleStealthState();
    }

    protected virtual void HandleMovingState()
    {
        if (_movement.IsMoving()) return;
        TransitionToStealth();
    }

    protected virtual void HandleStealthState()
    {
        if (!_movement.IsMoving()) return;
        TransitionToMoving();
    }

    protected void TransitionToMoving()
    {
        _currentState = State.Moving;
        _animator.SetBool("IsMoving", true);
    }

    protected void TransitionToStealth()
    {
        _currentState = State.Stealth;
        _animator.SetBool("IsMoving", false);
    }
}