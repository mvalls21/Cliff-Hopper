using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VictoryScreenController : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void PlayAgain()
    {
        LevelGeneration.InfiniteGeneration = false;
        SceneManager.LoadScene("Scenes/Game");
    }

    public void InifniteMode()
    {
        LevelGeneration.InfiniteGeneration = true;
        SceneManager.LoadScene("Scenes/Game");
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