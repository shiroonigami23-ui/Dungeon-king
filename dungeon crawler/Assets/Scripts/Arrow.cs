using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage = 20f;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Don't let missed arrows live forever
    }

    // Use Trigger for smoother physics
    private void OnTriggerEnter2D(Collider2D col)
    {
        // 1. Hit Enemy
        if (col.CompareTag("Enemy"))
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null) enemy.TakeDamage(damage);
            
            BossController boss = col.GetComponent<BossController>();
            if (boss != null) boss.TakeDamage(damage);

            Destroy(gameObject);
        }
        
        // 2. Hit Walls or Borders
        if (col.CompareTag("Border") || col.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            Destroy(gameObject);
        }
    }
}