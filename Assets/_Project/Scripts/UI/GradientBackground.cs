using System;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class GradientBackground : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Color topColor = new Color32(15, 23, 42, 255);
        [SerializeField] private Color bottomColor = new Color32(2, 6, 23, 255);
   
        void Start()
        {
            SetBackgroundColor();
        }

        private void Update()
        {
            SetBackgroundColor();
        }

        private void SetBackgroundColor()
        {
            Texture2D texture = new Texture2D(1, 256);
   
            //  Color topColor = new Color32(15, 23, 42, 255);   // Azul oscuro
            //Color bottomColor = new Color32(2, 6, 23, 255);  // Casi negro
   
            for (int y = 0; y < 256; y++)
            {
                float t = y / 255f;
                texture.SetPixel(0, y, Color.Lerp(bottomColor, topColor, t));
            }
   
            texture.Apply();
   
            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
   
            background.sprite = sprite;
        }
    }
}