using System;
using UnityEngine;

namespace CardMatch.Events
{
    public class BaseEvent<T> : ScriptableObject
    {
        private event Action<T> Event;

        public void Subscribe(Action<T> action)
        {
            Event += action;
        }
        
        public void Unsubscribe(Action<T> action)
        {
            Event -= action;
        }

        public void Raise(T data)
        {
            Event?.Invoke(data);
        }
    }
}