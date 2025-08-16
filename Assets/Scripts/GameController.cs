using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Task = System.Threading.Tasks.Task;

[RequireComponent(typeof(BoxCollider))]
public sealed class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _endGameScreen;
    [SerializeField] private TextMeshProUGUI _resultText;

    private bool _isGameOver = false;

    public event Action OnGameOver;

    private void Awake()
    {
        _endGameScreen.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerController>(out _)) return;
        EndGame(true);
    }

    public void Lose()
    {
        EndGame(false);
    }

    private async void EndGame(bool isWin)
    {
        if (_isGameOver) return;
        _isGameOver = true;
        OnGameOver?.Invoke();

        await Task.Delay(1000);
        _endGameScreen.SetActive(true);
        _resultText.text = isWin ? "You Win!" : "You Lose!";
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
    }
}