using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Boss Stats")]
    public string bossName = "Giant Slime King";
    public float maxHealth = 200f;
    public float currentHealth;
    public float moveSpeed = 1.5f;
    public float attackDamage = 15f;

    [Header("Loot on Death")]
    public GameObject victoryPortalPrefab; // Spawns when boss dies

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        
        // 1. Tell UI to turn on the big bar
        UIManager.Instance.ActivateBossUI(bossName, maxHealth);
    }

    void Update()
    {
        if (player == null || isDead) return;

        // Basic Chase Logic (You can make this more complex later)
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
        sr.flipX = direction.x > 0; // Flip based on direction
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        
        // 2. Update the UI bar
        UIManager.Instance.UpdateBossHealth(currentHealth);
        StartCoroutine(HitFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        
        // 3. Hide the UI bar
        UIManager.Instance.DeactivateBossUI();

        // Spawn victory portal/loot
        if (victoryPortalPrefab != null)
        {
            Instantiate(victoryPortalPrefab, transform.position, Quaternion.identity);
        }
        
        Debug.Log(bossName + " Defeated!");
        Destroy(gameObject, 0.5f); // Give time for a death animation if you have one
    }

    IEnumerator HitFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(attackDamage);
        }
    }
}