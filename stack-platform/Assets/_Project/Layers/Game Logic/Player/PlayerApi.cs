using System;
using System.Collections;
using _Project.Helper.ZEnject;
using _Project.Layers.Data.Entities;
using _Project.Layers.Game_Logic.Camera;
using _Project.Layers.Game_Logic.Signals;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Player
{
    public class PlayerApi : MonoBehaviour
    {
        private SignalBus _signalBus;
        private PureCoroutine _pureCoroutine;
        
        [Inject] private PlayerEntity _playerEntity;
        public APIComponents Api;

        [Inject]
        public void Construct(SignalBus signalBus, PureCoroutine pureCoroutine)
        {
            _signalBus = signalBus;
            _pureCoroutine = pureCoroutine;
        }
        
        private void OnEnable()
        {
            _signalBus.Subscribe<GameStartedSignal>(OnGameStarted);
            _signalBus.Subscribe<LevelStartedSignal>(OnLevelStarted);
            _signalBus.Subscribe<LevelFinishedSignal>(OnLevelFinished);
        }
        private void OnDisable()
        {
            _signalBus.Unsubscribe<GameStartedSignal>(OnGameStarted);
            _signalBus.Unsubscribe<LevelStartedSignal>(OnLevelStarted);
            _signalBus.Unsubscribe<LevelFinishedSignal>(OnLevelFinished);
        }
        
        private void OnGameStarted()
        {
            Debug.Log("Game Started signal received!");
            // Buraya istediğin işlemi yazarsın
            Api.Rigidbody.useGravity = true;
            Api.Animator.SetBool("Start", true);
        }
        
        private void OnLevelStarted()
        {
            _pureCoroutine.RunPureCoroutine(DelayedOnLevel(.5f, false));
            // Api.Animator.SetBool("Dance", false);
        }
        
        private void OnLevelFinished()
        {
            _pureCoroutine.RunPureCoroutine(DelayedOnLevel(.5f, true));
        }

        private IEnumerator DelayedOnLevel(float seconds, bool condition)
        {
            yield return new WaitForSeconds(seconds);
            Api.Animator.SetBool("Dance", condition);
        }
    }
}
