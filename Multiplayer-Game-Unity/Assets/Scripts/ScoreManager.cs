using UnityEngine;
using TMPro;
using Unity.Netcode;

public class ScoreManager : NetworkBehaviour
{
    private NetworkVariable<int> hostScore = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Server);
    private NetworkVariable<int> clientScore = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Server);

    [SerializeField] private TextMeshProUGUI hostScoreText;
    [SerializeField] private TextMeshProUGUI clientScoreText;

    private void Awake()
    {
        hostScore.OnValueChanged += UpdateHostScoreUI;
        clientScore.OnValueChanged += UpdateClientScoreUI;
    }

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

    private void UpdateHostScoreUI(int oldScore, int newScore)
    {
        hostScoreText.text = $"Host Score: {newScore}";
    }

    private void UpdateClientScoreUI(int oldScore, int newScore)
    {
        clientScoreText.text = $"Client Score: {newScore}";
    }
}