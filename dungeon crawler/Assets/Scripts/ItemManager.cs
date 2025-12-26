using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class ItemManager : MonoBehaviour
{
    // General Setup
    public Item[] Items;
    private int[] AllItemIDs;
    private GameObject[] AllItems;
    public GameObject Hand1Item;
    public GameObject HandForSword;
    public GameObject HandForBow;
    private int TotalItems;


    // Drop Item
    private GameObject player;
    public float offset;
    public int HeldLayer;

    void Awake()
    {
        TotalItems = Items.Length;
        AllItemIDs = new int[TotalItems];
        for (int i = 0; i < Items.Length; i++)
        {
            AllItemIDs[i] = Items[i].ID;
        }

   /*   foreach (Item i in Items)
        {
            AllItems[i] = i.item;
            
            i.damage = i.damage;
            i.health = i.health;
        } */
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Hold(int Itemid)
    {
        if (Hand1Item.transform.childCount > 0)
        {
            GameObject g = Hand1Item.transform.GetChild(0).gameObject;
            Destroy(g);
        }

        // place new item in hand
        int ID = Itemid;
        Item i = Array.Find(Items, Item => Item.ID == ID);
        Hand1Item.transform.parent.rotation = Quaternion.identity;
        Hand1Item.transform.parent.position = new Vector3(i.pivotPoint.position.x, i.pivotPoint.position.y, 0);
        Hand1Item.transform.position = new Vector3(i.heldPoint.position.x, i.heldPoint.position.y, 0);
        GameObject g2 = Instantiate(i.item, Hand1Item.transform, false);
        g2.layer = HeldLayer;
    }

    public void Drop(int Itemid)
    {
        // Drop Item in Active slot
        int ID = Itemid;
        Item i = Array.Find(Items, Item => Item.ID == ID);

        // Create item in world
        Vector2 playerPos = new Vector2(player.transform.position.x + offset, player.transform.position.y);

        Instantiate(i.pickUp, playerPos, Quaternion.identity);

        // destroy old item in hand
        if (Hand1Item.transform.childCount > 0)
        {
            GameObject g = Hand1Item.transform.GetChild(0).gameObject;
            Destroy(g);
        }
    }

    public void DestroyHeldItem()
    {
        // destroy old item in hand
        if (Hand1Item.transform.childCount > 0)
        {
            GameObject g = Hand1Item.transform.GetChild(0).gameObject;
            Destroy(g);
        }
    }

}
