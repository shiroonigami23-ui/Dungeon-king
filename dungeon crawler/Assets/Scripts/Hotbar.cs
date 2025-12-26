using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    // General SetUp
    public bool[] isFull;
    public GameObject[] slots;
    private float lastMouseDelta;

    // Coloring and Active
    public bool[] isActive;
    public Color[] colors;
    public int ActiveID;

    // Hand1Item Hold
    public GameObject Hand1;
    public int[] CarriedItemIDs;

    //Drop Item
    public ItemManager itemmanager;

    // Start is called before the first frame update
    void Start()
    {
        slots[0].GetComponent<Image>().color = colors[0];
        isActive[0] = true;
        slots[0].GetComponent<Animator>().SetBool("isActive", true);
        ActiveID = 0;

        // Hold Active Slot Item
        if (isFull[0] == true)
        {
            itemmanager.Hold(CarriedItemIDs[ActiveID]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Input

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SlotChanged(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SlotChanged(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SlotChanged(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SlotChanged(3);
        }

        //wheel goes up
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            if (ActiveID < 3)
                SlotChanged(ActiveID + 1);
            else
                SlotChanged(0); 
            
        }//wheel goes down
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            if (ActiveID > 0)
                SlotChanged(ActiveID - 1);
            else
                SlotChanged(3);
        }

        // Drop Items

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (isFull[ActiveID] == true)
            {
                DropItem(slots[ActiveID]);
                isFull[ActiveID] = false;
            }
        }
    }

    void SlotChanged(int slot)
    {
        if (isFull[slot] == true)
        {
            for (int i = 0; i < isActive.Length; i++)
            {
                if (i == slot)
                {
                    slots[slot].GetComponent<Image>().color = colors[0];
                    slots[slot].GetComponent<Animator>().SetBool("isActive", true);
                    isActive[slot] = true;
                    ActiveID = slot;

                    // Hold Item
                    if (isFull[slot] == true)
                    {
                        itemmanager.Hold(CarriedItemIDs[ActiveID]);
                    }
                }
                else
                {
                    slots[i].GetComponent<Image>().color = colors[1];
                    isActive[i] = false;
                    slots[i].GetComponent<Animator>().SetBool("isActive", false);
                }
            }
        }
        else
        {
            for (int i = 0; i < isActive.Length; i++)
            {
                if (i == slot)
                {
                    slots[slot].GetComponent<Image>().color = colors[0];
                    isActive[slot] = true;
                    ActiveID = slot;
                    slots[slot].GetComponent<Animator>().SetBool("isActive", true);
                    itemmanager.DestroyHeldItem();
                }
                else
                {
                    slots[i].GetComponent<Image>().color = colors[1];
                    isActive[i] = false;
                    slots[i].GetComponent<Animator>().SetBool("isActive", false);
                }
            }
        }
    }

    void DropItem(GameObject Active)
    {
        // Get rid of object icon in active slot

        if (isFull[ActiveID] == true)
        {
            GameObject g = Active.transform.GetChild(1).gameObject;
            Destroy(g);
            isFull[ActiveID] = false;

            // Call Drop Item Function()
            itemmanager.Drop(CarriedItemIDs[ActiveID]);

            CarriedItemIDs[ActiveID] = 0;
        }
    }
}
