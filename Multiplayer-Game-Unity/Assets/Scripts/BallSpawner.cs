using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BallSpawner : MonoBehaviour
{
    public GameObject ballPrefab;
    public Vector3 spawnPosition = new Vector3(0.1f, 0f, 0f); // Set your desired spawn position here

    void Start()
    {
        NetworkManager.Singleton.OnServerStarted += SpawnBall;
    }

    private void SpawnBall()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            ball.GetComponent<NetworkObject>().Spawn();
        }
    }
}