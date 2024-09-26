using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.PoolSystem
{
	public class PoolObjects<T> where T : Component
	{
		private readonly T _prefab;
		
		private readonly LinkedList<T> Instantiated = new();
		private readonly LinkedList<T> FreeObjects = new();

		private Transform _parent;

		public PoolObjects(T prefab)
		{
			_prefab = prefab;
			
			var parent = new GameObject {name = $"{_prefab.name} pool objects"};
			_parent = parent.transform;
		}

		public T Get()
		{
			T poolObject = FreeObjects.FirstOrDefault();

			if (poolObject is null)
				poolObject = Create();
			else 
				FreeObjects.Remove(poolObject);
			
			poolObject.gameObject.SetActive(true);
			poolObject.transform.SetParent(null);
			
			Instantiated.AddLast(poolObject);

			return poolObject;
		}

		public void Return(T returnObject)
		{
			if (Instantiated.Remove(returnObject))
			{
				returnObject.gameObject.SetActive(false);
				returnObject.transform.SetParent(_parent);
				FreeObjects.AddLast(returnObject);
			}
			else
				throw new ArgumentException("Returning a non-existent object to the pool", returnObject.name);
		}
		
		private T Create()
		{
			var instantiatedObject = Object.Instantiate(_prefab);
			instantiatedObject.name = _prefab.name;
			
			return instantiatedObject;
		}
	}
	
}