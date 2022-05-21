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
    [SerializeField] private TrackingCamera trackingCamera;

    [SerializeField] private float _movementSpeed = 2f;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (isClient && isLocalPlayer)
        {
            trackingCamera = Camera.main.transform.GetComponent<TrackingCamera>();
            trackingCamera.SetTrackingTransform(transform);
        }
    }

    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector2 currentPos = _rigidbody2D.position;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 inputVector = new Vector2(horizontalInput, verticalInput).normalized;
        Vector2 movement = inputVector * _movementSpeed;
        Vector2 newPos = currentPos + movement * Time.fixedDeltaTime;

        _rigidbody2D.MovePosition(newPos);
    }
}
