using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadScreenController : MonoBehaviour
{
    public Text scoreText;

    public void Show(int score)
    {
        gameObject.SetActive(true);
        scoreText.text = $"Score: {score}";
        
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