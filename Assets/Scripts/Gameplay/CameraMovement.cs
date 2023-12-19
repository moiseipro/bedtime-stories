using System;
using UnityEngine;

namespace Gameplay
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Vector3 _offsetTracking;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        private void FixedUpdate()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            float rotation = Input.GetKey(KeyCode.Q) ? -1 : Input.GetKey(KeyCode.E) ? 1 : 0;
            Vector3 trackingPosition = transform.position;
            trackingPosition.x += horizontalInput;
            trackingPosition.z += verticalInput;
            _transform.position = Vector3.Lerp(transform.position, trackingPosition, _speed * Time.fixedDeltaTime);
        }
    }
}