using System.Collections.Generic;
using Source.Core.Utilities.External;
using UnityEngine;
using UnityEngine.Serialization;

namespace Source.Systems.Effects
{
    [RequireComponent(typeof(TrailRenderer))]
    public class EdgeOutline : MonoBehaviour
    {
        [SerializeField] public Renderer targetRenderer;
        [SerializeField] public float dashSpeed = 1f;

        private int _currentIndex = 0;
        private bool _isMoving = true;  
        public TrailRenderer Trail;
        private List<Vector3> _waypoints = new List<Vector3>();
    
        private Vector3 dl = Vector3.zero;
        private Vector3 dr = Vector3.zero;
        private Vector3 ul = Vector3.zero;
        private Vector3 ur = Vector3.zero;
    
        private void OnEnable()
        {
            SetWaypoints();
        }

        private void OnDisable()
        {
            _waypoints.Clear();
        }

        private void Update()
        {
            if (!_isMoving) return;

            if (_currentIndex >= _waypoints.Count)
            {
                _isMoving = false;
                return;
            }

            var targetPos = _waypoints[_currentIndex];
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                dashSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                _currentIndex++;
            }
        }

        private void SetWaypoints()
        {
            SRender.AnyObjectAllCornerVerticesLocation(targetRenderer, out ul, out ur, out dl, out dr);
            
            _waypoints.Add(dl);
            _waypoints.Add(ul);
            _waypoints.Add(ur);
            _waypoints.Add(dr);
        
            if (_waypoints == null || _waypoints.Count < 2)
            {
                enabled = false;
                return;
            }

            Trail.transform.position = _waypoints[0];
            _currentIndex = 1;
            Trail = GetComponent<TrailRenderer>();
            Trail.Clear();
            _isMoving = true;
        }
    }
}