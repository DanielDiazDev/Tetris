using System;
using _Project.Scripts.Auth;
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
        [SerializeField] private IntEvent finalScoreEvent;
        [SerializeField] private Button saveFinalScoreButton;
        private int finalScore;
        private PlayFabLeaderboard leaderboard;

        private void OnEnable()
        {
            finalScoreEvent.Subscribe(SetFinalScore);
        }

        private void OnDestroy()
        {
            finalScoreEvent.Unsubscribe(SetFinalScore);
        }

        private void Awake()
        {
         //  resetButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
          //  exitButton.onClick.AddListener(Application.Quit);
          //  saveFinalScoreButton.onClick.AddListener(() => leaderboard.UpdatePlayerStatics("Leaderboard", finalScore));
            saveFinalScoreButton.onClick.AddListener(() => PlayFabLeaderboard.Instance.UpdatePlayerStatics("Leaderboard", finalScore));
        }

        // public void Init(PlayFabLeaderboard leaderboard)
        // {
        //     this.leaderboard = leaderboard;
        // }

        private void SetFinalScore(int score)
        {
            finalScore = score;
            finalScoreText.text = $"Final Score: {score}";
        }
    }
}