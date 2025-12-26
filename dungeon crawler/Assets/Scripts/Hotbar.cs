using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [Header("General Setup")]
    public bool[] isFull;
    public GameObject[] slots;
    public int[] CarriedItemIDs;
    public bool[] isActive;
    public int ActiveID;

    [Header("Visuals")]
    public Color[] colors; // colors[0] = Active, colors[1] = Inactive
    public ItemManager itemmanager;

    void Start()
    {
        // Set first slot as active at start
        SlotChanged(0);
    }

    void Update()
    {
        // 1. Number Key Input
        if (Input.GetKeyDown(KeyCode.Alpha1)) SlotChanged(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SlotChanged(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SlotChanged(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SlotChanged(3);

        // 2. Mouse Wheel Input (Scrolling)
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (scroll > 0) // Wheel Up
        {
            int nextSlot = (ActiveID >= 3) ? 0 : ActiveID + 1;
            SlotChanged(nextSlot);
        }
        else if (scroll < 0) // Wheel Down
        {
            int nextSlot = (ActiveID <= 0) ? 3 : ActiveID - 1;
            SlotChanged(nextSlot);
        }

        // 3. Drop Item Input (Q)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isFull[ActiveID])
            {
                DropItem();
            }
        }
    }

    public void SlotChanged(int slot)
    {
        ActiveID = slot;

        for (int i = 0; i < slots.Length; i++)
        {
            bool isThisSlot = (i == slot);
            isActive[i] = isThisSlot;

            // Visuals: Color and Animation
            slots[i].GetComponent<Image>().color = isThisSlot ? colors[0] : colors[1];
            
            Animator slotAnim = slots[i].GetComponent<Animator>();
            if (slotAnim != null) slotAnim.SetBool("isActive", isThisSlot);
        }

        // Physically update the item in the Player's hand
        UpdateHeldItem();
    }

    void UpdateHeldItem()
    {
        if (isFull[ActiveID])
        {
            itemmanager.Hold(CarriedItemIDs[ActiveID]);
        }
        else
        {
            itemmanager.HideAll(); // From our new ItemManager
        }
    }

    void DropItem()
    {
        // 1. Remove the Icon from the UI (usually child index 1)
        if (slots[ActiveID].transform.childCount > 1)
        {
            Destroy(slots[ActiveID].transform.GetChild(1).gameObject);
        }

        // 2. Tell ItemManager to spawn the item on the ground
        itemmanager.Drop(CarriedItemIDs[ActiveID]);

        // 3. Clear data
        isFull[ActiveID] = false;
        CarriedItemIDs[ActiveID] = -1;
        itemmanager.HideAll();
    }
}