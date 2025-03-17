using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

// Manages the UI for connecting as a host or client in a networked game
public class ConnectUIScript : MonoBehaviour
{
    [SerializeField] private Button hostButton; // Button to start hosting
    [SerializeField] private Button clientButton; // Button to start as client

    private void Start()
    {
        // Add listeners to buttons for hosting and joining as client
        hostButton.onClick.AddListener(HostButtonOnClick);
        clientButton.onClick.AddListener(ClientButtonOnClick);
    }

    // Starts the game as host when the host button is clicked
    private void HostButtonOnClick()
    {
        NetworkManager.Singleton.StartHost();
    }

    // Starts the game as client when the client button is clicked
    private void ClientButtonOnClick()
    {
        NetworkManager.Singleton.StartClient();
    }
}