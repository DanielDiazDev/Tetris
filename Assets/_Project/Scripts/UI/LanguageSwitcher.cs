using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class LanguageSwitcher : MonoBehaviour
    {
        [SerializeField] private Button englishButton;
        [SerializeField] private Button spanishButton;
        [SerializeField] private Color selectedColor = Color.cyan;

        private Color unselectedColor;

        private void Awake()
        {
            if (englishButton != null && englishButton.targetGraphic != null)
                unselectedColor = englishButton.targetGraphic.color;

            // Escuchar cambios de idioma para actualizar visuales si ocurren desde otro lado
            if (LocalizationManager.Instance != null)
                LocalizationManager.Instance.OnLanguageChanged += OnLanguageChanged;
        }

        private void OnDestroy()
        {
            if (LocalizationManager.Instance != null)
                LocalizationManager.Instance.OnLanguageChanged -= OnLanguageChanged;
        }

        private void Start()
        {
            UpdateButtonVisuals();
        }

        private void OnLanguageChanged(string localeCode)
        {
            UpdateButtonVisuals();
        }

        public void SetEnglish()
        {
            LocalizationManager.Instance?.SetLanguage(LocalizationManager.Instance.EnglishLocale);
        }

        public void SetSpanish()
        {
            LocalizationManager.Instance?.SetLanguage(LocalizationManager.Instance.SpanishLocale);
        }

        private void UpdateButtonVisuals()
        {
            if (LocalizationManager.Instance == null) return;

            string current = LocalizationManager.Instance.CurrentLocaleCode;

            bool englishIsSelected = current == LocalizationManager.Instance.EnglishLocale;
            bool spanishIsSelected = current == LocalizationManager.Instance.SpanishLocale;

            ApplyButtonStyle(englishButton, englishIsSelected);
            ApplyButtonStyle(spanishButton, spanishIsSelected);
        }

        private void ApplyButtonStyle(Button button, bool isSelected)
        {
            if (button == null || button.targetGraphic == null) return;
            button.targetGraphic.color = isSelected ? selectedColor : unselectedColor;
        }
    }
}
