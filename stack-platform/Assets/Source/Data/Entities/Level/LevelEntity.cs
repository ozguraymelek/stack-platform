using Source.Core.Utilities.External;
using Source.Gameplay.Platform.Wrappers;
using Source.Systems.Finish;
using UnityEngine;

namespace Source.Data.Entities.Level
{
    [CreateAssetMenu(fileName = "Level Data", menuName = "Data/Level/Create a new level data")]
    public class LevelEntity : ScriptableObject
    {
        public int PlatformCountLimit;

        public bool IsReachedPlatformLimit;

        private void OnEnable()
        {
            IsReachedPlatformLimit = false;
        }

        public void AlignFirstPlatform(IInteractable<Finish> lastPlatform, IInteractable<Gameplay.Platform.Platform> firstPlatform)
        {
            firstPlatform.GetTransform().transform.position = new Vector3(lastPlatform.GetTransform().position.x, 0.0f,
                                                                  lastPlatform.GetTransform().position.z)
                                                              + SMath.DirectionBetweenTwoVertexLocation(
                                                                  firstPlatform.GetRenderer(),
                                                                  VertexLocation.Forward, VertexLocation.Center) +
                                                              SMath.DirectionBetweenTwoVertexLocation(
                                                                  lastPlatform.GetRenderer(),
                                                                  VertexLocation.Forward, VertexLocation.Center);
        }
    }
}