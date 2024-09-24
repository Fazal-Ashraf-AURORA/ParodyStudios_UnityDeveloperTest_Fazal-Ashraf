using UnityEngine;

public class CollectCube : MonoBehaviour
{
    [Header("Collection Settings")]
    public string targetTag = "cube"; // Tag for collectible objects

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider has the correct tag
        if (other.CompareTag(targetTag))
        {
            Collect(other.gameObject);
        }
    }

    
    private void Collect(GameObject collectedObject)
    {
        Debug.Log("Cube Collected!");

        ScoreManager.instance.AddScore();

        // Destroy the collected object
        Destroy(collectedObject);
    }
}
