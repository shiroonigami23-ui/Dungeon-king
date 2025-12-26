using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [Header("Sprites (0: Idle, 1: Drawn)")]
    public Sprite[] sprites;
    private SpriteRenderer sr;

    [Header("Shooting")]
    public GameObject arrowPrefab;
    public Transform spawnPoint;
    public float projSpeed = 15f;
    public float timeBetweenShots = 0.5f;
    private float nextShotTime;

    private Transform pivot;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        // The pivot is the parent (Hand1)
        pivot = transform.parent; 
        if (sprites.Length > 0) sr.sprite = sprites[0];
    }

    void Update()
    {
        HandleAiming();

        // 1. Draw the bow (Change sprite on click)
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (sprites.Length > 1) sr.sprite = sprites[1];
        }

        // 2. Shoot (On Release)
        if (Input.GetKeyUp(KeyCode.Mouse0) && Time.time > nextShotTime)
        {
            Shoot();
            nextShotTime = Time.time + timeBetweenShots;
        }
    }

    void HandleAiming()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - pivot.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        // Rotate the pivot (the hand)
        pivot.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    void Shoot()
    {
        sr.sprite = sprites[0]; // Reset to idle sprite
        
        // Play Sound
        if(SoundManager.Instance != null) SoundManager.Instance.PlaySound(SoundManager.Instance.swordSwing);

        // Spawn Arrow
        GameObject a = Instantiate(arrowPrefab, spawnPoint.position, pivot.rotation);
        
        // Fire it forward (up direction of the pivot)
        Rigidbody2D arb = a.GetComponent<Rigidbody2D>();
        if (arb != null)
        {
            arb.velocity = pivot.up * projSpeed;
        }
    }
}