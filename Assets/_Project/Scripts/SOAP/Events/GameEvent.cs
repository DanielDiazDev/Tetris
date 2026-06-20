using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.SOAP.Events
{
    public class GameEvent<T> : ScriptableObject
    {
        private readonly List<Action<T>> listeners = new();

        public void Raise(T data)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i]?.Invoke(data);
            }
        }

        public void Subscribe(Action<T> callback)
        {
            if (!listeners.Contains(callback))
                listeners.Add(callback);
        }

        public void Unsubscribe(Action<T> callback)
        {
            listeners.Remove(callback);
        }
    }
}