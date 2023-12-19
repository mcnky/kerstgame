using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    //input fields
    private InputActionAsset inputAsset;
    private InputActionMap player;
    private InputAction shootAction;
    private InputAction move;

    //movement fields
    private Rigidbody rb;
    [SerializeField] private float movementForce = 1f;
    [SerializeField] private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    //shoot
    [SerializeField] private GameObject projectilePrefab; 
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Transform shootPoint;


    [SerializeField] private Camera playerCamera;
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();

        inputAsset = this.GetComponent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("Player");

        if (player != null)
        {
            shootAction = player.FindAction("Shoot");
            shootAction.performed += ctx => Shoot();
        }
        else
        {
            Debug.LogError("Player action map not found!");
        }
    }


    private void OnEnable()
    {
        move = player.FindAction("Move");
        player.Enable();
        shootAction.Enable();
    }

    private void OnDisable()
    {
        player.Disable();
        shootAction.Disable();
    }

    private void FixedUpdate()
    {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        if (rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;

        LookAt();
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;
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

    private void Shoot()
    {
        Debug.Log("Shoot method called."); // Controleer of deze methode wordt aangeroepen wanneer je probeert te schieten.

        // Maak een nieuw projectiel aan op de positie van de speler met de juiste rotatie
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, transform.rotation);

        Debug.Log("Projectile instantiated: " + projectile); // Controleer of het projectiel is geïnstantieerd.

        // Voeg snelheid toe aan het projectiel (bijvoorbeeld door een Rigidbody-component te gebruiken)
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        if (projectileRb != null)
        {
            projectileRb.velocity = transform.forward * projectileSpeed;
            Debug.Log("Projectile speed: " + projectileRb.velocity.magnitude); // Controleer of de snelheid is ingesteld.
        }
    }


}