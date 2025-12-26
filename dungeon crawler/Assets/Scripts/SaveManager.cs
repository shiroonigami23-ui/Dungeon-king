using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private string savePath;
    private Player player;
    private Sword sword;

    void Awake()
    {
        savePath = Application.persistentDataPath + "/dungeon_king_save.json";
        player = FindObjectOfType<Player>();
        sword = FindObjectOfType<Sword>();
    }

    public void SaveGame()
    {
        PlayerData data = new PlayerData();
        data.gold = player.currentGold;
        data.armor = player.armor;
        data.health = player.health;
        data.swordDamage = sword.attackDamage;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game Saved to: " + savePath);
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            player.currentGold = data.gold;
            player.armor = data.armor;
            player.health = data.health;
            sword.attackDamage = data.swordDamage;

            Debug.Log("Game Loaded!");
        }
        else
        {
            Debug.Log("No save file found.");
        }
    }
}