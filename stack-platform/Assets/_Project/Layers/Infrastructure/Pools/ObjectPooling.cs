using System.Collections.Generic;
using _Project.Layers.Game_Logic.Platform;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Infrastructure.Pools
{
    public class ObjectPooling : IInitializable
    {
        private readonly DiContainer _container;
        private readonly GameObject _prefab;
        private readonly Transform _root;
        private readonly int _initialPoolSize;
        private readonly Queue<GameObject> _pool = new Queue<GameObject>();

        public ObjectPooling(DiContainer container, 
            [Inject(Id = "Platform")]GameObject prefab, 
            [Inject(Id = "Root")]Transform root,
            [Inject(Id = "InitialPoolSize")] int initialPoolSize)
        {
            _container = container;
            _prefab = prefab;
            _root = root;
            _initialPoolSize = initialPoolSize;
        }
        
        public void Initialize()
        {
            Preload(_initialPoolSize);
        }

        public GameObject GetFromPool()
        {
            GameObject obj;
            obj = _pool.Count > 0 
                ? _pool.Dequeue() 
                : _container.InstantiatePrefab(_prefab, _root);
            
            obj.SetActive(true);
            return obj;
        }
        
        public void ReturnToPool(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(_root, false);
            _pool.Enqueue(obj);
        }
        
        private void Preload(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var obj = _container.InstantiatePrefab(_prefab, _root);
                obj.SetActive(false);
                _pool.Enqueue(obj);
            }
        }
    }
}