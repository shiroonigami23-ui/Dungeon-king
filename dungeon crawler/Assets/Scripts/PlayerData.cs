using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    // CORE STATS
    public int gold;
    public float currentHealth;
    public float maxHealth;
    public float armor;
    public float swordDamage;

    // RPG PROGRESSION
    public int level;
    public float currentXP;
    public int totalKills;

    // ABILITY UNLOCKS
    public bool hiddenAbilityUnlocked;
    public bool divineAbilityUnlocked;
    public string divineAbilityName;
    
    // UNIQUE ID (To ensure the Divine Seed stays consistent)
    public string uniquePlayerID;
}