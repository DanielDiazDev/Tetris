using System;
using _Project.Scripts.Auth;
using _Project.Scripts.Core;
using _Project.Scripts.PlayfabServices;
using _Project.Scripts.SOAP.Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private GameOverResultEvent gameOverResultEvent;
        [SerializeField] private Button saveFinalScoreButton;
        [SerializeField] private TextMeshProUGUI recordText;
        [SerializeField] private Button resetButton;
        [SerializeField] private int resetCost = 10;
        private int finalScore;
        private int finalLevel;

        private void OnEnable()
        {
            gameOverResultEvent.Subscribe(SetFinalScore);
            resetButton.onClick.AddListener(OnResetClicked);
            resetButton.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            gameOverResultEvent.Unsubscribe(SetFinalScore);
            resetButton.onClick.RemoveListener(OnResetClicked);
        }

        private void SetFinalScore(GameOverData gameOverData)
        {
            finalScore = gameOverData.score;
            finalLevel = gameOverData.level;
            finalScoreText.text = $"Final Score: {gameOverData.score}";
            recordText.gameObject.SetActive(gameOverData.isNewRecord);

            var currencyService = ServiceLocator.Instance.GetService<ICurrencyService>();
            currencyService.GetBalance("SC", balance =>
            {
                resetButton.gameObject.SetActive(balance >= resetCost);
            });
        }

        private void OnResetClicked()
        {
            PlayerPrefs.SetInt("SavedScore", finalScore);
            PlayerPrefs.SetInt("SavedLevel", finalLevel);

            ServiceLocator.Instance.GetService<ICurrencyService>().SubtractCurrency("SC", resetCost, _ =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }
    }
}