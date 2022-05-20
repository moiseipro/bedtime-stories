using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityTemplateProjects.Gameplay;

[RequireComponent(typeof(Rigidbody2D))]
public class IsometricCharacterMovement : NetworkBehaviour
{

    private Rigidbody2D _rigidbody2D;
    private TrackingCamera trackingCamera;

    [SerializeField] private float _movementSpeed;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (hasAuthority)
        {
            trackingCamera = Camera.main.GetComponent<TrackingCamera>();
            trackingCamera.SetTrackingTransform(transform);
        }
    }

    void Start()
    {
        
    }
    
    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            Vector2 currentPos = _rigidbody2D.position;
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector2 inputVector = new Vector2(horizontalInput, verticalInput);
            Vector2 movement = inputVector * _movementSpeed;
            Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;

            _rigidbody2D.MovePosition(newPos);
        }
    }
}
