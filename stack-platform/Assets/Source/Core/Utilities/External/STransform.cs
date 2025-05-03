using UnityEngine;

namespace Source.Core.Utilities.External
{
    public static class STransform
    {
        public static Vector3 SetWithZRandX(this Transform transform, Transform refTr, 
            ref bool isRight, float offset, float constX)
        {
            var rightOrLeft = Random.Range(0, 2);
            var randX = 0f;
            
            switch (rightOrLeft)
            {
                //right
                case 0:
                    randX = refTr.position.x + constX;
                    isRight = true;
                    break;
                //left
                case 1:
                    randX = refTr.position.x - constX;
                    isRight = false;
                    break;
            }

            return transform.position = new Vector3(randX, transform.position.y, refTr.position.z + offset);
        }
        
        public static void AdjustPivotByMesh(GameObject obj)
        {
            var mf = obj.GetComponent<MeshFilter>();
            if (mf == null)
            {
                Debug.LogError($"[{mf.GetType().Name}] is null");
                return;
            }

            var mesh = mf.mesh;
            var vertices = mesh.vertices;

            var center = mesh.bounds.center;

            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] -= center;
            }

            mesh.vertices = vertices;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            obj.transform.position += obj.transform.TransformVector(center);
        }
        
        public static float GetAnyObjectWidth(GameObject obj)
        {
            var mf = obj.GetComponent<MeshFilter>();
            if (mf == null)
            {
                Debug.LogError($"[{mf.GetType().Name}] is null");
                return 0;
            }
            
            var mesh = mf.mesh;
            var size = mesh.bounds.size;

            var width = size.x * obj.transform.localScale.x;
            
            return width;
        }
        
        public static Vector3 GetBottomCenter(GameObject obj)
        {
            var rend = obj.GetComponent<Renderer>();
            if (rend != null)
            {
                return new Vector3(rend.bounds.center.x, rend.bounds.max.y, rend.bounds.center.z - rend.bounds.extents.z);
            }
            Debug.LogError($"[{rend.GetType().Name}] is null");
            return obj.transform.position;
        }
    }
}