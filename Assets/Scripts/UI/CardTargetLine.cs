using System;
using UnityEngine;

namespace UnityTemplateProjects.UI
{
    [RequireComponent(typeof(LineRenderer))]
    public class CardTargetLine : MonoBehaviour
    {
        [SerializeField] private float _speed = 3f;
        
        private LineRenderer _lineRenderer;
        private Camera _camera;

        private Transform startTarget;
        private Transform endTarget;

        private const int _pointCount = 10;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _camera = Camera.main;
        }

        private void Update()
        {
            if (endTarget == null || startTarget == null) return;
            Vector3[] points = new Vector3[_pointCount];
            _lineRenderer.positionCount = _pointCount;

            Vector3 endPosition = endTarget.position;
            Vector3 startPosition = startTarget.position;
            // screenStartPos.z = _camera.nearClipPlane;
            // screenStartPos = _camera.ScreenToWorldPoint(screenStartPos);
            
            Vector3 dir;
            dir = endPosition - startPosition;
            float magnitude = dir.magnitude;
            dir /= magnitude;
            Debug.DrawLine(startPosition, endTarget.position, Color.yellow);

            Vector3 prev = startPosition, next;
            for (int i = 0; i < points.Length; i++) {
                float t = magnitude * i / (points.Length-1);
                next = startPosition + dir * t;
                points[i] = next;
                Debug.DrawLine(prev, next, Color.blue);
                prev = next;
            }
            _lineRenderer.SetPositions(points);
            
        }

        public void ShowLineTarget(Transform start, Transform end)
        {
            startTarget = start;
            endTarget = end;
        }

        public void HideLineTarget()
        {
            _lineRenderer.positionCount = 0;
            startTarget = null;
            endTarget = null;
        }
    }
}