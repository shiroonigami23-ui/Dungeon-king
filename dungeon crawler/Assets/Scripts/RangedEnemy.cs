using UnityEngine;
using System.Collections;

public class RangedEnemy : MonoBehaviour
{
    [Header("Stats")]
    public float health = 20f;
    public float speed = 2f;
    public float stopDistance = 5f; 
    public float retreatDistance = 3f; 
    
    [Header("Combat")]
    public GameObject projectilePrefab;
    public Transform shotPoint;
    public float timeBetweenShots = 2f;
    private float nextShotTime;

    private Transform player;
    private SpriteRenderer sr;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // 1. Movement Logic (Maintain Distance)
        if (distance > stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (distance < retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }

        // 2. Shooting Logic
        if (Time.time > nextShotTime && distance <= stopDistance + 1f)
        {
            Shoot();
            nextShotTime = Time.time + timeBetweenShots;
        }

        // Flip sprite to face player
        if (player.position.x < transform.position.x) sr.flipX = true;
        else sr.flipX = false;
    }

    void Shoot()
    {
        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        Instantiate(projectilePrefab, shotPoint.position, rotation);
        
        if(SoundManager.Instance != null)
            SoundManager.Instance.PlaySound(SoundManager.Instance.swordSwing);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        StartCoroutine(HitFlash());
        if (health <= 0) Die();
    }

    IEnumerator HitFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }

    void Die()
    {
        // Add coin drop logic here if needed
        Destroy(gameObject);
    }
}