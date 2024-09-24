using UnityEngine;

public class CustomPlayerInput : MonoBehaviour
{
    [HideInInspector]
    public float horizontalInput, verticalInput;

    [HideInInspector]
    public bool jumpButtonPressed;

    void Update()
    {
        // Update input values every frame
        GetMovementInput();
        CheckJumpInput();
    }

    public void GetMovementInput()
    {
        // Check for horizontal and vertical input using ternary operators
        horizontalInput = Input.GetKey(KeyCode.D) ? 1f : (Input.GetKey(KeyCode.A) ? -1f : 0f);
        verticalInput = Input.GetKey(KeyCode.W) ? 1f : (Input.GetKey(KeyCode.S) ? -1f : 0f);

        // Optional: Normalize input vector if needed
        Vector3 inputVector = new Vector3(horizontalInput, 0, verticalInput).normalized;
        horizontalInput = inputVector.x;
        verticalInput = inputVector.z;
    }

    public void CheckJumpInput()
    {
        // Check if the jump button (Space key) is pressed
        jumpButtonPressed = Input.GetKeyDown(KeyCode.Space);
    }
}
