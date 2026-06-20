using System;
using System.Linq;
using _Project.Scripts.SOAP.Events;
using Project.Scripts.Core;
using Project.Scripts.Input;
using Project.Scripts.UI.PlayerControllerButtons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Scripts.Pieces
{
    public class Tetromino : MonoBehaviour
    {
        [SerializeField] private float fallTime = 0.8f;
        [SerializeField] private Vector3 rotationPoint;
        [field: SerializeField] public Sprite sprite;
        [SerializeField] private FloatEvent setFallTimeEvent;
        [SerializeField] private TransformEvent addToGridEvent;
        [SerializeField] private VoidEvent checkLinesEvent;
        [SerializeField] private VoidEvent generateTetrominoEvent;
        private float previousTIme;
        private int rotationState;
        private bool isSoftDropping;
        private float minFallTime = 0.05f;
        private void OnEnable()
        {
            foreach (var moveButton in FindObjectsByType<HorizontalMoveButton>())
            {
                moveButton.OnHorizontalMoveButtonPressed += MoveHorizontal;
            }
            FindAnyObjectByType<SoftDropButton>().OnSoftDropButtonPressed += SetSoftDropping;
            FindAnyObjectByType<RotateButton>().OnRotateButtonPressed += RotateTetromino;
            setFallTimeEvent.Subscribe(SetFallSpeed);
            
        }

        private void OnDestroy()
        {
            setFallTimeEvent.Unsubscribe(SetFallSpeed);
        }

        private void Update()
        {
            
            HandleFallTime();
        }

        private void HandleFallTime()
        {
            if (Time.time - previousTIme > (isSoftDropping ? fallTime / 20f : fallTime))
            {
                transform.position += Vector3.down;
                if (!IsWithin())
                {
                    transform.position -= Vector3.down;
                    addToGridEvent.Raise(transform);
                    checkLinesEvent.Raise();
                    foreach (var moveButton in FindObjectsByType<HorizontalMoveButton>())
                    {
                        moveButton.OnHorizontalMoveButtonPressed -= MoveHorizontal;
                    }
                    FindAnyObjectByType<SoftDropButton>().OnSoftDropButtonPressed -= SetSoftDropping;
                    FindAnyObjectByType<RotateButton>().OnRotateButtonPressed -= RotateTetromino;
                    generateTetrominoEvent.Raise();
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

        private void SetFallSpeed(float decrement)
        {
            if (fallTime > minFallTime)
            {
                fallTime -= decrement;
            }
        }
       
    }
}
