using System;
using Project.Scripts.Pieces;
using UnityEngine;

namespace Project.Scripts.Core
{
    public class GameBootstrap : MonoBehaviour
    {
        public static GameBootstrap Instance {get; private set;}
        public static int score;
        public static int levelDifficulty;
        public static int height = 20;
        public static int width = 10;
        [SerializeField] private TetrominoGenerator  tetrominoGenerator;
        
        public static Transform[,] grid = new Transform[width, height];

        private void Awake()
        {
            Instance = this;
        }

        public void AddToGrid(Transform tetromino)
        {
            foreach (Transform child in tetromino)
            {
                int x = Mathf.RoundToInt(child.position.x);
                int y = Mathf.RoundToInt(child.position.y);
                
                grid[x, y] = child;

                // if (y >= 19) Reset
                // {
                //     levelDifficulty = 0;
                //     score = 0;
                //     fallTime = 0.8f;
                //     Debug.Log("GameOver");
                // }
            }
        }

        public void Generate()
        {
            tetrominoGenerator.Generate();
        }
        public void CheckLines()
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

            //score += 100; Llamar evento de sumar puntos
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
        private void IncrementLevel()
        {
            switch (score)
            {
                case 200:
                    levelDifficulty++;
                    break;
                case 400:
                    levelDifficulty++;
                    break;
                
            }
        }

        private void IncrementDifficulty()
        {
            switch (levelDifficulty)
            {
                case 1:
                 //   fallTime = 0.4f;
                    break;
                case 2:
                   // fallTime = 0.2f;
                    break;
            }
        }
    }
}