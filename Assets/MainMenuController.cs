using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenuScreen;

    public Canvas instruccionsScreen;

    private AudioSource _audioSource;

    private bool _gameStarting = false;

    public GameObject backgroundMusicObject;

    private AudioSource _backgroundSource;

    public GameObject cacodemon;
    public GameObject directionChange;
    public GameObject spikes;
    public GameObject coin;

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

    public void OpenInstructions()
    {
        mainMenuScreen.SetActive(false);
        instruccionsScreen.gameObject.SetActive(true);

        cacodemon.SetActive(true);
        spikes.SetActive(true);
        directionChange.SetActive(true);
        coin.SetActive(true);
    }

    public void ExitInstructions()
    {
        mainMenuScreen.SetActive(true);
        instruccionsScreen.gameObject.SetActive(false);

        cacodemon.SetActive(false);
        spikes.SetActive(false);
        directionChange.SetActive(false);
        coin.SetActive(false);
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