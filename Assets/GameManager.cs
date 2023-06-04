using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton Info

    public static GameManager Instance { get; private set; }

    #endregion

    #region Game Overlay Canvas

    public Canvas gameOverlayCanvas;

    public Text currentScoreText;

    public Text currentCoinsText;

    public Text godModeText;

    public Text coinIcon;

    #endregion

    #region Additional Screens

    public Canvas playerDeadScreen;

    public Canvas pauseScreen;

    public Canvas victoryScreen;

    #endregion

    public bool IsPlayerAlive = true;

    private bool _isGamePaused = false;

    public bool IsGamePaused
    {
        get => _isGamePaused;
        private set
        {
            _isGamePaused = value;
            GamePausedChanged(this, value);
        }
    }

    private bool _godModeActive = false;

    public bool GodModeActive
    {
        get => _godModeActive;
        private set
        {
            _godModeActive = value;
            godModeText.gameObject.SetActive(value);
        }
    }

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

    public int MaxScore = 0;

    public event EventHandler<int> ScoreChanged;

    public event EventHandler<bool> GamePausedChanged;

    private AudioSource _gameMusicSource;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        Score = 0;
        NumberCoins = PlayerPrefs.GetInt("NumberCoins");
        MaxScore = PlayerPrefs.GetInt("MaxScore");

        _gameMusicSource = GetComponent<AudioSource>();
    }

    private bool _gPressed;

    private bool _pPressed;

    public void Update()
    {
        if (Input.GetKey(KeyCode.G) && !_gPressed)
        {
            GodModeActive = !GodModeActive;
        }

        if (Input.GetKey(KeyCode.P) && !_pPressed && IsPlayerAlive)
        {
            IsGamePaused = !IsGamePaused;

            var pausedScreenController = pauseScreen.GetComponent<PausedScreenController>();
            if (IsGamePaused)
            {
                gameOverlayCanvas.gameObject.SetActive(false);
                pausedScreenController.Show(Score, MaxScore);
            }
            else
            {
                gameOverlayCanvas.gameObject.SetActive(true);
                pausedScreenController.Hide();
            }
        }

        _gPressed = Input.GetKey(KeyCode.G);
        _pPressed = Input.GetKey(KeyCode.P);
    }

    public void PlayerDied()
    {
        if (!IsPlayerAlive)
            return;

        IsPlayerAlive = false;

        gameOverlayCanvas.gameObject.SetActive(false);

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

    public void ExitPauseMode()
    {
        if (!IsGamePaused) return;
        IsGamePaused = false;
        gameOverlayCanvas.gameObject.SetActive(true);
    }

    public void PlayerFinished()
    {
        IsPlayerAlive = false;
        gameOverlayCanvas.gameObject.SetActive(false);

        _gameMusicSource.Stop();

        var screen = victoryScreen.GetComponent<VictoryScreenController>();
        screen.Show();
    }
}