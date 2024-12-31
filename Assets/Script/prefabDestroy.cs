using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyWithTimer : MonoBehaviour
{
    public float destroyDelay = 3.0f; // Delay in seconds before destroying the GameObject
    private bool isGrabbed;

    void Start()
    {
        
    }

    void Update()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(destroyDelay);

        // Destroy the GameObject
        Destroy(gameObject);
    }
}