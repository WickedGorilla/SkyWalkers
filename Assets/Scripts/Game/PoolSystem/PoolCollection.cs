using System.Collections.Generic;
using UnityEngine;

namespace Game.PoolSystem
{
    public class PoolCollection<T> where T : Component
    {
        private readonly Transform _parent;
        private readonly Dictionary<string, PoolObjects<T>> _pools;

        public PoolCollection(Transform parent = null)
        {
            _parent = parent;
            _pools = new Dictionary<string, PoolObjects<T>>();
        }
        
        public T Get(T prefab)
        {
            string name = prefab.name;
            return !_pools.TryGetValue(name, out var pool) ? CreatePool(prefab, name).Get() : pool.Get();
        }

        public T Get(T prefab, Vector2 position, float rotation)
        {
            T instance = Get(prefab);
            instance.transform.position = position;
            instance.transform.eulerAngles = new Vector3(0f, 0f, rotation);
            return instance;
        }
        
        public void Return(T poolObject)
        {
            _pools[poolObject.name].Return(poolObject);
        }

        private PoolObjects<T> CreatePool(T prefab, string name)
        {
            PoolObjects<T> poolObjects = new PoolObjects<T>(prefab, _parent);
            _pools.Add(name, poolObjects);
            
            return poolObjects;
        }
    }
}