using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public sealed class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;

    private Rigidbody _rigidbody;
    private Vector3 _moveDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector2 input)
    {
        CalculateMoveDirection(input);
        ApplyMovement();
    }

    public bool IsMoving()
    {
        return _moveDirection.sqrMagnitude > 0.01f;
    }

    private void CalculateMoveDirection(Vector2 input)
    {
        _moveDirection = new Vector3(input.x, 0f, input.y).normalized;
    }

    private void ApplyMovement()
    {
        var velocity = _moveDirection * _moveSpeed;
        velocity.y = _rigidbody.linearVelocity.y;
        _rigidbody.linearVelocity = velocity;
    }
}