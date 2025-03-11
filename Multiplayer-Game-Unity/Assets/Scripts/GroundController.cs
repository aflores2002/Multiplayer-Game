using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    private BoxCollider boxCollider;

    void Awake()
    {
        // Use Awake instead of Start to ensure early initialization
        EnsureCollider();
    }

    void EnsureCollider()
    {
        // Try to get existing collider first
        boxCollider = GetComponent<BoxCollider>();

        // If no collider exists, add one
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider>();
        }

        // Null check before configuring
        if (boxCollider != null)
        {
            // Configure the collider
            boxCollider.isTrigger = false;
        }
        else
        {
            Debug.LogError("Could not add BoxCollider to " + gameObject.name);
        }
    }
}