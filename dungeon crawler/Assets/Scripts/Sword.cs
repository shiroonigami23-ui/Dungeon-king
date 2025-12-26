using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [Header("Setup")]
    public Transform hand1;
    public Transform hand2;
    private SpriteRenderer sr; 
    private GameObject player;
    private CameraFollow camScript;

    [Header("Combat Stats")]
    public float attackDamage = 15f;
    public float attackCooldown = 0.25f;
    public float attackRange = 1.0f;
    public float knockbackForce = 4f;

    [Header("Detection")]
    public LayerMask enemyLayer;
    public Transform attackPoint; // Put an empty GameObject at the sword tip

    private bool isAttacking = false;
    private bool facingRight = true;
    private int currentHand = 0; // 0 for hand1, 1 for hand2

    void Start()
    {
        // Automatically find components
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        camScript = Camera.main.GetComponent<CameraFollow>();
        
        // Setup hands based on hierarchy if not assigned
        if(hand1 == null) hand1 = transform.parent;
    }

    void Update()
    {
        HandleAiming();

        // Attack trigger
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking)
        {
            StartCoroutine(PerformAttack());
        }
    }

    void HandleAiming()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - player.transform.position;

        // Rotate the active pivot towards the mouse
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        hand1.rotation = Quaternion.Euler(0, 0, angle - 90);
        if(hand2 != null) hand2.rotation = Quaternion.Euler(0, 0, angle - 90);

        // Sorting: Sword goes behind player when aiming "Up"
        sr.sortingOrder = (direction.y > 0) ? 1 : 3;

        // Flip character scale based on mouse (logic similar to your original)
        if (direction.x >= 0 && !facingRight) { facingRight = true; }
        else if (direction.x < 0 && facingRight) { facingRight = false; }
    }

    IEnumerator PerformAttack()
    {
        isAttacking = true;
SoundManager.Instance.PlaySound(SoundManager.Instance.swordSwing);
        // 1. Switch hands/positions for that "alternating" feel
        if (currentHand == 0)
        {
            transform.position = hand1.position;
            transform.localScale = new Vector3(1, 1, 1);
            currentHand = 1;
        }
        else
        {
            if(hand2 != null) transform.position = hand2.position;
            transform.localScale = new Vector3(-1, 1, 1);
            currentHand = 0;
        }

        // 2. Detect Enemies in a circle at the sword tip
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        // 3. Apply Damage and Effects
        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(attackDamage);
                
                // Trigger the Juice
                if(camScript != null) camScript.Shake(0.1f, 0.15f);
                
                // Apply Knockback to enemy
                Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
                if(enemyRb != null)
                {
                    Vector2 knockDirection = (enemy.transform.position - player.transform.position).normalized;
                    enemyRb.AddForce(knockDirection * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    // Help visualize the hit zone in Unity Editor
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}