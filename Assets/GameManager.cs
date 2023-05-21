using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool IsPlayerAlive { get; private set; } = true;

    public int NumberCoins { get; private set; } = 0;

    public int Score { get; private set; } = 0;

    public event EventHandler<int> ScoreChanged;

    public void PlayerDied()
    {
        IsPlayerAlive = false;
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
}