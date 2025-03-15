using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CustomPlayerSpawner : NetworkBehaviour
{
    public GameObject redPlayerPrefab;
    public GameObject bluePlayerPrefab;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Spawn the host as the red player
            GameObject redPlayer = Instantiate(redPlayerPrefab, Vector3.zero, Quaternion.identity);
            redPlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
        }
        else
        {
            // Spawn the client as the blue player
            GameObject bluePlayer = Instantiate(bluePlayerPrefab, Vector3.zero, Quaternion.identity);
            bluePlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
        }
    }
}