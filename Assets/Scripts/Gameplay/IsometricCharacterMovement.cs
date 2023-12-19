using UnityEditor;
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
            Rotation();
        }

        private void Move()
        {
            Vector3 currentPos = transform.position;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            
            Vector3 inputVector = new Vector3(horizontalInput, Gravity(), verticalInput).normalized;
            Vector3 movement = inputVector * _movementSpeed;
            _characterController.Move(movement * Time.fixedDeltaTime);
            /*if (Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput) > 0.2f)
            {
                Vector3 direction = -new Vector3(horizontalInput, 0, verticalInput).normalized;
                Quaternion rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.LookRotation(direction, Vector3.up),
                    Time.fixedDeltaTime * _movementSpeed);
                transform.rotation = rotation;
            }*/
            
        }

        private void Rotation()
        {
            var dx = Screen.width / 2f - Input.mousePosition.x;
            var dy = Screen.height / 2f - Input.mousePosition.y;
            
            float sR = Mathf.Atan2(dx, dy);
            float sD = 360 * sR / (2 * Mathf.PI);
            
            Quaternion target = Quaternion.Euler(transform.rotation.eulerAngles.x, sD, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.fixedDeltaTime * _movementSpeed);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, target, Time.fixedDeltaTime * 100f);
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
