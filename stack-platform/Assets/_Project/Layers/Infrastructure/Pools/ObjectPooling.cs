using System.Collections.Generic;
using _Project.Layers.Game_Logic.Platform;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Infrastructure.Pools
{
    public class ObjectPooling : IInitializable, IObjectPool
    {
        private readonly DiContainer _container;
        private readonly Platform _prefab;
        private readonly Transform _root;
        private readonly int _initialPoolSize;
        private readonly Queue<IInteractable<Platform>> _pool = new Queue<IInteractable<Platform>>();

        public ObjectPooling(DiContainer container, 
            [Inject(Id = "Platform")]Platform prefab, 
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

        public IInteractable<Platform> Get()
        {
            var item = _pool.Count > 0
                ? _pool.Dequeue()
                : _container.InstantiatePrefab(_prefab.GetTransform().gameObject, _root)
                    .GetComponent<IInteractable<Platform>>();
            
            item.GetTransform().gameObject.SetActive(true);
            return item;
        }

        public void Release(IInteractable<Platform> item)
        {
            item.GetTransform().gameObject.SetActive(false);
            item.GetTransform().transform.SetParent(_root, false);
            _pool.Enqueue(item);
        }
        
        private void Preload(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var item = _container.InstantiatePrefab(_prefab.GetTransform().gameObject, _root)
                    .GetComponent<IInteractable<Platform>>();
                item.GetTransform().gameObject.SetActive(false);
                _pool.Enqueue(item);
            }
        }

    }
}