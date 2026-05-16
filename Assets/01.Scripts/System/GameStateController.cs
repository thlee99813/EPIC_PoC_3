using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour
{
    [SerializeField] private GameObject _panelGameEnd;
    [SerializeField] private GameObject _panelGameClear;
    private GameState _currentState = GameState.Paused;
    public GameState CurrentState => _currentState;
    public event Action<GameState> Changed;

    private void Start()
    {
        _panelGameEnd.SetActive(false);
        _panelGameClear.SetActive(false);
        SetState(GameState.Playing);
    }

    public void SetState(GameState state)
    {
        if (_currentState == state)
        {
            return;
        }

        _currentState = state;
        ApplyState(state);
        Changed?.Invoke(state);
    }

    public void GameOver()
    {
        SetState(GameState.GameOver);
    }

    public void GameClear()
    {
        SetState(GameState.GameClear);
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ApplyState(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
            Time.timeScale = 1f;
            _panelGameEnd.SetActive(false);
            _panelGameClear.SetActive(false);
            break;

            case GameState.BuildMode:
                Time.timeScale = 0f;
                break;

            case GameState.Event:
                Time.timeScale = 0f;
                break;

            case GameState.Paused:
                Time.timeScale = 0f;
                break;

            case GameState.GameOver:
                Time.timeScale = 0f;
                _panelGameEnd.SetActive(true);
                break;
            case GameState.GameClear:
                Time.timeScale = 0f;
                _panelGameClear.SetActive(true);
                break;
        }
    }
}