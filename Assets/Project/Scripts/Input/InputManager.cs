using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Scripts.Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        public Vector2 DragDelta { get; private set; }
        public bool IsDragging { get; private set; }
        public event Action OnTap;
        private Vector2 lastPointerPos;
        private void OnEnable()
        {
            var playerInput = GetComponent<PlayerInput>();
            playerInput.actions["PointerPress"].performed += OnPress;
            playerInput.actions["PointerPress"].canceled += OnRelease;
            playerInput.actions["PointerPosition"].performed += OnPointerMove;
        }

        private void OnDisable()
        {
            var playerInput = GetComponent<PlayerInput>();
            playerInput.actions["PointerPress"].performed -= OnPress;
            playerInput.actions["PointerPress"].canceled -= OnRelease;
            playerInput.actions["PointerPosition"].performed -= OnPointerMove;
        }
        private void Awake()
        {
            Instance = this;
        }
        private void OnPress(InputAction.CallbackContext ctx)
        {
            IsDragging = true;
           // lastPointerPos = ctx.ReadValue<Vector2>();
            DragDelta = Vector2.zero;
        }

        private void OnRelease(InputAction.CallbackContext ctx)
        {
            if (DragDelta == Vector2.zero)
            {
                OnTap?.Invoke();
            }
            IsDragging = false;
            DragDelta = Vector2.zero;
        }

        private void OnPointerMove(InputAction.CallbackContext ctx)
        {
            if (!IsDragging) return;
            Vector2 currentPos = ctx.ReadValue<Vector2>();
            DragDelta = currentPos - lastPointerPos; 
            lastPointerPos = currentPos;
        }

        public void DragToZero()
        {
            DragDelta = Vector2.zero;
        }
    }
}