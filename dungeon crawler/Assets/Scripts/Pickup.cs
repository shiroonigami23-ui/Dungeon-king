using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType { HotbarItem, Gold, Health }

    [Header("Type Settings")]
    public PickupType type;
    public int itemID; // Used for Hotbar items
    public float value = 1f; // Amount of Gold or Health
    public GameObject icon; // The UI icon for the hotbar

    [Header("Management (Auto-filled)")]
    private Hotbar hotbar;
    private ItemManager itemmanager;

    [Header("Bobbing Animation")]
    public float amplitude = 0.2f;
    public float frequency = 1.5f;
    private Vector3 posOffset;

    void Start()
    {
        GameObject hbObj = GameObject.FindGameObjectWithTag("HotBar");
        if(hbObj != null) hotbar = hbObj.GetComponent<Hotbar>();

        GameObject imObj = GameObject.FindGameObjectWithTag("ItemManager");
        if(imObj != null) itemmanager = imObj.GetComponent<ItemManager>();

        posOffset = transform.position;
    }

    void Update()
    {
        // Smooth floating animation
        Vector3 tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.time * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Player playerScript = col.GetComponent<Player>();

            switch (type)
            {
                case PickupType.Gold:
                    playerScript.currentGold += (int)value;
                    if(SoundManager.Instance != null) SoundManager.Instance.PlaySound(SoundManager.Instance.coinPickup);
                    Destroy(gameObject);
                    break;

                case PickupType.Health:
                    // Only pick up if the player actually needs health
                    if (playerScript.health < playerScript.maxHealth)
                    {
                        playerScript.Heal(value);
                        // Reuse coin sound or add a 'drink' sound later
                        if(SoundManager.Instance != null) SoundManager.Instance.PlaySound(SoundManager.Instance.coinPickup);
                        Destroy(gameObject);
                    }
                    break;

                case PickupType.HotbarItem:
                    HandleHotbarPickup();
                    break;
            }
        }
    }

    private void HandleHotbarPickup()
    {
        if (hotbar == null) return;

        for (int i = 0; i < hotbar.slots.Length; i++)
        {
            if (hotbar.isFull[i] == false)
            {
                hotbar.isFull[i] = true;
                hotbar.CarriedItemIDs[i] = itemID;
                if(icon != null) Instantiate(icon, hotbar.slots[i].transform, false);

                if (hotbar.isActive[i] == true && itemmanager != null)
                {
                    itemmanager.Hold(hotbar.CarriedItemIDs[i]);
                }

                Destroy(gameObject);
                break; 
            }
        }
    }
}