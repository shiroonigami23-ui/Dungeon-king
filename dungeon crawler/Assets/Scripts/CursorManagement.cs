using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManagement : MonoBehaviour
{
    // General Setup
    private SpriteRenderer sr;

    // Cursors
    public Sprite[] cursors;

    void Start()
    {
        Cursor.visible = false;
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = cursors[0];
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;

    }
}
