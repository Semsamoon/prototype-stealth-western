using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public sealed class GameController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerController>(out _)) return;
        Time.timeScale = 0;
        Debug.Log("Game is finished!");
    }
}