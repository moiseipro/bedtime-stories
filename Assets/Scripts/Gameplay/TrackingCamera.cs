using System;
using UnityEngine;

namespace UnityTemplateProjects.Gameplay
{
    public class TrackingCamera : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Vector2 _offsetTracking;
        
        private Transform _target;

        private void Update()
        {
            if (_target)
            {
                Vector3 trackingPosition = _target.position + (Vector3) _offsetTracking;
                trackingPosition.z = transform.position.z;
                transform.position = Vector3.Lerp(transform.position, trackingPosition, _speed * Time.deltaTime);
            }
        }

        public void SetTrackingTransform(Transform newTarget)
        {
            _target = newTarget;
        }
    }
}