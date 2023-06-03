using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausedScreenController : MonoBehaviour
{
    public Text currentScoreText;

    public Text maxScoreText;

    public GameObject gameManagerObject;

    public void Show(int currentScore, int maxScore)
    {
        gameObject.SetActive(true);

        currentScoreText.text = $"Current Score: {currentScore}";
        maxScoreText.text = $"Max Score: {maxScore}";
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Continue()
    {
        var gameManager = gameManagerObject.GetComponent<GameManager>();
        gameManager.ExitPauseMode();
        
        gameObject.SetActive(false);
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