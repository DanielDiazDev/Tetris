using System;
using UnityEngine;

namespace _Project.Scripts.SOAP
{
    public abstract class Variable<T> : RuntimeScriptableObject
    {
        [SerializeField] private T initialValue;
        [SerializeField] private T value;

        public event Action<T> OnValueChanged = delegate { };

        public virtual T Value
        {
            get => value;
            set
            {
                if (Equals(this.value, value)) return;

                this.value = value;
                OnValueChanged.Invoke(this.value);
            }
        }

        protected override void OnReset()
        {
            value = initialValue;
            OnValueChanged.Invoke(value);
        }
    }
}