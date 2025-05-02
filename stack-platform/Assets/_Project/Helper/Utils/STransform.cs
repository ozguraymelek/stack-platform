using UnityEngine;

namespace _Project.Helper.Utils
{
    public static class STransform
    {
        public static Vector3 SetWithZRandX(this Transform transform, Transform refTr, 
            ref bool isRight, float offset, float constX)
        {
            var rightOrLeft = Random.Range(0, 2);
            var randX = 0f;
            if (rightOrLeft == 0) //right
            {
                randX = refTr.position.x + constX;
                isRight = true;
            }
            if (rightOrLeft == 1) //left
            {
                randX = refTr.position.x - constX;
                isRight = false;
            }

            return transform.position = new Vector3(randX, transform.position.y, refTr.position.z + offset);
        }
        
        public static Vector3 GetBottomCenter(GameObject obj)
        {
            var rend = obj.GetComponent<Renderer>();
            if (rend != null)
            {
                return new Vector3(rend.bounds.center.x, rend.bounds.max.y, rend.bounds.center.z - rend.bounds.extents.z);
            }

            return obj.transform.position;
        }
    }
}
