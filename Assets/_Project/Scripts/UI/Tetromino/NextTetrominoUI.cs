using System;
using _Project.Scripts.SOAP.Events;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Tetromino
{
    public class NextTetrominoUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private SpriteEvent nextSpriteEvent;

        private void OnEnable()
        {
            nextSpriteEvent.Subscribe(UpdateNextSprite);
        }

        private void OnDestroy()
        {
            nextSpriteEvent.Unsubscribe(UpdateNextSprite);
        }

        private void UpdateNextSprite(Sprite sprite)
        {
            icon.sprite = sprite;
        }
    }
}