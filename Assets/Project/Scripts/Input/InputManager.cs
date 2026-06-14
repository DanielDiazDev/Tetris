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
            DragDelta = Vector2.zero;
        }

        private void OnRelease(InputAction.CallbackContext ctx)
        {
            IsDragging = false;
            DragDelta = Vector2.zero;
        }

        private void OnPointerMove(InputAction.CallbackContext ctx)
        {
            if (!IsDragging) return;
            DragDelta = ctx.ReadValue<Vector2>();
        }
    }
}