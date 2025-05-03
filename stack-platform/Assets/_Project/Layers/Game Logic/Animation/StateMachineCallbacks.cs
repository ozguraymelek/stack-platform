using _Project.Layers.Game_Logic.Signals;
using UnityEngine;
using UnityEngine.Animations;
using Zenject;

namespace _Project.Layers.Game_Logic.Animation
{
    public class StateMachineCallbacks : StateMachineBehaviour
    {
        [Inject] private SignalBus _signalBus;
        
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            if (stateInfo.IsName("Run"))
            {
                Debug.LogWarning("Run");
                _signalBus.Fire(new MovementToggleSignal(true));
            }
            else if (stateInfo.IsName("Victory Idle") || stateInfo.IsName("Dance"))
            {
                Debug.LogWarning("Stopped");
                _signalBus.Fire(new MovementToggleSignal(false));
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, 
            AnimatorControllerPlayable controller)
        {
            if (stateInfo.IsName("Standing Up"))
            {
                Debug.LogWarning("Standing Up");
                _signalBus?.Fire<PhysicToggleSignal>();
            }
        }
    }
}
