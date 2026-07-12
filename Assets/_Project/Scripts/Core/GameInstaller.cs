using System;
using _Project.Scripts.Auth;
using _Project.Scripts.GameOver;
using _Project.Scripts.Grid;
using _Project.Scripts.Prefs;
using _Project.Scripts.Score;
using _Project.Scripts.SOAP.Events;
using _Project.Scripts.UINodeGraph.Runtime;
using Project.Scripts.Pieces;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private TetrominoGenerator  tetrominoGenerator;
        [SerializeField] private IntEvent addScoreEvent;
        [SerializeField] private FloatEvent setFallTimeEvent;
        [SerializeField] private GameOverResultEvent gameOverResultEvent;
        [SerializeField] private ScoreModel scoreModel;
        [SerializeField] private IntEvent setLevelEvent;
        private IGridSystem gridSystem;
        private IScoreSystem scoreSystem;
        private GameOverSystem gameOverSystem;
        private LevelSystem levelSystem;
       // private ILeaderboardService leaderboardService;
        private void Awake()
        {
           gridSystem = new GridSystem(10, 20, 1f, new Vector3(-5, -10, 0));
           scoreSystem = new ScoreSystem(scoreModel, gridSystem);
           levelSystem = new LevelSystem(gridSystem, setFallTimeEvent, scoreSystem, setLevelEvent);
           gameOverSystem = new GameOverSystem(tetrominoGenerator, gameOverResultEvent, scoreSystem, gridSystem, levelSystem);
        //   leaderboardService = new PlayFabLeaderboardService();
         //  ServiceLocator.Instance.RegisterService(leaderboardService);

           if (PlayerPrefs.HasKey("SavedScore"))
           {
               int savedScore = PlayerPrefs.GetInt("SavedScore");
               int savedLevel = PlayerPrefs.GetInt("SavedLevel");
               scoreModel.SetScore(savedScore);
               levelSystem.SetLevel(savedLevel);
               PlayerPrefs.DeleteKey("SavedScore");
               PlayerPrefs.DeleteKey("SavedLevel");
           }

           tetrominoGenerator.Init(gridSystem);
           
        }
    }
}