using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    [Header("Data")]
    public CharacterData[] characters; // Assign your 6 ScriptableObjects here
    private int currentIndex = 0;

    [Header("UI Display")]
    public Text nameText;
    public Text classText;
    public Text statsText;
    public Text abilityText;
    public Image characterPreview;

    void Start()
    {
        UpdateDisplay();
    }

    public void NextCharacter()
    {
        currentIndex = (currentIndex + 1) % characters.Length;
        UpdateDisplay();
        PlayClickSound();
    }

    public void PreviousCharacter()
    {
        currentIndex--;
        if (currentIndex < 0) currentIndex = characters.Length - 1;
        UpdateDisplay();
        PlayClickSound();
    }

    void UpdateDisplay()
    {
        CharacterData c = characters[currentIndex];
        
        nameText.text = c.characterName;
        classText.text = $"{c.gender} {c.classType}";
        statsText.text = $"HP: {c.health} | Armor: {c.armor} | Speed: {c.speed}";
        abilityText.text = $"Bonus: {c.bonusAbilityName}\n{c.bonusDescription}";
        
        // CharacterPreview would be a sprite or a 3D model snapshot
        // characterPreview.sprite = c.menuSprite; 
    }

    public void SelectAndPlay()
    {
        // Save the selection so the Dungeon scene knows who to spawn
        PlayerPrefs.SetInt("SelectedCharID", currentIndex);
        SceneManager.LoadScene("DungeonLevel1"); 
    }

    void PlayClickSound()
    {
        if(SoundManager.Instance != null)
            SoundManager.Instance.PlaySound(SoundManager.Instance.coinPickup);
    }
}