using _Project.Scripts.SOAP.Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public sealed class GameOverSceneController : MonoBehaviour
{
    [SerializeField] private GameOverResultEvent gameOverResultEvent;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button leaderboardButton;

    private void Awake()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        Hide();
    }

    private void OnEnable()
    {
        if (gameOverResultEvent != null) gameOverResultEvent.Subscribe(ShowGameOver);
        if (exitButton != null) exitButton.onClick.AddListener(ExitToMenu);
        if (resetButton != null) resetButton.onClick.AddListener(RestartGame);
        if (leaderboardButton != null) leaderboardButton.onClick.AddListener(OpenLeaderboard);
    }

    private void OnDisable()
    {
        if (gameOverResultEvent != null) gameOverResultEvent.Unsubscribe(ShowGameOver);
        if (exitButton != null) exitButton.onClick.RemoveListener(ExitToMenu);
        if (resetButton != null) resetButton.onClick.RemoveListener(RestartGame);
        if (leaderboardButton != null) leaderboardButton.onClick.RemoveListener(OpenLeaderboard);
    }

    private void ShowGameOver(GameOverData result)
    {
        scoreText.text = result.score.ToString("N0");
        recordText.text = result.isNewRecord ? "NEW RECORD!" : "NO NEW RECORD";
        recordText.color = result.isNewRecord ? new Color32(255, 209, 102, 255) : new Color32(177, 192, 224, 255);
        if (levelText != null) levelText.text = $"LEVEL {result.level}";
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void Hide()
    {
        if (canvasGroup == null) return;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    private void ExitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OpenLeaderboard()
    {
        PlayerPrefs.SetInt("OpenLeaderboardOnLoad", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }
}
