using _Project.Helper.Utils;
using _Project.Layers.Game_Logic.Platform;
using UnityEngine;

namespace _Project.Layers.Data.Entities
{
    [CreateAssetMenu(fileName = "Level Data", menuName = "Data/Level/Create a new level data")]
    public class LevelEntity : MonoBehaviour
    {
        public int PlatformCountLimit;
        public Vector3 FirstPlatformPosition;
        public Vector3 FinishPlatformLocalPosition;

        //TODO: change lastPlatform data type to finish platform script
        public void AlignFirstPlatform(Platform lastPlatform, Platform firstPlatform)
        {
            // FirstPlatformPosition = FinishPlatformLocalPosition;
            firstPlatform.transform.position = new Vector3(lastPlatform.transform.position.x, 0.0f, lastPlatform.transform.position.z)
                                               + SMath.DirectionBetweenTwoVertexLocation(firstPlatform.Renderer,
                                                   VertexLocation.Forward, VertexLocation.Center) +
                                               SMath.DirectionBetweenTwoVertexLocation(lastPlatform.Renderer,
                                                   VertexLocation.Forward, VertexLocation.Center);
        }
    }
}
