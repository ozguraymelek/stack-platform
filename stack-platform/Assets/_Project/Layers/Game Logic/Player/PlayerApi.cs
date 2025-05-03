using System;
using System.Collections;
using _Project.Helper.Utils;
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
        private static readonly int Dance = Animator.StringToHash("Dance");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int Start = Animator.StringToHash("Start");

        private SignalBus _signalBus;
        private PureCoroutine _pureCoroutine;
        
        public APIComponents Api;

        [Inject]
        public void Construct(SignalBus signalBus, PureCoroutine pureCoroutine)
        {
            _signalBus = signalBus;
            _pureCoroutine = pureCoroutine;

            SLog.InjectionStatus(this,
                (nameof(_signalBus), _signalBus),
                (nameof(_pureCoroutine), _pureCoroutine)
            );
        }
        
        private void OnEnable()
        {
            _signalBus.Subscribe<GameStartedSignal>(OnGameStarted);
            _signalBus.Subscribe<LevelStartedSignal>(OnLevelStarted);
            _signalBus.Subscribe<LevelFinishedSignal>(OnLevelFinished);
            _signalBus.Subscribe<GameFailedSignal>(OnGameFailed);
            
            _signalBus.Subscribe<PhysicToggleSignal>(EnableTriggerAndDisableGravity);
        }
        private void OnDisable()
        {
            _signalBus.Unsubscribe<GameStartedSignal>(OnGameStarted);
            _signalBus.Unsubscribe<LevelStartedSignal>(OnLevelStarted);
            _signalBus.Unsubscribe<LevelFinishedSignal>(OnLevelFinished);
            _signalBus.Unsubscribe<GameFailedSignal>(OnGameFailed);
            
            _signalBus.Unsubscribe<PhysicToggleSignal>(EnableTriggerAndDisableGravity);
        }
        
        private void OnGameStarted()
        {
            Debug.Log("Game Started signal received!");
            Api.Rigidbody.useGravity = true;
            Api.Animator.SetBool(Start, true);
        }
        
        private void OnLevelStarted()
        {
            _pureCoroutine.RunPureCoroutine(DelayedOnLevel(.5f, false));
            
            EnableTriggerAndDisableGravity();
        }
        
        private void OnLevelFinished()
        {
            _pureCoroutine.RunPureCoroutine(DelayedOnLevel(.5f, true));
        }

        private IEnumerator DelayedOnLevel(float seconds, bool condition)
        {
            yield return new WaitForSeconds(seconds);
            Api.Animator.SetBool(Dance, condition);
        }
        
        private void OnGameFailed()
        {
            Api.Rigidbody.useGravity = true;
            Api.Collider.isTrigger = false;
            Api.Animator.SetTrigger(Fall);
        }

        private void EnableTriggerAndDisableGravity()
        {
            Api.Rigidbody.useGravity = false;
            Api.Collider.isTrigger = true;
        }
    }
}