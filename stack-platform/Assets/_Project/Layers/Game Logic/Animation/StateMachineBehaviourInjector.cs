using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Animation
{
    [RequireComponent(typeof(Animator))]
    public class StateMachineBehaviourInjector : MonoBehaviour
    {
        [Inject] DiContainer _container;
        [SerializeField] private Animator animator;
        
        void Awake()
        {
            animator ??= GetComponent<Animator>();
            
            foreach (var behaviours in animator.GetBehaviours<StateMachineBehaviour>())
            {
                _container.Inject(behaviours);
            }
        }
    }
}