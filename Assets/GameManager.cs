using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Canvas playerDeadScreen;

    public bool IsPlayerAlive { get; private set; } = true;

    public int NumberCoins { get; private set; } = 0;

    public int Score { get; private set; } = 0;

    public event EventHandler<int> ScoreChanged;

    public void PlayerDied()
    {
        IsPlayerAlive = false;

        var screen = playerDeadScreen.GetComponent<DeadScreenController>();
        screen.Show(Score);
    }

    public void IncreaseCoin()
    {
        NumberCoins++;
        Debug.Log($"Number of coins: {NumberCoins}");
    }

    public void IncreaseScore()
    {
        Score++;
        Debug.Log($"Score: {Score}");

        ScoreChanged?.Invoke(this, Score);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scenes/Game");
    }
}