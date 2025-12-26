using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject shopUI;      // Drag the Shop Panel here
    public Text goldDisplay;       // Drag the Text object that shows Gold here
    public Text upgradeInfoText;   // Optional: Text to show current costs

    [Header("Upgrade Settings")]
    public int damageUpgradeCost = 50;
    public int armorUpgradeCost = 75;

    private Player player;
    private Sword playerSword;

    void Start()
    {
        // Find the player and their sword
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<Player>();
            // Look for the sword in the player's children
            playerSword = playerObj.GetComponentInChildren<Sword>();
        }
        
        // Hide shop at start
        if (shopUI != null) shopUI.SetActive(false);
    }

    void Update()
    {
        // Keep the gold display updated while shop is open
        if (shopUI != null && shopUI.activeSelf && player != null)
        {
            goldDisplay.text = "Gold: " + player.currentGold;
            if (upgradeInfoText != null)
            {
                upgradeInfoText.text = $"Dmg Cost: {damageUpgradeCost} | Armor Cost: {armorUpgradeCost}";
            }
        }
    }

    public void OpenShop()
    {
        shopUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
        Time.timeScale = 1f; // Unpause the game
    }

    public void UpgradeDamage()
    {
        if (player.currentGold >= damageUpgradeCost)
        {
            player.currentGold -= damageUpgradeCost;
            
            // Apply damage boost to sword
            if (playerSword == null) playerSword = FindObjectOfType<Sword>();
            if (playerSword != null)
            {
                playerSword.attackDamage += 5;
                damageUpgradeCost += 25; // Increase price for next time
                Debug.Log("Damage Upgraded! New Damage: " + playerSword.attackDamage);
            }
        }
        else
        {
            Debug.Log("Not enough gold for Damage!");
        }
    }

    public void UpgradeArmor()
    {
        if (player.currentGold >= armorUpgradeCost)
        {
            player.currentGold -= armorUpgradeCost;
            player.armor += 1;
            armorUpgradeCost += 50; // Increase price
            Debug.Log("Armor Upgraded! Current Armor: " + player.armor);
        }
        else
        {
            Debug.Log("Not enough gold for Armor!");
        }
    }
}