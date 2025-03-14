using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    private BoxCollider2D boxCollider2D;

    void Awake()
    {
        // Use Awake instead of Start to ensure early initialization
        EnsureCollider();
    }

    void EnsureCollider()
    {
        // Try to get existing collider first
        boxCollider2D = GetComponent<BoxCollider2D>();

        // If no collider exists, add one
        if (boxCollider2D == null)
        {
            boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
        }

        // Null check before configuring
        if (boxCollider2D != null)
        {
            // Configure the collider
            boxCollider2D.isTrigger = false;
        }
        else
        {
            Debug.LogError("Could not add BoxCollider2D to " + gameObject.name);
        }
    }
}