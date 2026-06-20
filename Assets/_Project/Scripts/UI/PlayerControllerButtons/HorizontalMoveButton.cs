using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts.UI.PlayerControllerButtons
{
    public class HorizontalMoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private enum DirectionType
        {
            Left,
            Right
        }

        [SerializeField] private DirectionType directionType;
        private Vector3 direction; 
        private bool isPressed;
        private float repeatRate = 0.1f; 
        private float lastMoveTime;
        public event Action<Vector3> OnHorizontalMoveButtonPressed;

        private void Awake()
        {
            direction = directionType == DirectionType.Left ? Vector3.left : Vector3.right;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isPressed = true;
            DoMove(); 
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPressed = false;
        }

        private void Update()
        {
            if (isPressed && Time.time - lastMoveTime > repeatRate)
            {
                DoMove();
            }
        }
        private void DoMove()
        {
            OnHorizontalMoveButtonPressed?.Invoke(direction);
            lastMoveTime = Time.time;
        }
    }
}