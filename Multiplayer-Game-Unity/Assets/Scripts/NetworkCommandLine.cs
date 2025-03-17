using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// Handles command-line arguments to start the game in different network modes
public class NetworkCommandLine : MonoBehaviour
{
    private NetworkManager netManager; // Reference to the NetworkManager

    void Start()
    {
        netManager = GetComponentInParent<NetworkManager>();

        // Skip command-line processing in the Unity Editor
        if (Application.isEditor) return;

        // Retrieve command-line arguments
        var args = GetCommandlineArgs();

        // Check for the '-mode' argument and start the appropriate network mode
        if (args.TryGetValue("-mode", out string mode))
        {
            switch (mode)
            {
                case "server":
                    netManager.StartServer();
                    break;
                case "host":
                    netManager.StartHost();
                    break;
                case "client":
                    netManager.StartClient();
                    break;
            }
        }
    }

    // Parses command-line arguments into a dictionary
    private Dictionary<string, string> GetCommandlineArgs()
    {
        Dictionary<string, string> argDictionary = new Dictionary<string, string>();

        var args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; ++i)
        {
            var arg = args[i].ToLower();
            if (arg.StartsWith("-"))
            {
                var value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
                value = (value?.StartsWith("-") ?? false) ? null : value;

                argDictionary.Add(arg, value);
            }
        }
        return argDictionary;
    }
}