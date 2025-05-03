using System.Collections.Generic;
using _Project.Helper.Utils;
using UnityEngine;

namespace _Project.Layers.Game_Logic.Effect
{
    [RequireComponent(typeof(TrailRenderer))]
    public class EdgeOutline : MonoBehaviour
    {
        public Renderer targetRenderer;
        public float dashSpeed = 1f;

        public int currentIndex = 0;
        public bool isMoving = true;  
        public TrailRenderer trail;
        public List<Vector3> waypoints;
    
        Vector3 dl = Vector3.zero;
        Vector3 dr = Vector3.zero;
        Vector3 ul = Vector3.zero;
        Vector3 ur = Vector3.zero;
    
        private void OnEnable()
        {
            SRender.AnyObjectAllCornerVerticesLocation(targetRenderer, out ul, out ur, out dl, out dr);
            waypoints.Add(dl);
            waypoints.Add(ul);
            waypoints.Add(ur);
            waypoints.Add(dr);
        
            if (waypoints == null || waypoints.Count < 2)
            {
                enabled = false;
                return;
            }

            trail.transform.position = waypoints[0];
            currentIndex = 1;
            trail = GetComponent<TrailRenderer>();
            trail.Clear();
            isMoving = true;
        }

        private void OnDisable()
        {
            waypoints.Clear();
        }

        private void Update()
        {
            if (!isMoving) return;

            if (currentIndex >= waypoints.Count)
            {
                isMoving = false;
                return;
            }

            var targetPos = waypoints[currentIndex];
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                dashSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                currentIndex++;
            }
        }
    }
}