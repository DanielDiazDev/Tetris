using System;
using _Project.Scripts.SOAP.Events;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private IntEvent finalScoreEvent;
        [SerializeField] private Button resetButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button saveFinalScoreButton;
        private int finalScore;
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
            resetButton.onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
            exitButton.onClick.AddListener(Application.Quit);
            saveFinalScoreButton.onClick.AddListener(() => Debug.Log(finalScore));
        }

        private void SetFinalScore(int score)
        {
            finalScore = score;
            finalScoreText.text = $"Final Score: {score}";
        }
    }
}