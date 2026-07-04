using System;
using _Project.Scripts.SOAP.Events;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Grid
{
    public class GridSystem : IGridSystem
    {
        private int addScore = 100;
        private Transform[,] grid;
        private int height;
        private int width;
        public int X => width;
        public int Y => height;
        public event Action OnGameOver;
        public event Action OnCheckLevel;
        public event Action<int> OnAddScore;
        

        public GridSystem(int height, int width)
        {
            grid = new Transform[width, height];
            this.height = height;
            this.width = width;
            
        }

        public void AddToGrid(Transform tetromino)
        {
            foreach (Transform child in tetromino)
            {
                int x = Mathf.RoundToInt(child.position.x);
                int y = Mathf.RoundToInt(child.position.y);
                
                grid[x, y] = child;

                if (y >= 19) 
                {
                    OnGameOver?.Invoke();
                }
            }
        }
        public bool IsWithin(Transform tetromino)
        {
            foreach (Transform child in tetromino)
            {
                int x = Mathf.RoundToInt(child.position.x);
                int y = Mathf.RoundToInt(child.position.y);
                if (x < 0 || x >= width || y < 0 || y >= height)
                {
                    return false;
                }
        
                if (grid[x, y] != null)
                {
                    return false;
                }
            }
            return true;
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
            OnAddScore?.Invoke(addScore);
            OnCheckLevel?.Invoke();
            return true;
        }

        private void ClearLine(int i)
        {
            for (int j = 0; j < width; j++)
            {
                Object.Destroy(grid[j, i].gameObject);
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

        
    }
}