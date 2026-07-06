using System;
using System.Linq;
using _Project.Scripts.Grid;
using _Project.Scripts.SOAP.Events;
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
        [SerializeField] private TetrominoEvent tetrominoLockedEvent;
        [SerializeField] private Vector3Event swipeEvent;
        [SerializeField] private BoolEvent softDroppedEvent;
        [SerializeField] private VoidEvent rotationEvent;
        private float previousTIme;
        private int rotationState;
        private bool isSoftDropping;
        private float minFallTime = 0.05f;
        private IGridSystem gridSystem;
        private GhostTetrominoController ghostTetrominoController;
        private void OnEnable()
        {
            swipeEvent.Subscribe(MoveHorizontal);
            softDroppedEvent.Subscribe(SetSoftDropping);
            rotationEvent.Subscribe(RotateTetromino);
            setFallTimeEvent.Subscribe(SetFallSpeed);
            
        }

        private void OnDestroy()
        {
            swipeEvent.Unsubscribe(MoveHorizontal);
            softDroppedEvent.Unsubscribe(SetSoftDropping);
            rotationEvent.Unsubscribe(RotateTetromino);
            setFallTimeEvent.Unsubscribe(SetFallSpeed);
        }
        public void Init(IGridSystem gridSystem, GhostTetrominoController ghostTetrominoController)
        {
            this.gridSystem = gridSystem;
            this.ghostTetrominoController = ghostTetrominoController;
            ghostTetrominoController.CreateGhost(gameObject);
            ghostTetrominoController.UpdateGhost(gameObject);
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
                SnapParent();
                if (!gridSystem.IsWithin(transform))
                {
                    transform.position -= Vector3.down;
                    SnapParent();
                    swipeEvent.Unsubscribe(MoveHorizontal);
                    if (ghostTetrominoController?.GhostTetromino != null)
                    {
                        Destroy(ghostTetrominoController.GhostTetromino);
                    }
                    softDroppedEvent.Unsubscribe(SetSoftDropping);
                    rotationEvent.Unsubscribe(RotateTetromino);
                    tetrominoLockedEvent.Raise(this);
                    enabled = false;
                }
                previousTIme = Time.time;
            }
        }

        private void MoveHorizontal(Vector3 direction)
        {
            transform.position += direction;
            SnapParent();
            if (!gridSystem.IsWithin(transform))
            {
                transform.position -= direction;
                SnapParent();
            }
            ghostTetrominoController.UpdateGhost(gameObject);
        }
        private void SnapParent()
        {
            transform.position = gridSystem.GridToWorldCenter(
                gridSystem.WorldToGrid(transform.position).x,
                gridSystem.WorldToGrid(transform.position).y
            );
        }

        private void RotateTetromino(Unit _)
        {
            rotationState = (rotationState + 1) % 4;
            
                transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, -90);
               SnapParent();
                if (!gridSystem.IsWithin(transform))
                {
                    transform.RotateAround(transform.TransformPoint(rotationPoint), Vector3.forward, 90);
                    SnapParent();
                    rotationState = (rotationState + 3) % 4;
                }
            ghostTetrominoController.UpdateGhost(gameObject);
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
