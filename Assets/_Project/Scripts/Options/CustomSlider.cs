using _Project.Scripts.Prefs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Options
{
    public class CustomSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        [SerializeField] private float minReal = 0.001f;
        [SerializeField] private float maxReal = 0.1f;
        [SerializeField] private string sliderId;

        private void Start()
        {
            slider.minValue = 0;
            slider.maxValue = 1;
            float savedReal = PlayerPrefsManager.GetFloat(sliderId, (minReal + maxReal) * 0.5f);
            float normalized = Mathf.InverseLerp(minReal, maxReal, savedReal);
            slider.value = normalized;
            slider.onValueChanged.AddListener(OnSliderChanged);
            OnSliderChanged(slider.value);
        }

        private void OnSliderChanged(float normalizedValue)
        {
            float realValue = Mathf.Lerp(minReal, maxReal, normalizedValue);

            PlayerPrefsManager.SetFloat(sliderId, realValue);
        }
        
        
    }
}