using _Project.Helper.Utils;
using _Project.Layers.Game_Logic.Game_Flow.Level_Finish;
using _Project.Layers.Game_Logic.Platform;
using UnityEngine;

namespace _Project.Layers.Data.Entities
{
    [CreateAssetMenu(fileName = "Level Data", menuName = "Data/Level/Create a new level data")]
    public class LevelEntity : ScriptableObject
    {
        public int PlatformCountLimit;
        public Vector3 FirstPlatformPosition;
        public Vector3 FinishPlatformLocalPosition;

        public bool IsReachedPlatformLimit;
        public bool IsReachedTarget;
        
        //TODO: change lastPlatform data type to finish platform script
        public void AlignFirstPlatform(IInteractable<Finish> lastPlatform, IInteractable<Platform> firstPlatform)
        {
            FirstPlatformPosition = FinishPlatformLocalPosition;
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
