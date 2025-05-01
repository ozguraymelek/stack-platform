using UnityEngine;

namespace _Project.Helper.Utils
{
    public static class SRender 
    {
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
