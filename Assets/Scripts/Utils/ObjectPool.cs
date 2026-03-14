using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CardMatch.Utils
{
    public class ObjectPool<T> where T : Component
    {
        private readonly GameObject _prefab;
        private readonly GameObject _poolContainer;
        private readonly Queue<T> _pool = new Queue<T>();

        public ObjectPool(GameObject prefab)
        {
            _prefab = prefab;

            _poolContainer = new GameObject(prefab.name + "_Pool");
            _poolContainer.SetActive(false);
        }

        public void Generate(uint count)
        {
            for (int i = 0; i < count; ++i)
            {
                GameObject @object = Object.Instantiate(_prefab, _poolContainer.transform);
                _pool.Enqueue( @object.GetComponent<T>());
            }
        }

        public bool TryGet(out T component)
        {
            return _pool.TryDequeue(out component);
        }

        public void Release(T component)
        {
            if (component == null)
            {
                Debug.LogWarning("Component is null");
                return;
            }
            
            component.transform.SetParent(_poolContainer.transform);
            _pool.Enqueue(component);
        }

        public void Clear()
        {
            while (_pool.Count > 0)
            {
                T obj = _pool.Dequeue();
                if (obj != null)
                    Object.Destroy(obj.gameObject);
            }
        }
    }
}