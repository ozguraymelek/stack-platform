using Source.Core.Utilities.External;
using Source.Data.Platform;
using Source.Systems.GameFlow;
using UnityEngine;
using Zenject;

namespace Source.Systems.Finish
{
    public class FinishSpawner : MonoBehaviour
    {
        private PlatformTracker _platformTracker;
        
        [InjectOptional(Optional = true)]
        private LevelManager _levelManager;
        
        private FinishInteraction.Factory _finishInteractionFactory;
        
        private Vector3 _diffDirectionBetweenFwAndCenterForInitialPlatform;
        private Vector3 _diffDirectionBetweenFwAndCenterForSpawnedPlatform;
        
        [Inject]
        public void Construct(PlatformTracker platformTracker, FinishInteraction.Factory finishInteractionFactor)
        {
            _platformTracker = platformTracker;
            _finishInteractionFactory = finishInteractionFactor;

            SLog.InjectionStatus(this,
                (nameof(_platformTracker), _platformTracker),
                (nameof(_finishInteractionFactory), _finishInteractionFactory)
            );
        }
        
        public void SpawnFinishPlatform()
        {
            if (_platformTracker.InitialPlatform == null) return;
            var spawnedFinishPlatform = _finishInteractionFactory.Create().GetComponent<Finish>();
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
        }
    }
}