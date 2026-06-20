using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts.UI.PlayerControllerButtons
{
    public class SoftDropButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Action<bool> OnSoftDropButtonPressed; 

        public void OnPointerDown(PointerEventData eventData)
        {
            OnSoftDropButtonPressed?.Invoke(true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnSoftDropButtonPressed?.Invoke(false);
        }
    }
}