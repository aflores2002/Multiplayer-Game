using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// Handles the spawning of the ball in a networked game
public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab; // Prefab of the ball to spawn
    public Vector3 spawnPosition = new Vector3(0.1f, 0f, 0f); // Set spawn position for ball

    void Start()
    {
        // Subscribe to the server start event to spawn the ball
        NetworkManager.Singleton.OnServerStarted += SpawnBall;
    }

    // Spawns the ball when the server starts
    private void SpawnBall()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            // Instantiate the ball at the specified position
            GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            // Spawn the ball as a networked object
            ball.GetComponent<NetworkObject>().Spawn();
        }
    }
}