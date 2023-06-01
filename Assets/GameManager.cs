using System;
using Unity.VisualScripting.FullSerializer.Internal.Converters;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Canvas playerDeadScreen;

    public Text currentScoreText;

    public Text currentCoinsText;

    public Text coinIcon;

    public static bool IsPlayerAlive = true;
    
    private int _numberCoins;

    public int NumberCoins
    {
        get => _numberCoins;
        private set
        {
            _numberCoins = value;
            
            if (currentCoinsText != null)
                currentCoinsText.text = $"{value}";
        }
    } 

    private int _score;

    public int Score
    {
        get => _score;
        private set
        {
            _score = value;
            
            if (currentScoreText != null)
                currentScoreText.text = $"{value}";
        }
    }

    static public int MaxScore = 0;

    public event EventHandler<int> ScoreChanged;

    private AudioSource _gameMusicSource;

    public void Start()
    {
        Score = 0;
        NumberCoins = PlayerPrefs.GetInt("NumberCoins");
        MaxScore = PlayerPrefs.GetInt("MaxScore");

        _gameMusicSource = GetComponent<AudioSource>();
        IsPlayerAlive = true;
    }

    public void PlayerDied()
    {
        if (!IsPlayerAlive) 
            return;
        
        IsPlayerAlive = false;

        Destroy(currentScoreText);
        Destroy(currentCoinsText);
        Destroy(coinIcon);

        currentCoinsText = null;
        currentCoinsText = null;
        
        _gameMusicSource.Stop();

        if (Score > MaxScore)
        {
            MaxScore = Score;
            PlayerPrefs.SetInt("MaxScore", MaxScore);
        }
        
        PlayerPrefs.SetInt("NumberCoins", NumberCoins);
        PlayerPrefs.Save();
        
        var screen = playerDeadScreen.GetComponent<DeadScreenController>();
        screen.Show(Score, MaxScore);
    }

    public void IncreaseCoin()
    {
        NumberCoins++;
    }

    public void IncreaseScore()
    {
        Score++;
        ScoreChanged?.Invoke(this, Score);
    }

    public void Restart()
    {
        SceneManager.LoadScene("Scenes/Game");
    }
}