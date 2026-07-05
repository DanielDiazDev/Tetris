using _Project.Scripts.Grid;
using _Project.Scripts.Score;
using _Project.Scripts.SOAP.Events;
using _Project.Scripts.UINodeGraph.Runtime;
using Project.Scripts.Pieces;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private TetrominoGenerator  tetrominoGenerator;
        [SerializeField] private IntEvent addScoreEvent;
        [SerializeField] private FloatEvent setFallTimeEvent;
        [SerializeField] private IntEvent finalScoreEvent;
        [SerializeField] private ScoreModel scoreModel;
       // [SerializeField] private LeaderboardModel leaderboardModel;
        //private PlayFabLeaderboard leaderboard;
        private IGridSystem gridSystem;
        private IScoreSystem scoreSystem;
        private float fallTimeDecrement = 0.001f;
        
        private void Awake()
        {
           ResetGame();
           gridSystem = new GridSystem(10, 20, 1f, new Vector3(-5, -10, 0));
           scoreSystem = new ScoreSystem(scoreModel);
           tetrominoGenerator.Init(gridSystem);
        }
        private void OnEnable()
        {
            gridSystem.OnGameOver += HandleGameOver;
            gridSystem.OnAddScore += HandleScore;
            gridSystem.OnCheckLevel += CheckLevelProgression;
        }
        private void OnDestroy()
        {
            gridSystem.OnGameOver -= HandleGameOver;
            gridSystem.OnAddScore -= HandleScore;
            gridSystem.OnCheckLevel -= CheckLevelProgression;
        }
        private void HandleGameOver()
        {
            tetrominoGenerator.Stop();
            UINavigationManager.Instance.NavigateTo("9e392b27-fe0f-476a-975f-a5c2983282d9");
            finalScoreEvent.Raise(scoreSystem.GetScore());
        }
        private void HandleScore(int score) => scoreSystem.AddScore(score);

        private void CheckLevelProgression()
        {
            int currentScore = scoreSystem.GetScore();
            int threshold = 200;

            if (currentScore >= (LevelSystem.Instance.Level + 1) * threshold)
            {
                IncrementLevel();
            }
        }

        private void IncrementLevel()
        {
            LevelSystem.Instance.IncreaseLevel();
            setFallTimeEvent.Raise(fallTimeDecrement);
        }

        private void ResetGame()
        {
             LevelSystem.Instance.ResetLevel();
        }
    }
}