using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    // Change Sprites
    public Sprite[] sprites;
    private SpriteRenderer sr;

    // Projectile Stuff (Shooting)
    public GameObject projectile;
    public Transform spawnPoint;
    public float projSpeed;

    // Aim Stuff
    private Vector2 target;
    public Transform hand1;

    // Start is called before the first frame update
    void Start()
    {
        hand1 = gameObject.transform.parent.parent;
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = sprites[0];
    }

    // Update is called once per frame
    void Update()
    {
        // Aim
        target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hand1.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(target.y - hand1.position.y, target.x - hand1.position.x) * Mathf.Rad2Deg - 90);

        // Shoot on Mouse Click
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            sr.sprite = sprites[1];
        }
    }

    void Shoot()
    {
        sr.sprite = sprites[0];
        GameObject a = Instantiate(projectile, spawnPoint.position, hand1.rotation);
        Rigidbody2D arb = a.GetComponent<Rigidbody2D>();
        arb.AddForce(spawnPoint.transform.up * (projSpeed / 1000), ForceMode2D.Impulse);
    }
}
