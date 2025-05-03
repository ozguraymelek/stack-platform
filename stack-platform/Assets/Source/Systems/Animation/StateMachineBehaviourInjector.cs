using UnityEngine;
using Zenject;

namespace Source.Systems.Animation
{
    [RequireComponent(typeof(Animator))]
    public class StateMachineBehaviourInjector : MonoBehaviour
    {
        [Inject] private DiContainer _container;
        [SerializeField] private Animator animator;
        
        private void Awake()
        {
            animator ??= GetComponent<Animator>();
            
            foreach (var behaviours in animator.GetBehaviours<StateMachineBehaviour>())
            {
                _container.Inject(behaviours);
            }
        }
    }
}