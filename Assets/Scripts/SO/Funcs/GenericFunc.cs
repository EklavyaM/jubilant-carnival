using System;
using UnityEngine;

namespace CardMatch.SO.Funcs
{
    public abstract class GenericFunc<T> : ScriptableObject
    {
        private event Func<T> Func;

        public void Subscribe(Func<T> func)
        {
            Func += func;
        }

        public void Unsubscribe(Func<T> func)
        {
            Func -= func;
        }

        public T Request()
        {
            return Func == null ? default(T) : Func();
        }
    }
}