using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float rotationSpeed = 720f; // Rotation speed for adjusting orientation
    public LayerMask walkableLayer; // Layer for ground and walls
    private Rigidbody rb;
    private Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get movement input
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.z = Input.GetAxis("Vertical");

        // Normalize input to avoid faster diagonal movement
        moveInput.Normalize();
    }

    void FixedUpdate()
    {
        // Cast a ray downwards to check if player is on ground or wall
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 1.5f, walkableLayer))
        {
            // Adjust the player's rotation to match the surface normal
            Vector3 surfaceNormal = hit.normal;
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, surfaceNormal) * transform.rotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move the player in the direction of the input relative to the surface
            Vector3 relativeMoveDir = transform.TransformDirection(moveInput);
            rb.velocity = new Vector3(relativeMoveDir.x * speed, rb.velocity.y, relativeMoveDir.z * speed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Handle collision with the wall and ground to make sure the player can stick to them
        if ((walkableLayer & (1 << collision.gameObject.layer)) != 0)
        {
            // You can implement any specific behavior when touching the ground or walls here
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Player leaves the walkable surface
        if ((walkableLayer & (1 << collision.gameObject.layer)) != 0)
        {
            // You can implement any specific behavior when leaving the ground or walls here
        }
    }
}
