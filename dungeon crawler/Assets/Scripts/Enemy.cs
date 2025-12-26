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

    [Header("Components")]
    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        // Find the player automatically by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

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

        // Flip sprite based on movement
        sr.flipX = direction.x < 0;
    }

    void StopChasing()
    {
        rb.velocity = Vector2.zero;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

    SoundManager.Instance.PlaySound(SoundManager.Instance.hitHurt);
        Debug.Log("Enemy hit! Remaining Health: " + health);

        // Flash Red (Optional visual feedback)
        StartCoroutine(FlashRed());

        if (health <= 0) Die();
    }

    IEnumerator FlashRed()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    [Header("Loot")]
public GameObject coinPrefab; // Assign your GoldCoin prefab here in Inspector

void Die()
{
    // Spawn a coin where the enemy died
    if (coinPrefab != null)
    {
        Instantiate(coinPrefab, transform.position, Quaternion.identity);
    }

    Debug.Log("Enemy Destroyed!");
    Destroy(gameObject);
}

    // Hit the player if we touch them
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(attackDamage);
        }
    }
}