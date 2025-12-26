using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Stats")]
    public float health = 100f;
    public float maxHealth = 100f;
    public float invincibilityDuration = 1.5f;

    [Header("Physics & Components")]
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Vector2 moveInput;
    private Vector2 moveVelocity;

    [Header("State Bools")]
    private bool isDashing = false;
    private bool canDash = true;
    private bool isInvincible = false;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        health = maxHealth;
    }

    void Update()
    {
        if (isDashing) return; // Freeze input during dash

        // 1. Movement Input
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;

        // 2. Animation
        anim.SetBool("Run", moveInput.sqrMagnitude > 0);

        // 3. Dash Trigger
        if (Input.GetKeyDown(KeyCode.Space) && canDash && moveInput.sqrMagnitude > 0)
        {
            StartCoroutine(PerformDash());
        }

        // 4. Flip Character based on Mouse
        HandleRotation();
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }
    }

    private IEnumerator PerformDash()
    {
        canDash = false;
        isDashing = true;
        isInvincible = true; // Can't be hit while dashing
SoundManager.Instance.PlaySound(SoundManager.Instance.dash);
        rb.velocity = moveInput.normalized * dashSpeed;
        
        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        rb.velocity = Vector2.zero;
        isInvincible = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    void HandleRotation()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x > transform.position.x && !facingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }
        else if (mousePos.x < transform.position.x && facingRight)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible || health <= 0) return;

        health -= damage;
        StartCoroutine(HitFeedback());

        if (health <= 0)
        {
            health = 0;
            if (UIManager.Instance != null) UIManager.Instance.ShowDeathScreen();
            Debug.Log("Game Over");
        }
    }

    private IEnumerator HitFeedback()
    {
        isInvincible = true;
        // Blink effect
        for (float i = 0; i < invincibilityDuration; i += 0.2f)
        {
            sr.color = new Color(1, 0.5f, 0.5f, 0.5f); // Reddish transparent
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
        isInvincible = false;
    }
}