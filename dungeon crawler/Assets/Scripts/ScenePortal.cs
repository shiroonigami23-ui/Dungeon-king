using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{
    [Header("Targeting")]
    public string sceneToLoad; // Name of the scene (Case Sensitive!)
    public Vector2 spawnPosition; // Where the player appears in the next scene

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Optional: Save player stats before leaving
            // PlayerPrefs.SetFloat("PlayerHealth", other.GetComponent<Player>().health);
            
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}