using System;
using System.Linq;
using Project.Scripts.Core;
using Project.Scripts.Input;
using Project.Scripts.UI.PlayerControllerButtons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Scripts.Pieces
{
    public class Tetromino : MonoBehaviour
    {
        private float previousTIme;
        public float fallTime = 0.8f;
        public Vector3 rotationPoint; 
        private int rotationState = 0;
        private bool isSoftDropping;
        
        private void OnEnable()
        {
            foreach (var moveButton in FindObjectsByType<HorizontalMoveButton>())
            {
                moveButton.OnHorizontalMoveButtonPressed += MoveHorizontal;
            }
            FindAnyObjectByType<SoftDropButton>().OnSoftDropButtonPressed += SetSoftDropping;
            FindAnyObjectByType<RotateButton>().OnRotateButtonPressed += RotateTetromino;
            
        }
        private void Update()
        {
            
            HandleFallTime();
            //IncrementLevel();
         //   IncrementDifficulty(); 
        }

        private void HandleFallTime()
        {
            if (Time.time - previousTIme > (isSoftDropping ? fallTime / 20f : fallTime))
            {
                transform.position += Vector3.down;
                if (!IsWithin())
                {
                    transform.position -= Vector3.down;
                    GameBootstrap.Instance.AddToGrid(transform);
                    GameBootstrap.Instance.CheckLines();
                    foreach (var moveButton in FindObjectsByType<HorizontalMoveButton>())
                    {
                        moveButton.OnHorizontalMoveButtonPressed -= MoveHorizontal;
                    }
                    FindAnyObjectByType<SoftDropButton>().OnSoftDropButtonPressed -= SetSoftDropping;
                    FindAnyObjectByType<RotateButton>().OnRotateButtonPressed -= RotateTetromino;
                    GameBootstrap.Instance.Generate();
                    enabled = false;
                }
                previousTIme = Time.time;
            }
        }

        private void MoveHorizontal(Vector3 direction)
        {
            transform.position += direction;
            if (!IsWithin())
            {
                transform.position -= direction;
            }
        }
        private bool IsWithin()
        {
            foreach (Transform child in transform)
            {
                int x = Mathf.RoundToInt(child.position.x);
                int y = Mathf.RoundToInt(child.position.y);
                if (x < 0 || x >= GameBootstrap.width || y < 0 || y >= GameBootstrap.height)
                {
                    return false;
                }

                if (GameBootstrap.grid[x, y] != null)
                {
                    return false;
                }
            }
            return true;
        }

        private void RotateTetromino()
        {
            rotationState = (rotationState + 1) % 4;
            
                transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -90);
                if (!IsWithin())
                {
                    transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, 90);
                    rotationState = (rotationState + 3) % 4;
                }
        }

        private void SetSoftDropping(bool isDropping)
        {
            isSoftDropping = isDropping;
        }
       
    }
}
