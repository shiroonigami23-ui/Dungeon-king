using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Item
{
    [Header("Item Info (IMPORTANT)")]
    public string name;
    public int ID;
    public bool isSword;
    public bool isItem;
    public bool isBow;
    public Transform heldPoint;
    public Transform pivotPoint;

    [Header("Item GameObject")]
    public GameObject item;
    public GameObject pickUp;
    [Header("Effects")]
    public float damage;
    public float health;
}
