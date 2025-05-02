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
        
        public static float AngleBetweenSpecificLocations(Vector3 to, Vector3 from, bool wantConjugateAngle = false)
        {
            var direction = to - from;
            // direction.y = 0;

            if (direction.sqrMagnitude == 0)
                return 0f;

            var angleRad = Mathf.Atan2(direction.x, direction.z);
            var angleDeg = angleRad * Mathf.Rad2Deg;

            if (!wantConjugateAngle) return angleDeg;
            
            if (angleDeg < 0)
                angleDeg += 360f;

            return angleDeg;
        }
    }
}
