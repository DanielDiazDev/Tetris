using System;
using Project.Scripts.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Scripts.Pieces
{
    public class Tetromino : MonoBehaviour
    {
        private float previousTIme;
        public float fallTime = 0.8f;
        public float moveSpeed = 0.8f;

        public static int height = 20;
        public static int width = 10;
        public Vector3 rotationPoint; 
      
        private static Transform[,] grid = new Transform[width, height];
        public static int score;
        public static int levelDifficulty;
        private int rotationState = 0;
        private void Update()
        {
            
            HandleHorizontalMovement();
            HandleFallTime();
            RotateTetromino();
            IncrementLevel();
            IncrementDifficulty(); 
        }

        private void HandleFallTime()
        {
            if (Time.time - previousTIme > (Keyboard.current.downArrowKey.isPressed ? fallTime / 20f : fallTime))
            {
                transform.position += Vector3.down;
                if (!IsWithin())
                {
                    transform.position -= Vector3.down;
                    AddToGrid();
                    CheckLines();
                    FindAnyObjectByType<TetrominoGenerator>().Generate(); //Revisar para que pare
                    enabled = false;
                }
                previousTIme = Time.time;
            }
        }

        private void HandleHorizontalMovement()
        {
            if (InputManager.Instance.IsDragging && InputManager.Instance.DragDelta != Vector2.zero)
            {
                float deltaX = InputManager.Instance.DragDelta.x;

                if (Mathf.Abs(deltaX) > 0.01f) 
                {
                    Vector3 dir = deltaX > 0 ? Vector3.right : Vector3.left;

                    transform.position += dir * (moveSpeed * Time.deltaTime);

                    if (!IsWithin())
                    {
                        transform.position -= dir * (moveSpeed * Time.deltaTime);
                    }
                }
            }
        }

        private void CheckLines()
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

            score += 100;
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
        private bool IsWithin()
        {
            foreach (Transform child in transform)
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

        private void RotateTetromino()
        {
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                rotationState = (rotationState + 1) % 4;
            
                transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -90);
                if (!IsWithin())
                {
                    transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, 90);
                    rotationState = (rotationState + 3) % 4;
                }
            }
        }

        private void AddToGrid()
        {
            foreach (Transform child in transform)
            {
                int x = Mathf.RoundToInt(child.position.x);
                int y = Mathf.RoundToInt(child.position.y);
                
                grid[x, y] = child;

                if (y >= 19)
                {
                    levelDifficulty = 0;
                    score = 0;
                    fallTime = 0.8f;
                    Debug.Log("GameOver");
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
                    fallTime = 0.4f;
                    break;
                case 2:
                    fallTime = 0.2f;
                    break;
            }
        }
    }
}
