using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    //Management
    private Hotbar hotbar;
    public GameObject icon;
    public int ID;
    private ItemManager itemmanager;

    //Bobbing
    public float amplitude = 0.5f;
    public float frequency = 1f;
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        hotbar = GameObject.FindGameObjectWithTag("HotBar").GetComponent<Hotbar>();
        itemmanager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
        posOffset = transform.position;
    }

    void Update()
    {
        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            for (int i = 0; i < hotbar.slots.Length; i++)
            {
                if (hotbar.isFull[i] == false)
                {
                    // Add to hotbar
                    hotbar.isFull[i] = true;
                    hotbar.CarriedItemIDs[i] = ID;
                    Instantiate(icon, hotbar.slots[i].transform, false);

                    // Hold in hand if Active
                    if (hotbar.isActive[i] == true)
                    {
                        itemmanager.Hold(hotbar.CarriedItemIDs[i]);
                    }

                    Destroy(gameObject);
                    break; 
                }
            }
        }
    }
}
