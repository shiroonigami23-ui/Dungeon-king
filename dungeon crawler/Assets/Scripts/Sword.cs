using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // General Setup
    public Transform hand1;
    public Transform hand2;
    public float rotation;
    private Vector3 mousePos;
    private GameObject Player;

    // Sorting and Flipping
    private SpriteRenderer sr; 
    public int orderInLayer;
    public bool facingRight;
    private int currentFlip;

    // Attacking
    public float speed;
    private bool Attacking;
    public Transform target;
    public float timer;
    private int attack;

    // Start is called before the first frame update
    void Start()
    {
        hand1 = gameObject.transform.parent.parent;
        hand2 = GameObject.FindGameObjectWithTag("SwordPosition2").GetComponent<Transform>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player");
        facingRight = true;
        Attacking = false;
        attack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Aim stuff
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector3(mousePosition.x, mousePosition.y, 0);

        // Rotate Based On mousePos
        hand1.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(mousePos.y - hand1.position.y, mousePos.x - hand1.position.x) * Mathf.Rad2Deg - 90);
        hand2.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(mousePos.y - hand2.position.y, mousePos.x - hand2.position.x) * Mathf.Rad2Deg - 90);

        //(CREATE WAY TO ONLY ROTATE ON CERTAIN AXIS AT CERTAIN INSTANCES)

        // Stab (Attack)
        if (Input.GetKeyDown(KeyCode.Mouse0) && Attacking == false)
        {
            Attacking = true;
            if (attack == 0)
            {
                transform.position = hand2.transform.position;
                transform.localScale = new Vector3(-1, -1, 1);
                attack = 1;
            }
            else
            {
                transform.position = hand1.transform.GetChild(0).GetComponent<Transform>().position;
                transform.localScale = new Vector3(1, 1, 1);
                attack = 0;
            }
            timer = 0.1f;
        }

        if (Attacking == true)
        {
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
            }
            else if (timer <= 0 && timer > -0.1f)
            {
                timer -= Time.deltaTime;
            }
            else if (timer <= -0.1f )
            {
                Attacking = false;
            }
        }

        // Change Order Based On mousePos and Player Position

        var delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Player.transform.position;

        if (delta.x >= 0 && !facingRight)
        { // mouse is on right side of player
            facingRight = true;
            orderInLayer = 2;
            sr.sortingOrder = orderInLayer;
        }
        else if (delta.x < 0 && facingRight)
        { // mouse is on left side
            facingRight = false;
            orderInLayer = 2;
            //    orderInLayer = 0;
            sr.sortingOrder = orderInLayer;
        } 
    }
}
