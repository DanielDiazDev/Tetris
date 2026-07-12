using System;
using _Project.Scripts.Utils;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Scripts.Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }
        private PlayerInput playerInput;
        public event Action<Vector2, float> OnStartTouchEvent;
        public event Action<Vector2,  float> OnEndTouchEvent;
        public event Action<Vector2> OnTouchHeldEvent;
        public event Action OnTouch;
        private bool isTouching;
        private new Camera camera;
        private InputAction primaryContactInput;
        private InputAction primaryPosition;
        private InputAction touchInput;
        private void OnEnable()
        {
            
            playerInput.actions.Enable();
            primaryContactInput.started += StartTouchPrimary;
            primaryContactInput.canceled += EndTouchPrimary;
            primaryPosition.performed += TouchHeldPrimary;
            touchInput.started += StartTouch;
        }

       

        private void OnDisable()
        {
            playerInput.actions.Disable();
            primaryContactInput.started -= StartTouchPrimary;
            primaryContactInput.canceled -= EndTouchPrimary;
            primaryPosition.performed -= TouchHeldPrimary;
            touchInput.started -= StartTouch;
           
        }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            camera = Camera.main;
            playerInput = GetComponent<PlayerInput>();
            primaryContactInput = playerInput.actions["PrimaryContact"];
            primaryPosition = playerInput.actions["PrimaryPosition"];
            touchInput = playerInput.actions["Touch"];
        }
         private void StartTouch(InputAction.CallbackContext ctx)
        {
            OnTouch?.Invoke();
        }

        private void TouchHeldPrimary(InputAction.CallbackContext ctx)
        {
            if(!isTouching) return;
            OnTouchHeldEvent?.Invoke(
                ScreenUtils.ScreenToWorld(camera, primaryPosition.ReadValue<Vector2>())
            );
        }
        private void StartTouchPrimary(InputAction.CallbackContext ctx)
        {
            isTouching = true;
            OnStartTouchEvent?.Invoke(ScreenUtils.ScreenToWorld(camera, 
                primaryPosition.ReadValue<Vector2>()), (float)ctx.startTime);
        }
        private void EndTouchPrimary(InputAction.CallbackContext ctx)
        {
            isTouching = false;
            OnEndTouchEvent?.Invoke(ScreenUtils.ScreenToWorld(camera, 
                primaryPosition.ReadValue<Vector2>()), (float)ctx.time);
        }

        public Vector2 PrimaryPosition()
        {
            return ScreenUtils.ScreenToWorld(camera, primaryPosition.ReadValue<Vector2>());
        }
       
    }
}