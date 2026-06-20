using System;
using _Project.Scripts.Score;
using _Project.Scripts.SOAP.Events;
using Project.Scripts.Pieces;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class GameBootstrap : MonoBehaviour
    {
        //public static GameBootstrap Instance {get; private set;}
        [SerializeField] private int addScore = 100;
        public static int height = 20;
        public static int width = 10;
        [SerializeField] private TetrominoGenerator  tetrominoGenerator;
        [SerializeField] private IntEvent addScoreEvent;
        [SerializeField] private FloatEvent setFallTimeEvent;
        [SerializeField] private VoidEvent generateTetrominoEvent;
        [SerializeField] private TransformEvent addToGridEvent;
        [SerializeField] private VoidEvent checkLinesEvent;
        [SerializeField] private IntEvent finalScoreEvent;
        
        private float fallTimeDecrement = 0.001f;
        public static Transform[,] grid = new Transform[width, height];
        private bool isGameOver;

        private void Awake()
        {
           // Instance = this;
           ResetGame();
        }
        private void OnEnable()
        {
            generateTetrominoEvent.Subscribe(Generate);
            addToGridEvent.Subscribe(AddToGrid);
            checkLinesEvent.Subscribe(CheckLines);
        }

        private void OnDestroy()
        {
            generateTetrominoEvent.Unsubscribe(Generate);
            addToGridEvent.Unsubscribe(AddToGrid);
            checkLinesEvent.Unsubscribe(CheckLines);
        }
        private void AddToGrid(Transform tetromino)
        {
            foreach (Transform child in tetromino)
            {
                int x = Mathf.RoundToInt(child.position.x);
                int y = Mathf.RoundToInt(child.position.y);
                
                grid[x, y] = child;

                if (y >= 19) 
                {
                    isGameOver = true;
                   GameOverTest.Instance.ShowGameOver();
                   finalScoreEvent.Raise(ScoreSystem.Instance.GetScore());
                }
            }
        }
        
        private void CheckLines(Unit _)
        {
            for (int i = height - 1; i >= 0; i--)
            {
                if (HaveLine(i))
                {
                    ClearLine(i);
                    DownLine(i);
                }
            }
        }
        private bool HaveLine(int i)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, i] == null)
                {
                    return false;
                }
            }
            ScoreSystem.Instance.AddScore(addScore);
            CheckLevelProgression();
            return true;
        }

        private void ClearLine(int i)
        {
            for (int j = 0; j < width; j++)
            {
                Destroy(grid[j, i].gameObject);
                grid[j, i] = null;  
            }
        }
       
        private void DownLine(int i)
        {
            for (int y = i; y < height; y++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (grid[j, y] != null)
                    {
                        grid[j, y - 1] = grid[j, y];
                        grid[j, y] = null;
                        grid[j, y -1].position += Vector3.down;
                    }
                }
            }
        }

        private void Generate(Unit _)
        {
            if(isGameOver) return;
            tetrominoGenerator.Generate();
        }
        private void CheckLevelProgression()
        {
            int currentScore = ScoreSystem.Instance.GetScore();
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
             //ScoreSystem.Instance.ResetScore();
            // setFallTimeEvent.Raise(0.8f);
        }

    }
}