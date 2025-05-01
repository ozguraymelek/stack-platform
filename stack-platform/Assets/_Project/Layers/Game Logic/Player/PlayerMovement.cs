using System;
using _Project.Layers.Data.Entities;
using _Project.Layers.Data.Interfaces.Player;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Inject] private PlayerEntity _playerEntity;
        [Inject] private IMovementProvider _movementProvider;
        
        private void Update()
        {
            if (_playerEntity.IsMovementEnable == false) return;
            var speed = _playerEntity.Speed;
            var delta = _movementProvider.Current.GetDelta(speed) * Time.deltaTime;
            transform.position += delta;
            _playerEntity.Position = transform.position;
        }
        
        public void EnableMovement()
        {
            _playerEntity.IsMovementEnable = true;
        }

        public void DisableMovement()
        {
            _playerEntity.IsMovementEnable = false;
        }
    }
}
