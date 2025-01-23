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

		private Transform _disableParent;

		public PoolObjects(T prefab, Transform disableParent = null)
		{
			_prefab = prefab;

			_disableParent = disableParent == null
				? new GameObject { name = $"{prefab.name} pool objects" }.transform
				: disableParent;
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

		public T Get(Transform parent)
		{
			T poolObject = Get();
			poolObject.transform.SetParent(parent);
			
			return poolObject;
		}
		
		public void Return(T returnObject)
		{
			if (Instantiated.Remove(returnObject))
			{
				returnObject.gameObject.SetActive(false);
				returnObject.transform.SetParent(_disableParent);
				FreeObjects.AddLast(returnObject);
			}
			else
				throw new ArgumentException("Returning a non-existent object to the pool", returnObject.name);
		}
		
		private T Create()
		{
			var instantiatedObject = Object.Instantiate(_prefab, _disableParent);
			instantiatedObject.name = _prefab.name;
			
			return instantiatedObject;
		}
	}
	
}