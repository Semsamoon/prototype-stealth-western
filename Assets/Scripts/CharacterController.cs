using UnityEngine;

public abstract class CharacterController : MonoBehaviour
{
    protected CharacterMovement _movement;
    protected Animator _animator;

    protected virtual void Awake()
    {
        _movement = GetComponent<CharacterMovement>();
        _animator = GetComponent<Animator>();
    }
}