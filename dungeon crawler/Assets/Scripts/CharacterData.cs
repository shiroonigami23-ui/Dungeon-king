using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacter", menuName = "DungeonKing/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public enum ClassType { Warrior, Knight, Mage }
    public enum Gender { Male, Female }
    public ClassType classType;
    public Gender gender;

    [Header("Base Stats")]
    public float health = 100f;
    public float speed = 5f;
    public float armor = 2f;
    public float damageMultiplier = 1f;

    [Header("Abilities")]
    public string bonusAbilityName;
    public string bonusDescription;
    
    [Header("Hidden Unlocks")]
    public string[] achievementAbilities; // 4-5 options
}