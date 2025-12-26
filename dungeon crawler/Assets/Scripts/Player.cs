using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Character Selection")]
    public CharacterData characterProfile; // Assign the ScriptableObject here

    [Header("Movement Settings")]
    public float speed = 5f;
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Player Stats")]
    public int currentGold = 0;
    public float health = 100f;
    public float maxHealth = 100f;
    public float armor = 2f; 
    public float invincibilityDuration = 1.5f;
    public float damageMultiplier = 1f;

    [Header("Class Specific Variables")]
    private int warriorHitCount = 0;   // For Battle Trance
    private float mageGoldTimer = 0f;  // For Arcane Flow

    [Header("Components")]
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Vector2 moveInput;
    private Vector2 moveVelocity;

    [Header("State Flags")]
    private bool isDashing = false;
    private bool canDash = true;
    private bool isInvincible = false;
    private bool facingRight = true;

    public CharacterData[] allCharacterOptions; // Assign the same 6 objects here too

void Start()
{
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    sr = GetComponent<SpriteRenderer>();

    // 1. SAFETY CHECK: Make sure you didn't leave the list empty in the Inspector
    if (allCharacterOptions == null || allCharacterOptions.Length == 0)
    {
        Debug.LogError("CRITICAL: You forgot to drag the CharacterData assets into the Player script in the Inspector!");
        return; 
    }

    // 2. LOAD THE SELECTION
    int selectedID = PlayerPrefs.GetInt("SelectedCharID", 0);
    
    // Safety check for ID range
    if (selectedID >= allCharacterOptions.Length) selectedID = 0;

    characterProfile = allCharacterOptions[selectedID];

    if (characterProfile != null)
    {
        maxHealth = characterProfile.health;
        speed = characterProfile.speed;
        armor = characterProfile.armor;
        damageMultiplier = characterProfile.damageMultiplier;
        
        // Optional: Change sprite to match selection
        // sr.sprite = characterProfile.characterSprite;
    }

    health = maxHealth;
}

    void Update()
    {
        if (isDashing) return;

        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;

        if (anim != null) anim.SetBool("Run", moveInput.sqrMagnitude > 0);

        if (Input.GetKeyDown(KeyCode.Space) && canDash && moveInput.sqrMagnitude > 0)
        {
            StartCoroutine(PerformDash());
        }

        HandleRotation();
        HandleClassPassives();
    }

    private void HandleClassPassives()
    {
        if (characterProfile == null) return;

        // MAGE: Arcane Flow - Regenerate 1 Gold (Mana) every 10 seconds
        if (characterProfile.classType == CharacterData.ClassType.Mage)
        {
            mageGoldTimer += Time.deltaTime;
            if (mageGoldTimer >= 10f)
            {
                currentGold++;
                mageGoldTimer = 0;
            }
        }
    }

    // Called by Sword.cs or Arrow.cs when a hit is successful
    public void OnSuccessfulHit()
    {
        if (characterProfile == null) return;

        // WARRIOR: Battle Trance - Every 5th hit heals 10% HP
        if (characterProfile.classType == CharacterData.ClassType.Warrior)
        {
            warriorHitCount++;
            if (warriorHitCount >= 5)
            {
                Heal(maxHealth * 0.10f);
                warriorHitCount = 0;
                Debug.Log("Warrior Battle Trance: Healed!");
            }
        }
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
        isInvincible = true; 
        if (SoundManager.Instance != null) SoundManager.Instance.PlaySound(SoundManager.Instance.dash);

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
        transform.localScale = (mousePos.x > transform.position.x) ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1);
        facingRight = (mousePos.x > transform.position.x);
    }

    public void TakeDamage(float damage)
    {
        if (isInvincible || health <= 0) return;

        // KNIGHT: Bastion - 15% Chance to Block completely
        if (characterProfile != null && characterProfile.classType == CharacterData.ClassType.Knight)
        {
            if (Random.value < 0.15f)
            {
                Debug.Log("Knight Bastion: Blocked!");
                return; 
            }
        }

        float finalDamage = Mathf.Max(damage - armor, 1);
        health -= finalDamage;

        StartCoroutine(HitFeedback());

        if (health <= 0)
        {
            health = 0;
            if (UIManager.Instance != null) UIManager.Instance.ShowDeathScreen();
        }
    }

    public void Heal(float amount)
    {
        health = Mathf.Min(health + amount, maxHealth);
        StartCoroutine(HealFeedback());
    }

    private IEnumerator HitFeedback()
    {
        isInvincible = true;
        for (float i = 0; i < invincibilityDuration; i += 0.2f)
        {
            sr.color = new Color(1, 0.5f, 0.5f, 0.5f);
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
        isInvincible = false;
    }

    private IEnumerator HealFeedback()
    {
        sr.color = Color.green;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }
}