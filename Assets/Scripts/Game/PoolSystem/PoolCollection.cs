using System.Collections.Generic;
using UnityEngine;

namespace Game.PoolSystem
{
    public class PoolCollection<T> where T : Component
    {
        private readonly Dictionary<string, PoolObjects<T>> _pools = new();

        public T Get(T prefab)
        {
            string name = prefab.name;
            return !_pools.ContainsKey(name) ? CreatePool(prefab, name).Get() : _pools[name].Get();
        }

        public T Get(T prefab, Vector2 position, float rotation, Transform parent)
        {
            T instance = Get(prefab);
            instance.transform.position = position;
            instance.transform.eulerAngles = new Vector3(0f, 0f, rotation);
            instance.transform.SetParent(parent);
            instance.transform.localScale = Vector3.one;

            return instance;
        }
        
        public void Return(T poolObject)
        {
            _pools[poolObject.name].Return(poolObject);
        }

        private PoolObjects<T> CreatePool(T prefab, string name)
        {
            PoolObjects<T> poolObjects = new PoolObjects<T>(prefab);
            _pools.Add(name, poolObjects);
            
            return poolObjects;
        }
    }
}