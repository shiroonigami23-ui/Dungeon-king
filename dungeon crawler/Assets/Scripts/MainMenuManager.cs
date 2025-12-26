using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // The exact name of your first gameplay scene (e.g., "SampleScene" or "DungeonLevel1")
    public string firstLevelName;

    public void PlayGame()
    {
        // Load the first level
        SceneManager.LoadScene(firstLevelName);
        // Ensure time isn't paused if they quit from a death screen previously
        Time.timeScale = 1f; 
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}