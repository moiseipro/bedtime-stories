using Mirror;
using UnityEngine;
using UnityTemplateProjects.Gameplay;

namespace Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class IsometricCharacterMovement : MonoBehaviour
    {

        private CharacterController _characterController;
        [SerializeField] private TrackingCamera trackingCamera;

        [SerializeField] private float _movementSpeed = 4f;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            trackingCamera = Camera.main.transform.GetComponent<TrackingCamera>();
            trackingCamera.SetTrackingTransform(transform);
        }

        void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            Vector3 currentPos = transform.position;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            
            Vector3 inputVector = new Vector3(horizontalInput, Gravity(), verticalInput).normalized;
            Vector3 movement = inputVector * _movementSpeed;
            _characterController.Move(movement * Time.fixedDeltaTime);
        }

        private float Gravity()
        {
            float value;
            if (!_characterController.isGrounded)
            {
                value = -1f;
            }
            else
            {
                value = 0f;
            }

            return value;
        }
    }
}
