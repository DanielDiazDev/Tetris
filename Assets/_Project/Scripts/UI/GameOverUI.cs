using System;
using _Project.Scripts.Auth;
using _Project.Scripts.Core;
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
        private int finalScore;
      //  private PlayFabLeaderboard leaderboard;

        private void OnEnable()
        {
            gameOverResultEvent.Subscribe(SetFinalScore);
        }

        private void OnDestroy()
        {
            gameOverResultEvent.Unsubscribe(SetFinalScore);
        }
        private void SetFinalScore(GameOverData gameOverData)
        {
            finalScore = gameOverData.score;
            finalScoreText.text = $"Final Score: {gameOverData.score}";
            recordText.gameObject.SetActive(gameOverData.isNewRecord);

            // if (gameOverData.isNewRecord)
            // {
            //     saveFinalScoreButton.onClick.AddListener(() => 
            //         ServiceLocator.Instance.GetService<ILeaderboardService>()
            //             .UpdatePlayerStatics("Leaderboard", gameOverData.score));
            // }
        }

    }
}