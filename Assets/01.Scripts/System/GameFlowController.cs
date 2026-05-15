using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowController : MonoBehaviour
{
    [SerializeField] private GameObject _panelGameEnd;

    private void Start()
    {
        _panelGameEnd.SetActive(false);
    }

    public void ShowGameEnd()
    {
        _panelGameEnd.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}