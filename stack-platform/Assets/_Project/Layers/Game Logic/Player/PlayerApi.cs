using System;
using _Project.Layers.Data.Entities;
using _Project.Layers.Game_Logic.Signals;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Player
{
    public class PlayerApi : MonoBehaviour
    {
        private SignalBus _signalBus;
        
        [Inject] private PlayerEntity _playerEntity;
        public APIComponents Api;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private void OnEnable()
        {
            _signalBus.Subscribe<GameStartedSignal>(OnGameStarted);
        }
        private void OnDisable()
        {
            _signalBus.Unsubscribe<GameStartedSignal>(OnGameStarted);
        }
        
        private void OnGameStarted()
        {
            Debug.Log("Game Started signal received!");
            // Buraya istediğin işlemi yazarsın
            Api.Rigidbody.useGravity = true;
            Api.Animator.SetBool("Start", true);
        }
    }
}
