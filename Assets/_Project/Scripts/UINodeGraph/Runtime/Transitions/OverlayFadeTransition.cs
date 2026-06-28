using System;
using System.Collections;
using UnityEngine;

namespace _Project.Scripts.UINodeGraph.Runtime.Transitions
{
    [Serializable]
    public class OverlayFadeTransition : UIFlowTransition
    {
        public override IEnumerator DoTransition(CanvasGroup from, CanvasGroup to, float duration)
        {
            if (to) { 
                to.gameObject.SetActive(true); 
                to.alpha = 0; 
            }

            float time = 0;
            while (time < duration) {
                float t = time / duration;
                if (to) to.alpha = t; 
                time += Time.unscaledDeltaTime;
                yield return null;
            }

            if (to) to.alpha = 1;
        }
    }
}