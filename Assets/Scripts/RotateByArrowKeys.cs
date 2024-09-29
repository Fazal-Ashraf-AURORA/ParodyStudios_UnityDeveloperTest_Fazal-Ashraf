using UnityEngine;

public class RotateByArrowKeys : MonoBehaviour
{
    // Angle to rotate by (90 degrees)
    private float rotationAngle = 90f;

    void Update()
    {
        // Check for arrow key input
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateObject(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            RotateObject(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RotateObject(Vector3.forward);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            RotateObject(Vector3.back);
        }
    }

    // Method to rotate the object
    void RotateObject(Vector3 direction)
    {
        // Rotate by 90 degrees along the specified axis
        transform.Rotate(direction * rotationAngle, Space.World);
    }
}
