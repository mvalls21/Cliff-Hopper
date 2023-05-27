using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    private AudioSource _audioSource;

    private bool _gameStarting = false;

    public GameObject backgroundMusicObject;

    private AudioSource _backgroundSource;

    public void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _backgroundSource = backgroundMusicObject.GetComponent<AudioSource>();
    }

    public void StartInfiniteMode()
    {
        if (_gameStarting) return;
        
        LevelGeneration.InfiniteGeneration = true;
        StartCoroutine(StartGame());
    }

    public void StartEvaluationMode()
    {
        if (_gameStarting) return;
        
        LevelGeneration.InfiniteGeneration = false;
        StartCoroutine(StartGame());
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }

    private IEnumerator StartGame()
    {
        _gameStarting = true;
        
        _audioSource.Play();
        _backgroundSource.Pause();
        
        yield return new WaitForSeconds(4.0f);

        SceneManager.LoadScene("Scenes/Game");
    }
}