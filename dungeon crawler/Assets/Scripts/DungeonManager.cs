using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonManager : MonoBehaviour
{
    [Header("Level Settings")]
    public string nextLevelName;
    public GameObject exitPortal;
    
    private bool levelComplete = false;

    void Start()
    {
        if(exitPortal != null) exitPortal.SetActive(false);
    }

    void Update()
    {
        // Check if all enemies are dead
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0 && !levelComplete)
        {
            CompleteLevel();
        }
    }

    void CompleteLevel()
    {
        levelComplete = true;
        Debug.Log("Level Clear! Portal Opened.");
        if(exitPortal != null) exitPortal.SetActive(true);
    }
}