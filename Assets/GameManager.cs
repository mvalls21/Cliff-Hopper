using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool IsPlayerAlive { get; private set; } = true;

    public int NumberCoins { get; private set; } = 0;

    public void PlayerDied()
    {
        IsPlayerAlive = false;
    }

    public void IncreaseCoin()
    {
        NumberCoins++;
        Debug.Log($"Number of coins: {NumberCoins}");
    }
}