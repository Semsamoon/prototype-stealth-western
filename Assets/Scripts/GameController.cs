using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public sealed class GameController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerController>(out _)) return;
        Debug.Log("Game is finished!");
    }
}