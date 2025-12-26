using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Player UI")]
    public Slider playerHealthBar;
    public GameObject deathScreen;

    [Header("Boss UI")]
    public GameObject bossUIContainer; // The parent object holding the slider and text
    public Slider bossHealthBar;
    public Text bossNameText;

    private Player player;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        
        if (playerHealthBar != null)
        {
            playerHealthBar.maxValue = player.maxHealth;
            playerHealthBar.value = player.health;
        }

        // Hide boss UI at start
        if(bossUIContainer != null) bossUIContainer.SetActive(false);
        if(deathScreen != null) deathScreen.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;
        playerHealthBar.value = player.health;
    }

    // --- Boss Functions Called by Boss Script ---
    public void ActivateBossUI(string bossName, float maxHealth)
    {
        bossUIContainer.SetActive(true);
        bossNameText.text = bossName;
        bossHealthBar.maxValue = maxHealth;
        bossHealthBar.value = maxHealth;
    }

    public void UpdateBossHealth(float currentHealth)
    {
        bossHealthBar.value = currentHealth;
    }

    public void DeactivateBossUI()
    {
        bossUIContainer.SetActive(false);
    }
    // -------------------------------------------

    public void ShowDeathScreen()
    {
        deathScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    // Updated to load the Main Menu instead of just restarting
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Make sure your menu scene is named exactly this
    }
}