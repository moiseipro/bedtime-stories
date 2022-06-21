using System;
using UnityEngine;

namespace UnityTemplateProjects.Gameplay
{
    public class TrackingCamera : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Vector3 _offsetTracking;
        
        [SerializeField] private Transform _target;

        private void FixedUpdate()
        {
            if (_target)
            {
                Vector3 trackingPosition = _target.position + (Vector3) _offsetTracking;
                transform.position = Vector3.Lerp(transform.position, trackingPosition, _speed * Time.fixedDeltaTime);
            }
        }

        public void SetTrackingTransform(Transform newTarget)
        {
            _target = newTarget;
        }
    }
}