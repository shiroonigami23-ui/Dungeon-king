using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ItemData // This defines what an "Item" is in your list
{
    public string name;
    public int ID;
    public GameObject itemPrefab;   // The one held in hand
    public GameObject pickUpPrefab; // The one on the ground
    public Transform pivotPoint;    // Where it rotates
    public Transform heldPoint;     // Where it sits in hand
}

public class ItemManager : MonoBehaviour
{
    [Header("Inventory Data")]
    public ItemData[] Items;
    public GameObject Hand1Item; // This should be the "Hand" child of your Player
    public int HeldLayer;

    [Header("Drop Settings")]
    private GameObject player;
    public float dropOffset = 1.5f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        HeldLayer = LayerMask.NameToLayer("Player");
    }

    public void Hold(int Itemid)
    {
        HideAll(); // Clear hand first

        // Find the item in our list by ID
        ItemData i = Array.Find(Items, data => data.ID == Itemid);

        if (i != null && i.itemPrefab != null)
        {
            // Position the hand/pivot based on the item's specific data
            Transform handPivot = Hand1Item.transform.parent;
            handPivot.rotation = Quaternion.identity;
            
            // Spawn the weapon
            GameObject newWeapon = Instantiate(i.itemPrefab, Hand1Item.transform, false);
            newWeapon.layer = HeldLayer;
            
            Debug.Log("Holding: " + i.name);
        }
    }

    public void Drop(int Itemid)
    {
        ItemData i = Array.Find(Items, data => data.ID == Itemid);

        if (i != null && i.pickUpPrefab != null)
        {
            // Spawn the pickup item on the ground near the player
            Vector2 dropPos = new Vector2(player.transform.position.x + dropOffset, player.transform.position.y);
            Instantiate(i.pickUpPrefab, dropPos, Quaternion.identity);
            
            HideAll();
        }
    }

    public void HideAll()
    {
        // Destroy whatever is currently in the player's hand
        if (Hand1Item.transform.childCount > 0)
        {
            foreach (Transform child in Hand1Item.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}