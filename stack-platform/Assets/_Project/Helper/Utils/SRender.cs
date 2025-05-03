using UnityEngine;

namespace _Project.Helper.Utils
{
    public static class SRender 
    {
        public static bool IsObjectOutOfCameraFrustum(Renderer renderer, UnityEngine.Camera camera)
        {
            if (renderer == null)
            {
                Debug.LogError("The object you want to check if it is in the camera frustum area " +
                               "does not have a Renderer component.");
                return false;
            }

            var planes = GeometryUtility.CalculateFrustumPlanes(camera);

            return !GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        }
        public static Vector3 AnyObjectVertexLocation(Renderer rend, VertexLocation location, bool maxY = true)
        {
            var b = rend.bounds;
            var loc = Vector3.zero;
            var maxYLoc = maxY ? b.max.y : b.min.y;
            loc = location switch
            {
                VertexLocation.Center => new Vector3(b.center.x, maxYLoc, b.center.z),
                VertexLocation.LeftForward => new Vector3(b.min.x, maxYLoc, b.max.z),
                VertexLocation.Forward => new Vector3(b.center.x, maxYLoc, b.max.z),
                VertexLocation.RightForward => new Vector3(b.max.x, maxYLoc, b.max.z),
                VertexLocation.Left => new Vector3(b.min.x, maxYLoc, b.center.z),
                VertexLocation.Right => new Vector3(b.max.x, maxYLoc, b.center.z),
                VertexLocation.LeftBackward => new Vector3(b.min.x, maxYLoc, b.min.z),
                VertexLocation.Backward => new Vector3(b.center.x, maxYLoc, b.min.z),
                VertexLocation.RightBackward => new Vector3(b.max.x, maxYLoc, b.min.z),
                _ => loc
            };

            return loc;
        }
        public static void AnyObjectCornerVertexLocation(Renderer rend, out Vector3 left, out Vector3 right, VertexLocation leftLocation, VertexLocation rightLocation, bool maxY = true)
        {
            var b = rend.bounds;
            var loc = Vector3.zero;
            var maxYLoc = maxY ? b.max.y : b.min.y;
            left = leftLocation switch
            {
                VertexLocation.LeftForward => new Vector3(b.min.x, maxYLoc, b.max.z),
                VertexLocation.LeftBackward => new Vector3(b.min.x, maxYLoc, b.min.z),
                _ => loc
            };
            right = rightLocation switch
            {
                VertexLocation.RightForward => new Vector3(b.max.x, maxYLoc, b.max.z),
                VertexLocation.RightBackward => new Vector3(b.max.x, maxYLoc, b.min.z),
                _ => loc
            };
        }
    }
    
    public enum AlignAxis
    {
        X,Y,Z
    }
    
    public enum VertexLocation
    {
        Center,
        LeftForward,
        Forward,
        RightForward,
        Left,
        Right,
        LeftBackward,
        Backward,
        RightBackward,
    }
}