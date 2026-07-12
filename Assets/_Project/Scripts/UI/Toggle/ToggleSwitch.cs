using System;
using System.Collections;
using _Project.Scripts.Prefs;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project.Scripts.UI.Toggle
{
    public class ToggleSwitch : MonoBehaviour, IPointerClickHandler
    {
        [Header("Slider setup")] 
        [SerializeField, Range(0, 1f)]
        protected float sliderValue;
        public bool CurrentValue { get; private set; }
        [SerializeField] private bool isSfx;
        
        private bool previousValue;
        private Slider slider;

        [Header("Animation")] 
        [SerializeField, Range(0, 1f)] private float animationDuration = 0.5f;
        [SerializeField] private AnimationCurve slideEase =
            AnimationCurve.EaseInOut(0, 0, 1, 1);

        private Coroutine _animateSliderCoroutine;

        [Header("Events")] 
        [SerializeField] private UnityEvent onToggleOn;
        [SerializeField] private UnityEvent onToggleOff;

        private ToggleSwitchGroupManager toggleSwitchGroupManager;
        
        protected Action transitionEffect;
        
        protected virtual void OnValidate()
        {
            SetupToggleComponents();

            slider.value = sliderValue;
        }

        private void SetupToggleComponents()
        {
            if (slider != null)
                return;

            SetupSliderComponent();
        }

        private void SetupSliderComponent()
        {
            slider = GetComponent<Slider>();

            if (slider == null)
            {
                Debug.Log("No slider found!", this);
                return;
            }

            slider.interactable = false;
            var sliderColors = slider.colors;
            sliderColors.disabledColor = Color.white;
            slider.colors = sliderColors;
            slider.transition = Selectable.Transition.None;
        }
        
        public void SetupForManager(ToggleSwitchGroupManager manager)
        {
            toggleSwitchGroupManager = manager;
        }


        protected virtual void Awake()
        {
            SetupSliderComponent();
            bool savedState;
            if (isSfx)
            {
                savedState = PlayerPrefsManager.GetBool("SfxToggle");
            }
            else
            {
                savedState = PlayerPrefsManager.GetBool("MusicToggle");
            }
            SetStateAndStartAnimation(savedState);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Toggle();
        }

        
        private void Toggle()
        {
            if (toggleSwitchGroupManager != null)
                toggleSwitchGroupManager.ToggleGroup(this);
            else
                SetStateAndStartAnimation(!CurrentValue);
        }

        public void ToggleByGroupManager(bool valueToSetTo)
        {
            SetStateAndStartAnimation(valueToSetTo);
        }
        
        
        private void SetStateAndStartAnimation(bool state)
        {
            previousValue = CurrentValue;
            CurrentValue = state;

            if (previousValue != CurrentValue)
            {
                if (CurrentValue)
                    onToggleOn?.Invoke();
                else
                    onToggleOff?.Invoke();
            }

            if (_animateSliderCoroutine != null)
                StopCoroutine(_animateSliderCoroutine);

            _animateSliderCoroutine = StartCoroutine(AnimateSlider());
        }


        private IEnumerator AnimateSlider()
        {
            float startValue = slider.value;
            float endValue = CurrentValue ? 1 : 0;

            float time = 0;
            if (animationDuration > 0)
            {
                while (time < animationDuration)
                {
                    time += Time.deltaTime;

                    float lerpFactor = slideEase.Evaluate(time / animationDuration);
                    slider.value = sliderValue = Mathf.Lerp(startValue, endValue, lerpFactor);

                    transitionEffect?.Invoke();
                        
                    yield return null;
                }
            }

            slider.value = endValue;
        }
    }
}
