using UnityEngine;

public sealed class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;

    private Transform _transform;

    private void OnValidate()
    {
        if (!_target) return;
        transform.position = _target.position + _offset;
    }

    private void Awake()
    {
        _transform = transform;
    }

    private void LateUpdate()
    {
        var position = _target.position + _offset;
        position.y = _offset.y;
        _transform.position = position;
    }
}