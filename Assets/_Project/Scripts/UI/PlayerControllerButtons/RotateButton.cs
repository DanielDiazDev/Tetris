using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI.PlayerControllerButtons
{
    public class RotateButton : MonoBehaviour
    {
        public event Action OnRotateButtonPressed;
        [SerializeField] private Button rotateButton;

        private void Awake()
        {
            rotateButton.onClick.AddListener(() => OnRotateButtonPressed?.Invoke());
        }
    }
}