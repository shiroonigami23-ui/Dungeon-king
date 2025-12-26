using UnityEngine;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    [Header("XP & Leveling")]
    public int level = 1;
    public float currentXP = 0;
    public float xpToNextLevel = 100;

    [Header("Achievement Tracking")]
    public int totalKills = 0;
    public int hiddenAbilityUnlockThreshold = 100;
    private bool hiddenAbilityUnlocked = false;

    [Header("Unique Divine ID")]
    public string uniquePlayerID; // Set this in Inspector or via Script
    public string divineAbilityName = "Locked";
    public bool divineAbilityUnlocked = false;

    private Player player;
    private Sword sword;

    void Start()
    {
        player = GetComponent<Player>();
        sword = GetComponentInChildren<Sword>();

        // Fallback: If no ID is set, use the Device ID
        if (string.IsNullOrEmpty(uniquePlayerID))
        {
            uniquePlayerID = SystemInfo.deviceUniqueIdentifier;
        }
    }

    // --- XP SYSTEM ---
    public void AddXP(float amount)
    {
        currentXP += amount;
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
        
        // Update UI (if you have one)
        if (UIManager.Instance != null) UIManager.Instance.UpdateXPBar(currentXP, xpToNextLevel);
    }

    void LevelUp()
    {
        level++;
        currentXP -= xpToNextLevel;
        xpToNextLevel *= 1.3f; // Scaling difficulty

        // Permanent Stat Boosts
        if (player != null)
        {
            player.maxHealth += 15;
            player.health = player.maxHealth;
            player.armor += 0.5f;
            player.speed += 0.2f;
        }

        Debug.Log($"LEVEL UP! You are now Level {level}");
        
        // Play Level Up Sound
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound(SoundManager.Instance.coinPickup); 
    }

    // --- ACHIEVEMENT & HIDDEN ABILITY SYSTEM ---
    public void AddKill()
    {
        totalKills++;
        
        // Check for Hidden Achievement Unlock
        if (totalKills >= hiddenAbilityUnlockThreshold && !hiddenAbilityUnlocked)
        {
            UnlockHiddenAbility();
        }

        // Check for the "Secret" Divine Unlock
        if (totalKills >= 500 && !divineAbilityUnlocked)
        {
            UnlockDivineAbility();
        }
    }

    void UnlockHiddenAbility()
    {
        hiddenAbilityUnlocked = true;
        // Logic: Grant a character-dependent bonus (e.g., extra crit or speed)
        player.speed += 1.5f;
        Debug.Log("ACHIEVEMENT UNLOCKED: Fleet Footed (Permanent Speed Boost)");
    }

    // --- THE UNIQUE DIVINE SEED (OP Logic) ---
    void UnlockDivineAbility()
{
    divineAbilityUnlocked = true;

    // 1. SEEDING: This ensures the "Random" result is the same for this ID every time
    // But different from your friend's ID.
    Random.InitState(uniquePlayerID.GetHashCode());

    // 2. GENERATE THE OP POWER
    int roll = Random.Range(0, 3);
    float multiplier = Random.Range(2.5f, 4.0f); // Massive OP multiplier

    switch (roll)
    {
        case 0:
            divineAbilityName = "Reaper's Verdict";
            if (sword != null) sword.attackDamage *= multiplier;
            break;
        case 1:
            divineAbilityName = "Titan's Soul";
            if (player != null)
            {
                player.maxHealth *= multiplier;
                player.health = player.maxHealth;
            }
            break;
        case 2:
            divineAbilityName = "Chrono-Step";
            if (player != null) player.dashCooldown /= multiplier;
            break;
    }

    // 3. APPLY THE GLOW EFFECT (Visual Proof of Power)
    // This makes the player sprite shine with a golden aura
    SpriteRenderer playerSR = GetComponent<SpriteRenderer>();
    if (playerSR != null)
    {
        playerSR.material.EnableKeyword("_EMISSION"); // Turn on the light
        playerSR.material.SetColor("_EmissionColor", Color.gold * 2.5f);
    }

    // 4. TRIGGER UI POPUP
    if (AchievementUI.Instance != null)
    {
        AchievementUI.Instance.ShowAchievement("DIVINE AWAKENING", $"Unique Soul Skill: {divineAbilityName} Awakened!");
    }

    Debug.Log($"DIVINE UNLOCK: {divineAbilityName} (Multiplier: x{multiplier})");
}
}