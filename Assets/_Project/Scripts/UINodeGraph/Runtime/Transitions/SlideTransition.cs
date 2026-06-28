using System;
using System.Collections;
using UnityEngine;

namespace _Project.Scripts.UINodeGraph.Runtime.Transitions
{
    [Serializable]
    public class SlideTransition : UIFlowTransition
    {
        public Vector2 offset = new Vector2(1, 0);
        public override IEnumerator DoTransition(CanvasGroup from, CanvasGroup to, float duration)
        {
            if (to) {
                to.gameObject.SetActive(true);
                to.alpha = 1;
                RectTransform rt = to.GetComponent<RectTransform>();
                
                Vector2 startPos = new Vector2(Screen.width * offset.x, Screen.height * offset.y);
                rt.anchoredPosition = startPos;

                float time = 0;
                while (time < duration) {
                    rt.anchoredPosition = Vector2.Lerp(startPos, Vector2.zero, time / duration);
                    time += Time.unscaledDeltaTime;
                    yield return null;
                }
                rt.anchoredPosition = Vector2.zero;
            }
        
            if (from) {
                from.alpha = 0;
                from.gameObject.SetActive(false);
            }
        }
    }
}