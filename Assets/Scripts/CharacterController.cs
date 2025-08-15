using UnityEngine;

public abstract class CharacterController : MonoBehaviour
{
    [SerializeField] protected Animator _animator;

    protected CharacterMovement _movement;

    protected virtual void Awake()
    {
        _movement = GetComponent<CharacterMovement>();
    }
}