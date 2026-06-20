using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.SOAP
{
    public abstract class VariableBase<T> : RuntimeScriptableObject
    {
        [SerializeField] protected T initialValue;
        [SerializeField] protected T value;
        protected event UnityAction OnValueChanged = delegate { };

        public T Value
        {
            get => value;
            set
            {
                if(Equals(this.value, value)) return;
                this.value = value;
                OnValueChanged.Invoke();
                
            }
        }
    }
}