using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float health = 30f;
    public float speed = 2f;
    public float chaseRange = 7f;
    public float attackDamage = 5f;
    public float xpReward = 20f;

    [Header("Components")]
    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    [Header("Loot & Effects")]
    public GameObject coinPrefab;      // Drag your Gold Coin prefab here
    public GameObject miniSlimePrefab; // Drag your Mini Slime prefab here
    public GameObject deathEffect;     // Optional: Drag a particle effect here

    private bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null || isDead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            StopChasing();
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;

        if (direction.x != 0)
        {
            sr.flipX = direction.x < 0;
        }
    }

    void StopChasing()
    {
        rb.velocity = Vector2.zero;
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.hitHurt);
        }

        StartCoroutine(FlashRed());

        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator FlashRed()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // --- RPG SYSTEM UPDATE ---
        PlayerStats stats = FindObjectOfType<PlayerStats>();
        if (stats != null)
        {
            stats.AddXP(xpReward);
            stats.AddKill(); // THIS TRIGGERS YOUR HIDDEN ABILITY TRACKING
        }

        // --- SPLITTING LOGIC ---
        if (miniSlimePrefab != null)
        {
            Instantiate(miniSlimePrefab, transform.position + Vector3.right * 0.5f, Quaternion.identity);
            Instantiate(miniSlimePrefab, transform.position + Vector3.left * 0.5f, Quaternion.identity);
        }

        // --- LOOT & VFX ---
        if (coinPrefab != null) Instantiate(coinPrefab, transform.position, Quaternion.identity);
        if (deathEffect != null) Instantiate(deathEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player p = collision.gameObject.GetComponent<Player>();
            if (p != null) p.TakeDamage(attackDamage);
        }
    }
}