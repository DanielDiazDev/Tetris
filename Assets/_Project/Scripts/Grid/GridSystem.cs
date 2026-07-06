using System;
using _Project.Scripts.SOAP.Events;
using TMPro;
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
        private float cellSize;
        private Vector3 origin;
        public int X => width;
        public int Y => height;
        public event Action OnGameOver;
        public event Action OnCheckLevel;
        public event Action<int> OnAddScore;
        

        public GridSystem(int width, int height, float cellSize, Vector3 origin)
        {
            grid = new Transform[width, height];
            this.height = height;
            this.width = width;
            this.cellSize = cellSize;
            this.origin = origin;
            DrawDebugLines();
        }

        public void AddToGrid(Transform tetromino)
        {
            foreach (Transform child in tetromino)
            {
                var gridChildPosition = WorldToGrid(child.position);
                int x = gridChildPosition.x;
                int y = gridChildPosition.y;
                
                grid[x, y] = child;
                Debug.Log($"{x},{y} ");
                // if (y >= 16) 
                // {
                //     OnGameOver?.Invoke();
                // }
            }
        }
        public void TriggerGameOver()
        {
            OnGameOver?.Invoke();
        }
   
        private bool IsValid(int x, int y) => x >= 0 && y >= 0 && x < width && y < height;
        private void DrawDebugLines() 
        {
            const float duration = 100f;
            var parent = new GameObject("Debugging");

            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    CreateWorldText(parent, x + "," + y, GridToWorldCenter(x, y), Forward);
                    Debug.DrawLine(GridToWorld(x, y), GridToWorld(x, y + 1), Color.white, duration);
                    Debug.DrawLine(GridToWorld(x, y), GridToWorld(x + 1, y), Color.white, duration);
                }
            }

            Debug.DrawLine(GridToWorld(0, height), GridToWorld(width, height), Color.white, duration);
            Debug.DrawLine(GridToWorld(width, 0), GridToWorld(width, height), Color.white, duration);
        }
        
        TextMeshPro CreateWorldText(GameObject parent, string text, Vector3 position, Vector3 dir, 
            int fontSize = 2, Color color = default, TextAlignmentOptions textAnchor = TextAlignmentOptions.Center, int sortingOrder = 0) 
        {
            GameObject gameObject = new GameObject("DebugText_" + text, typeof(TextMeshPro));
            gameObject.transform.SetParent(parent.transform);
            gameObject.transform.position = position;
            gameObject.transform.forward = dir;

            TextMeshPro textMeshPro = gameObject.GetComponent<TextMeshPro>();
            textMeshPro.text = text;
            textMeshPro.fontSize = fontSize;
            textMeshPro.color = color == default ? Color.white : color;
            textMeshPro.alignment = textAnchor;
            textMeshPro.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

            return textMeshPro;
        }
        public bool IsWithin(Transform tetromino)
        {
            foreach (Transform child in tetromino)
            {
                var gridChildPosition = WorldToGrid(child.position);
                int x = gridChildPosition.x;
                int y = gridChildPosition.y;
                if (!IsValid(x, y))
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
        
        

        public Vector3 GridToWorld(int x, int y)
        {
            return new Vector3(x, y, 0) *  cellSize + origin;
        }
        public Vector3 GridToWorldCenter(int x, int y) 
        {
            return new Vector3(x * cellSize + cellSize * 0.5f, y * cellSize + cellSize * 0.5f, 0) + origin;
        }

        public Vector2Int WorldToGrid(Vector3 worldPosition) 
        {
            Vector3 gridPosition = (worldPosition - origin) / cellSize;
            var x = Mathf.FloorToInt(gridPosition.x);
            var y = Mathf.FloorToInt(gridPosition.y);
            return new Vector2Int(x, y);
        }
        
        public Vector3 Forward => Vector3.forward;

        
    }
}