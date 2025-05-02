using _Project.Helper.Utils;
using _Project.Layers.Game_Logic.Platform;
using _Project.Layers.Infrastructure.Pools;
using UnityEngine;
using Zenject;

namespace _Project.Layers.Game_Logic.Game_Flow.Level_Finish
{
    public class FinishSpawner : MonoBehaviour
    {
        public Finish FinishPlatformPrefab;
        
        private SignalBus _signalBus;
        private PlatformTracker _platformTracker;
        [InjectOptional(Optional = true)]
        private LevelManager _levelManager;
        private FinishInteraction.Factory _finishInteractionFactory;
        
        private Vector3 _diffDirectionBetweenFwAndCenterForInitialPlatform;
        private Vector3 _diffDirectionBetweenFwAndCenterForSpawnedPlatform;
        
        [Inject]
        public void Construct(SignalBus signalBus, PlatformTracker platformTracker, FinishInteraction.Factory finishInteractionFactor)
        {
            _signalBus = signalBus;
            _platformTracker = platformTracker;
            _finishInteractionFactory = finishInteractionFactor;
        }
        
        public void SpawnFinishPlatform()
        {
            if (_platformTracker.InitialPlatform == null) return;
            var spawnedFinishPlatform = _finishInteractionFactory.Create().GetComponent<Finish>();
            // Debug.Log(_platformTracker.InitialPlatform.GetTransform().name);
            // Debug.Log(_platformTracker.InitialPlatform.GetRenderer().name);
            // Debug.Log(spawnedFinishPlatform.Renderer);
            _diffDirectionBetweenFwAndCenterForInitialPlatform = SMath.DirectionBetweenTwoVertexLocation(_platformTracker.InitialPlatform.GetRenderer(),
                VertexLocation.Forward, VertexLocation.Center);
            _diffDirectionBetweenFwAndCenterForSpawnedPlatform = SMath.DirectionBetweenTwoVertexLocation(spawnedFinishPlatform.Renderer,
                VertexLocation.Forward, VertexLocation.Center);
            
            spawnedFinishPlatform.transform.localPosition = _platformTracker.InitialPlatform.GetTransform().localPosition +
                                                            new Vector3(0f, 0.05f,
                                                                _diffDirectionBetweenFwAndCenterForInitialPlatform.z + (_diffDirectionBetweenFwAndCenterForInitialPlatform.z * 2 *
                                                                             (_levelManager.CurrentLevel
                                                                                  .PlatformCountLimit -
                                                                              1)) +
                                                                _diffDirectionBetweenFwAndCenterForSpawnedPlatform.z);
            
            spawnedFinishPlatform.transform.SetParent(transform, true);

            _levelManager.CurrentLevel.FinishPlatformLocalPosition = spawnedFinishPlatform.transform.position;
        }
    }
}
