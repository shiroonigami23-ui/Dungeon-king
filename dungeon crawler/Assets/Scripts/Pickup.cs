using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType { HotbarItem, Gold, Health }

    [Header("Type Settings")]
    public PickupType type;
    public int itemID; // Used for Hotbar items
    public float value = 1f; // Used for Gold or Health amount
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
        // Try to find the systems. If they don't exist, the game won't crash
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
            switch (type)
            {
                case PickupType.HotbarItem:
                    HandleHotbarPickup();
                    break;
                case PickupType.Gold:
                SoundManager.Instance.PlaySound(SoundManager.Instance.coinPickup);
                    Debug.Log("Gained Gold: " + value);
                    // Add to your MoneyManager here
                    Destroy(gameObject);
                    break;
                case PickupType.Health:
                    col.GetComponent<Player>().health += value;
                    Destroy(gameObject);
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