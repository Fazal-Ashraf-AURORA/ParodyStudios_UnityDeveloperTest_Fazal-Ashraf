//using UnityEngine;
//using UnityEngine.EventSystems;

//[RequireComponent(typeof(CharacterController))]
//public class CustomCharacterController : MonoBehaviour
//{
//    [Header("Movement Settings")]
//    public float speed = 6f;
//    public float jumpHeight = 2f;
//    public float gravity = -9.81f;
//    public float turnSmoothTime = 0.1f;
//    public LayerMask groundLayer; // Assign this in the inspector

//    private Vector3 direction;
//    private Vector3 currentGravityDirection = Vector3.down;
//    private float turnSmoothVelocity;
//    private float verticalVelocity;
//    private CharacterController characterController;
//    private CustomPlayerInput playerInput;
//    private Transform cam;
//    public Animator playerAnimator;

//    void Start()
//    {
//        // Lock the cursor
//        Cursor.lockState = CursorLockMode.Locked;

//        // Get the required components
//        characterController = GetComponent<CharacterController>();
//        playerInput = GetComponent<CustomPlayerInput>();
//        cam = Camera.main.transform; // Assuming the camera is the main camera
//    }

//    void Update()
//    {
//        // Update movement and jump every frame
//        HandleMovement();
//        HandleJump();
//        HandleAnimations();
//    }

//    private void HandleMovement()
//    {
//        // Get input values from the CustomPlayerInput script
//        float horizontal = playerInput.horizontalInput;
//        float vertical = playerInput.verticalInput;

//        // Create a direction vector based on input
//        direction = new Vector3(horizontal, 0, vertical).normalized;

//        if (direction.magnitude >= 0.1f)
//        {
//            // Calculate target angle based on camera direction and player input
//            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
//            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
//            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

//            // Calculate movement direction based on target angle
//            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

//            // Move the player using the character controller
//            characterController.Move(moveDirection.normalized * speed * Time.deltaTime);
//        }
//    }

//    private void HandleJump()
//    {
//        if (playerInput.jumpButtonPressed && IsGrounded())
//        {
//            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
//        }

//        verticalVelocity += gravity * Time.deltaTime;

//        Vector3 verticalMovement = new Vector3(0, verticalVelocity, 0);
//        characterController.Move(verticalMovement * Time.deltaTime);
//    }

//    void HandleAnimations()
//    {
//        // Check if the player is moving
//        bool isMoving = direction != Vector3.zero;

//        if (IsGrounded())
//        {
//            if (isMoving)
//            {
//                // Play running animation if player is moving and grounded
//                playerAnimator.Play("Running");
//            }
//            else
//            {
//                // Play idle animation if player is not moving and grounded
//                playerAnimator.Play("Idle");
//            }
//        }
//        else
//        {
//            // Play falling animation if player is not grounded
//            playerAnimator.Play("Falling Idle");
//        }
//    }

//    private bool IsGrounded()
//    {
//        float groundCheckRadius = 0.2f;
//        Vector3 groundCheckPosition = transform.position + Vector3.down * (characterController.height / 2);
//        return Physics.CheckSphere(groundCheckPosition, groundCheckRadius, groundLayer);
//    }

//    public void AdjustOrientation(Vector3 newGravityDirection)
//    {
//        // Rotate the player so the 'down' of the player matches the new gravity direction
//        currentGravityDirection = newGravityDirection;
//        transform.up = -newGravityDirection;

//        // Reposition player slightly to make sure they don't get stuck in the ground
//        characterController.Move(newGravityDirection * 0.5f);
//    }
//}


using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class CustomCharacterController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 6f;
    public float jumpForce = 5f;
    public float gravityMultiplier = 1f;
    public LayerMask groundLayer;

    private Vector3 direction;
    private Vector3 currentGravityDirection = Vector3.down;
    private Rigidbody rb;
    private bool isGrounded;

    private CustomPlayerInput playerInput;
    private Transform cam;
    public Animator playerAnimator;

    public bool GCHK;

    void Start()
    {
        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;

        // Get Rigidbody and freeze its rotation
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        // Get input and camera
        playerInput = GetComponent<CustomPlayerInput>();
        cam = Camera.main.transform;
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAnimations();

        if (isGrounded )
        {
            GCHK = true;
        }
        else
        {
            GCHK = false;
        }
    }

    private void HandleMovement()
    {
        float horizontal = playerInput.horizontalInput;
        float vertical = playerInput.verticalInput;

        // Get movement direction relative to the current gravity axis
        direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Calculate the movement direction based on camera
            Vector3 moveDirection = cam.forward * vertical + cam.right * horizontal;
            moveDirection = Vector3.ProjectOnPlane(moveDirection, currentGravityDirection).normalized;

            rb.MovePosition(rb.position + moveDirection * speed * Time.deltaTime);
        }
    }

    private void HandleJump()
    {
        if (playerInput.jumpButtonPressed && IsGrounded())
        {
            // Add jump force along the current gravity's opposite direction
            rb.AddForce(-currentGravityDirection * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        float distanceToGround = 1f;  // Adjust as needed
        RaycastHit hit;

        // Perform the raycast to check for ground
        isGrounded = Physics.Raycast(transform.position, currentGravityDirection, out hit, distanceToGround, groundLayer);

        // Debugging: Draw the ray in the Scene view for visualization
        Debug.DrawRay(transform.position, -currentGravityDirection * distanceToGround, isGrounded ? Color.green : Color.red);

        return isGrounded;
    }


    //private bool IsGrounded()
    //{
    //    // Define the distance to check for ground below the player's feet
    //    float groundCheckDistance = 1f;  // Adjust this value if needed

    //    // Calculate the bottom of the capsule based on current gravity
    //    Vector3 capsuleBottom = transform.position + (-currentGravityDirection * (GetComponent<CapsuleCollider>().height / 2));

    //    // Perform a raycast or a sphere cast to check if there's ground in the direction of gravity
    //    isGrounded = Physics.Raycast(capsuleBottom, -currentGravityDirection, groundCheckDistance, groundLayer);

    //    return isGrounded;
    //}

    //private bool IsGrounded()
    //{
    //    float groundCheckRadius = 0.5f;  // Adjust the radius as needed
    //    float groundCheckDistance = 0.5f;  // Adjust how far to check for ground (distance below player)

    //    // Calculate the bottom of the capsule based on the current gravity direction
    //    CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
    //    Vector3 capsuleBottom = transform.position + (-currentGravityDirection * (capsuleCollider.height / 2));

    //    // Perform a SphereCast in the direction of gravity to check for ground
    //    bool isGrounded = Physics.SphereCast(capsuleBottom, groundCheckRadius, -currentGravityDirection, out RaycastHit hit, groundCheckDistance, groundLayer);

    //    // Optional Debugging: Visualize the sphere cast in the editor (remove this line in production)
    //    Debug.DrawRay(capsuleBottom, -currentGravityDirection * groundCheckDistance, isGrounded ? Color.green : Color.red);

    //    return isGrounded;
    //}




    void HandleAnimations()
    {
        bool isMoving = direction != Vector3.zero;

        if (IsGrounded())
        {
            if (isMoving)
            {
                playerAnimator.Play("Running");
            }
            else
            {
                playerAnimator.Play("Idle");
            }
        }
        else
        {
            playerAnimator.Play("Falling Idle");
        }
    }

    public void AdjustOrientation(Vector3 newGravityDirection)
    {
        currentGravityDirection = newGravityDirection;
        // Re-align the player's up direction
        transform.up = -newGravityDirection;
    }
}

