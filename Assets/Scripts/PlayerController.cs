using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    private Player playerAsset;
    private InputAction move;

    private Rigidbody rb;
    [SerializeField] float movementForce = 1f;
    [SerializeField] float maxSpeed = 1f;
    [SerializeField] Vector3 forceDirection = Vector3.zero;
    [SerializeField] Camera playerCamera;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAsset = new Player();
        InputSystem.onDeviceChange += DeviceCheck;
    }

    private void OnEnable()
    {
        move = playerAsset.Movement.Move;
        playerAsset.Movement.Enable();
    }

    private void OnDisable()
    {
        playerAsset.Movement.Disable();
    }

    private void FixedUpdate()
    {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0f;
        if(horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rb.velocity = horizontalVelocity.normalized * maxSpeed;
        }
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right; 
        right.y = 0;
        return right.normalized;
    }
    void OnDestroy()
    {
        InputSystem.onDeviceChange -= DeviceCheck;
    }

    private void DeviceCheck(InputDevice device, InputDeviceChange change)
    {

        if (device is Gamepad)
        {
            if (change == InputDeviceChange.Added)
            {
                Debug.Log("Gamepad verbonden!");

            }
            else if (change == InputDeviceChange.Removed)
            {
                Debug.Log("Gamepad losgekoppeld!");

            }
        }
    }

}
