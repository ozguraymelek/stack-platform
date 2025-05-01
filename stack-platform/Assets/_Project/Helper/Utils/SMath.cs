using UnityEngine;

namespace _Project.Helper.Utils
{
    public static class SMath
    {
        public static Vector3 DirectionBetweenTwoVertexLocation(Renderer rend, VertexLocation loc0, VertexLocation loc1)
        {
            var tempLoc0 = SRender.AnyObjectVertexLocation(rend, loc0);
            var tempLoc1 = SRender.AnyObjectVertexLocation(rend, loc1);
            var direction = tempLoc0 - tempLoc1;
            return direction;
        }
    }
}
