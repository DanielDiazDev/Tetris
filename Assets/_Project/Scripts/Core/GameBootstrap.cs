using _Project.Scripts.GameOver;
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
        [SerializeField] private IntEvent setLevelEvent;
       // [SerializeField] private LeaderboardModel leaderboardModel;
        //private PlayFabLeaderboard leaderboard;
        private IGridSystem gridSystem;
        private IScoreSystem scoreSystem;
        private GameOverSystem gameOverSystem;
        private LevelSystem levelSystem;
        
        
        private void Awake()
        {
           gridSystem = new GridSystem(10, 20, 1f, new Vector3(-5, -10, 0));
           scoreSystem = new ScoreSystem(scoreModel, gridSystem);
           gameOverSystem = new GameOverSystem(tetrominoGenerator, finalScoreEvent, scoreSystem, gridSystem);
           levelSystem = new LevelSystem(gridSystem, setFallTimeEvent, scoreSystem, setLevelEvent);
           tetrominoGenerator.Init(gridSystem);
        }
    }
}