using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.SOAP
{
    public abstract class RuntimeScriptableObject : DescriptionSO
    {
        static readonly List<RuntimeScriptableObject> Instances = new List<RuntimeScriptableObject>();

        private void OnEnable()
        {
            Instances.Add(this);
        }

        private void OnDisable()
        {
            Instances.Remove(this);
        }

        protected abstract void OnReset();
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ResetAllInstances()
        {
            foreach (var instance in Instances)
            {
                instance.OnReset();
            }
        }
    }
}