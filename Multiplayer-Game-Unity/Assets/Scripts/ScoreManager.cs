using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int playerOneScore = 0;
    private int playerTwoScore = 0;

    public void AddScore(bool playerOneScored)
    {
        if (playerOneScored)
        {
            playerOneScore++;
        }
        else
        {
            playerTwoScore++;
        }
        DisplayScores();
    }

    public int GetPlayerOneScore()
    {
        return playerOneScore;
    }

    public int GetPlayerTwoScore()
    {
        return playerTwoScore;
    }

    private void DisplayScores()
    {
        Debug.Log($"Player One: {playerOneScore} - Player Two: {playerTwoScore}");
    }
}