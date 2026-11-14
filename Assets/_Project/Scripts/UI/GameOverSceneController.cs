using _Project.Scripts.PlayFab.Interfaces;
using _Project.Scripts.SOAP.Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityServiceLocator;

public sealed class GameOverSceneController : MonoBehaviour
{
    [SerializeField] private GameOverResultEvent gameOverResultEvent;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private int resetCost = 10;
    private int finalScore;
    private int finalLevel;
    private void OnEnable()
    {
        if (gameOverResultEvent != null) gameOverResultEvent.Subscribe(ShowGameOver);
        if (resetButton != null) resetButton.onClick.AddListener(RestartGame);
        if(exitButton != null) exitButton.onClick.AddListener(GoToMainMenu);
    }

    private void OnDisable()
    {
        if (gameOverResultEvent != null) gameOverResultEvent.Unsubscribe(ShowGameOver);
        if (resetButton != null) resetButton.onClick.RemoveListener(RestartGame);
    }

    private void ShowGameOver(GameOverData result)
    {
        finalScore = result.score;
        finalLevel = result.level;
        scoreText.text = result.score.ToString("N0");
        recordText.text = result.isNewRecord ? "NEW RECORD!" : "";
        recordText.color = result.isNewRecord ? new Color32(255, 209, 102, 255) : new Color32(177, 192, 224, 255);
        if (levelText != null) levelText.text = $"LEVEL {result.level}";
        var currencyService = ServiceLocator.Global.Get<ICurrencyService>();
        currencyService.GetBalance("SC", balance =>
        {
            resetButton.gameObject.SetActive(balance >= resetCost);
        });
    }

    private void RestartGame()
    {
        PlayerPrefs.SetInt("SavedScore", finalScore);
        PlayerPrefs.SetInt("SavedLevel", finalLevel);

        ServiceLocator.Global.Get<ICurrencyService>().SubtractCurrency("SC", resetCost, _ =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }
    private void GoToMainMenu()
    {
        ServiceLocator.Global.Unregister<ILogin>();
    }
 
}
