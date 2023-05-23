using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartInfiniteMode()
    {
        LevelGeneration.InfiniteGeneration = true;
        SceneManager.LoadScene("Scenes/Game");
    }

    public void StartEvaluationMode()
    {
        LevelGeneration.InfiniteGeneration = false;
        SceneManager.LoadScene("Scenes/Game");
    }

    public void Quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
    }
}