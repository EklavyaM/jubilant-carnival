using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardMatch.Utils
{
    public abstract class Switcher<T> : MonoBehaviour where T : Enum
    {
        [System.Serializable]
        public class Data
        {
            public T type;
            public GameObject @object;
        }

        [SerializeField] private List<Data> objects;
        
        public event Action<T> OnSwitch; 

        public void Switch(T type)
        {
            foreach (Data data in objects)
                data.@object.SetActive(data.type.Equals(type));
            
            OnSwitch?.Invoke(type);
        }
    }
}