using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPickUp : MonoBehaviour
{
    public int ID;
    private MoneyManager moneymanager;


    void Start()
    {
        moneymanager = GameObject.FindGameObjectWithTag("MoneyManager").GetComponent<MoneyManager>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            moneymanager.AddMoney(ID);
            Destroy(gameObject);
        }
    }
}
