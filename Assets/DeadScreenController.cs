using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadScreenController : MonoBehaviour
{
    public Text scoreText;

    public Text maxScoreText;

    public void Show(int score, int maxScore)
    {
        gameObject.SetActive(true);

        scoreText.text = $"Score: {score}";
        maxScoreText.text = $"Max Score: {maxScore}";
        
        var menuMusicSource  = GetComponent<AudioSource>();
        menuMusicSource.Play();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }
}