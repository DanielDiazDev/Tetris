using System;
using _Project.Scripts.Prefs;
using _Project.Scripts.SOAP.Events;
using UnityEngine;

namespace Project.Scripts.Input
{
    public class SwipeDirection : MonoBehaviour
    {
        [SerializeField] private float minimumDistance = .2f;
        //[SerializeField] private float maximumTime = 1f;
      //  [SerializeField, Range(0, 1)] private float directionThreshold = .9f;
        [SerializeField] private Vector3Event swipeEvent;
        [SerializeField] private BoolEvent softDroppedEvent;
        [SerializeField] private VoidEvent rotationEvent;
        private float lastMoveTime;
        private float moveCooldown => PlayerPrefsManager.GetFloat("MoveCooldown");
        private Vector2 startPosition;
        private float startTime;
        private Vector2 endPosition;
        private float endTime;
        private bool isHolding;

        [SerializeField] private float tapThreshold = 0.2f;
        private void OnEnable()
        {
            InputManager.Instance.OnStartTouchEvent += SwipeStart;
            InputManager.Instance.OnEndTouchEvent += SwipeEnd;
            InputManager.Instance.OnTouchHeldEvent += HandleDrag;
        }
        private void OnDisable()
        {
            InputManager.Instance.OnStartTouchEvent -= SwipeStart;
            InputManager.Instance.OnEndTouchEvent -= SwipeEnd;
            InputManager.Instance.OnTouchHeldEvent -=  HandleDrag;
        }
        
        private void HandleDrag(Vector2 position)
        {
            if (!isHolding) return;

            if (Time.time - lastMoveTime < moveCooldown) return;

            float deltaX = position.x - startPosition.x;
            float deltaY = position.y - startPosition.y;

            if (Mathf.Abs(deltaX) > minimumDistance)
            {
                Vector3 direction = deltaX > 0 ? Vector3.right : Vector3.left;
                swipeEvent.Raise(direction);
                startPosition = position;
                lastMoveTime = Time.time; 
            }
            else if (deltaY < -minimumDistance)
            {
                softDroppedEvent.Raise(true);
                lastMoveTime = Time.time;
            }
        }
        private void SwipeStart(Vector2 position, float time)
        {
            startPosition = position;
            startTime = time;
            isHolding = true;
        }
        private void SwipeEnd(Vector2 position, float time)
        {
            isHolding = false;
            softDroppedEvent.Raise(false);

            float pressDuration = time - startTime;
            if (pressDuration < tapThreshold)
            {
                rotationEvent.Raise();
            }
        }

        // private void DetectSwipe()
        // {
        //     if (Vector3.Distance(startPosition, endPosition) >= minimumDistance
        //         && (endTime - startTime) <= maximumTime)
        //     {
        //         Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
        //         Vector2 direction = (endPosition - startPosition).normalized;
        //
        //         SwipeDetection(direction);
        //     }
        // }
        //
        // private void SwipeDetection(Vector2 direction)
        // {
        //     if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        //     {
        //         if (direction.x > 0)
        //             Debug.Log("Swipe Right");
        //         else
        //             Debug.Log("Swipe Left");
        //     }
        //     else
        //     {
        //         if (direction.y > 0)
        //             Debug.Log("Swipe Up");
        //         else
        //             Debug.Log("Swipe Down");
        //     }
        //
        //     swipeEvent.Raise(direction);
        // }
    }
}