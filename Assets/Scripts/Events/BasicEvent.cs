using System;
using UnityEngine;

namespace CardMatch.Events
{
    public abstract class BasicEvent: ScriptableObject
    {
        private event Action Event;

        public void Subscribe(Action action)
        {
            Event += action;
        }
        
        public void Unsubscribe(Action action)
        {
            Event -= action;
        }

        public void Raise()
        {
            Event?.Invoke();
        }
    }
}