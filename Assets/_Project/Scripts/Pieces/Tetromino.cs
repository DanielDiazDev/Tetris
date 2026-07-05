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
        private float previousTIme;
        private int rotationState;
        private bool isSoftDropping;
        private float minFallTime = 0.05f;
        private IGridSystem gridSystem;
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
        public void Init(IGridSystem gridSystem)
        {
            this.gridSystem = gridSystem;
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
                    foreach (var moveButton in FindObjectsByType<HorizontalMoveButton>())
                    {
                        moveButton.OnHorizontalMoveButtonPressed -= MoveHorizontal;
                    }
                    FindAnyObjectByType<SoftDropButton>().OnSoftDropButtonPressed -= SetSoftDropping;
                    FindAnyObjectByType<RotateButton>().OnRotateButtonPressed -= RotateTetromino;
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
        }
        private void SnapParent()
        {
            transform.position = gridSystem.GridToWorldCenter(
                gridSystem.WorldToGrid(transform.position).x,
                gridSystem.WorldToGrid(transform.position).y
            );
        }

        private void RotateTetromino()
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
