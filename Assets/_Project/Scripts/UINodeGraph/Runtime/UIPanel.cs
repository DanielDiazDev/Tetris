using System;
using System.Collections;
using UnityEngine;

namespace _Project.Scripts.UINodeGraph.Runtime
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : MonoBehaviour
    {
        [Header("Graph Identity")]
        [HideInInspector] public string nodeGuid;
        
        [Header("Focus Settings")]
        private CanvasGroup canvasGroup;
        private Coroutine currentFade;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Show(float duration = 0)
        {
            gameObject.SetActive(true);
            if(currentFade != null) StopCoroutine(currentFade);
            if (duration > 0)
            {
                currentFade = StartCoroutine(FadeDuration(1, duration));
            }
            else
            {
                canvasGroup.alpha = 1;
            }
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        public void Hide(float duration = 0)
        {
            if(currentFade != null) StopCoroutine(currentFade);
            if (duration > 0 && gameObject.activeInHierarchy)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                currentFade = StartCoroutine(FadeDuration(0, duration, () => gameObject.SetActive(false)));
            }
            else
            {
                canvasGroup.alpha = 0;
                gameObject.SetActive(false);
            }
        }

        private IEnumerator FadeDuration(float targetAlpha, float duration, Action onComplete = null)
        {
            float startAlpha = canvasGroup.alpha;
            float time = 0;

            while (time < duration) 
            {
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;
            onComplete?.Invoke();
            currentFade = null;
        }
    }
}