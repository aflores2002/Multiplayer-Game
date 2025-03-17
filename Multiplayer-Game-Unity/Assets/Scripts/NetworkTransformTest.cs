using System;
using Unity.Netcode;
using UnityEngine;

// NetworkTransformTest script from Unity guide
public class NetworkTransformTest : NetworkBehaviour
{
    void Update()
    {
        if (IsServer)
        {
            float theta = Time.time;
            transform.position = new Vector3((float)Math.Cos(theta), 0.0f, (float)Math.Sin(theta));
        }
    }
}