using UnityEngine;
using TMPro;
using Unity.Netcode;

// Manages the scores for host and client players in a networked game
public class ScoreManager : NetworkBehaviour
{
    // Network variables to store scores for host and client, only writable by the server
    private NetworkVariable<int> hostScore = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Server);
    private NetworkVariable<int> clientScore = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Server);

    // UI elements to display scores
    [SerializeField] private TextMeshProUGUI hostScoreText;
    [SerializeField] private TextMeshProUGUI clientScoreText;

    private void Awake()
    {
        // Subscribe to score changes to update the UI
        hostScore.OnValueChanged += UpdateHostScoreUI;
        clientScore.OnValueChanged += UpdateClientScoreUI;
    }

    // Method to add score to either host or client
    public void AddScore(bool hostScored)
    {
        if (IsServer)
        {
            if (hostScored)
            {
                hostScore.Value++;
            }
            else
            {
                clientScore.Value++;
            }
        }
    }

    // Updates the host score UI when the score changes
    private void UpdateHostScoreUI(int oldScore, int newScore)
    {
        hostScoreText.text = $"{newScore}";
    }

    // Updates the client score UI when the score changes
    private void UpdateClientScoreUI(int oldScore, int newScore)
    {
        clientScoreText.text = $"{newScore}";
    }
}