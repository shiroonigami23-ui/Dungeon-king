using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Run
    [Header("Running")]
    public float speed;
    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    // Dash
    public float dashSpeed;
    private float dashTime;
    public float startDashTime;
    private int moved;

    // Layers
    public int invincibleLayer;
    public int playerLayer;
    
    // Trail
    [Header("Trail")]
    public ParticleSystem dust;

    // Animations
    private Animator anim;

    //Flip
    public bool facingRight;
    private SpriteRenderer sr;

    //Health
    public float health;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = gameObject.GetComponent<Animator>();
        dashTime = startDashTime;
        moved = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Run
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (moved == 0)
        {
            moveVelocity = moveInput.normalized * speed;
        }

        if (moveInput.x != 0 || moveInput.y != 0)
        {
            anim.SetBool("Run", true);
            Trail();
        }
        else
        {
            anim.SetBool("Run", false);
        }

        // Dash

        if (moved == 0)
        {
            if (moveInput.x != 0 && Input.GetKeyDown(KeyCode.Space) || moveInput.y != 0 && Input.GetKeyDown(KeyCode.Space))
            {
                moved = 1;
            }
        }
        else
        {
            if (dashTime <= 0)
            {
                moved = 0;
                dashTime = startDashTime;
                rb.velocity = Vector2.zero;
                gameObject.layer = playerLayer;
            }
            else
            {
                dashTime -= Time.deltaTime;
                if (moved == 1 && dashTime >= 0)
                {
                    rb.AddForce(moveVelocity * dashSpeed * Time.deltaTime);
                    //rb.velocity = new Vector2(moveInput.y * dashSpeed, rb.velocity.x);
                    gameObject.layer = invincibleLayer;
                    Debug.Log("Dashed");
                }
            }
        }


        // Flip Based On Mouse Position and Player Position

        var delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (delta.x >= 0 && !facingRight)
        { // mouse is on right side of player
            transform.localScale = new Vector3(1, 1, 1); // activate looking right
            facingRight = true;
        }
        else if (delta.x < 0 && facingRight)
        { // mouse is on left side of player
            transform.localScale = new Vector3(-1, 1, 1); // activate looking left
            facingRight = false;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    void Trail()
    {
        dust.Play();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
