using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Boss Stats")]
    public string bossName = "Slime King";
    public float maxHealth = 300f;
    private float currentHealth;
    public float moveSpeed = 1.5f;
    public float attackDamage = 15f;

    [Header("Phase 2 Settings")]
    public GameObject fireballPrefab;
    public float fireballInterval = 3f;
    private float fireballTimer;
    private bool isPhaseTwo = false;

    [Header("Minion Spawning")]
    public GameObject minionPrefab;
    public float spawnInterval = 5f;
    private float spawnTimer;

    [Header("Loot & Victory")]
    public GameObject victoryPortalPrefab;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        GameObject pObj = GameObject.FindGameObjectWithTag("Player");
        if (pObj != null) player = pObj.transform;

        if (UIManager.Instance != null)
            UIManager.Instance.ActivateBossUI(bossName, maxHealth);
    }

    void Update()
    {
        if (player == null || isDead) return;

        // Standard Movement
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
        sr.flipX = (direction.x > 0);

        // Standard Spawning
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnMinion();
            spawnTimer = 0;
        }

        // --- PHASE 2 ATTACK PATTERN ---
        if (isPhaseTwo)
        {
            fireballTimer += Time.deltaTime;
            if (fireballTimer >= fireballInterval)
            {
                ShootFireballCircle();
                fireballTimer = 0;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (UIManager.Instance != null) UIManager.Instance.UpdateBossHealth(currentHealth);

        CheckPhaseTransition();
        StartCoroutine(HitFlash());

        if (currentHealth <= 0) Die();
    }

    void CheckPhaseTransition()
    {
        // Trigger Phase 2 at 50% Health
        if (!isPhaseTwo && currentHealth <= maxHealth / 2)
        {
            isPhaseTwo = true;
            StartCoroutine(EnterPhaseTwo());
        }
    }

    IEnumerator EnterPhaseTwo()
    {
        Debug.Log("BOSS ENRAGED! PHASE 2 START.");
        moveSpeed *= 1.5f; // Get faster
        attackDamage += 10; // Hit harder

        // Visual: Grow 2x bigger over 2 seconds
        Vector3 targetScale = transform.localScale * 2f;
        float elapsed = 0;
        while (elapsed < 2f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, elapsed / 2f);
            sr.color = Color.Lerp(Color.white, Color.red, elapsed / 2f); // Turn red
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound(SoundManager.Instance.dash); // Use a loud sound for roar
    }

    void ShootFireballCircle()
    {
        // Shoots 8 fireballs in a 360-degree circle
        for (int i = 0; i < 360; i += 45)
        {
            Quaternion rot = Quaternion.Euler(0, 0, i);
            Instantiate(fireballPrefab, transform.position, rot);
        }
    }

    void SpawnMinion()
    {
        Vector3 spawnPos = transform.position + (Vector3)Random.insideUnitCircle * 3f;
        Instantiate(minionPrefab, spawnPos, Quaternion.identity);
    }

    IEnumerator HitFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = isPhaseTwo ? Color.red : Color.white; // Stay red if in Phase 2
    }

    void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        if (UIManager.Instance != null) UIManager.Instance.DeactivateBossUI();
        if (victoryPortalPrefab != null) Instantiate(victoryPortalPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject, 0.5f);
    }

    private void OnCollisionEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(attackDamage);
        }
    }
}