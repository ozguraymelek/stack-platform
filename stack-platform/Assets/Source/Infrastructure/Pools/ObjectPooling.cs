using System.Collections.Generic;
using Source.Core.Utilities.External;
using Source.Gameplay.Platform;
using Source.Gameplay.Platform.Wrappers;
using UnityEngine;
using Zenject;

namespace Source.Infrastructure.Pools
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

            SLog.InjectionStatus(this,
                (nameof(_container), _container),
                (nameof(_prefab), _prefab),
                (nameof(_root), _root),
                (nameof(_initialPoolSize), _initialPoolSize)
            );
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
            if (item.GetTransform().gameObject.layer == LayerMask.NameToLayer("Initial"))
                return;
            
            if (item.GetTransform().gameObject.layer == LayerMask.NameToLayer("Finish"))
                return;
            
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