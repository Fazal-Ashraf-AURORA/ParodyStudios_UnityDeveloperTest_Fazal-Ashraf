using System.Collections.Generic;
using UnityEngine;

public class GravityController : MonoBehaviour
{
    public float gravityStrength = 9.81f; // Default gravity strength
    private Vector3 newGravityDirection = Vector3.down; // Default gravity direction
    private CustomCharacterController playerController;

    [Header("Hologram References")]
    public GameObject hologramFront;
    public GameObject hologramBack;
    public GameObject hologramLeft;
    public GameObject hologramRight;

    private Dictionary<Vector3, GameObject> hologramMap;

    private void Start()
    {
        // Initialize the dictionary with the mappings
        hologramMap = new Dictionary<Vector3, GameObject>
        {
            { Vector3.left, hologramLeft },
            { Vector3.right, hologramRight },
            { Vector3.forward, hologramFront },
            { Vector3.back, hologramBack }
        };
        playerController = FindObjectOfType<CustomCharacterController>();
    }

    void Update()
    {
        // Detect which direction to set the new gravity to
        DetectGravityDirection();

        // Apply the new gravity when Enter is pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ApplyNewGravity();
        }

        // Show hologram indicating the new gravity direction
        DisplayHologram();
    }

    void DetectGravityDirection()
    {
        // Default to down direction
        newGravityDirection = Vector3.down;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            newGravityDirection = Vector3.forward; // Forward direction
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            newGravityDirection = Vector3.back; // Backward direction
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            newGravityDirection = Vector3.left; // Left direction
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            newGravityDirection = Vector3.right; // Right direction
        }
    }

    void ApplyNewGravity()
    {
        Physics.gravity = newGravityDirection * gravityStrength;
        Debug.Log("New Gravity Direction: " + newGravityDirection);

        // Rotate the player to align with the new gravity direction
        if (playerController != null)
        {
            playerController.AdjustOrientation(newGravityDirection);
        }
    }

    void DisplayHologram()
    {
        // First, disable all holograms
        foreach (var hologram in hologramMap.Values)
        {
            hologram.SetActive(false);
        }

        // If there's a corresponding hologram, activate it
        if (hologramMap.TryGetValue(newGravityDirection, out GameObject hologramToActivate))
        {
            hologramToActivate.SetActive(true);
        }
    }
}
