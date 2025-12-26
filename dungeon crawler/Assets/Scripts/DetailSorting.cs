using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailSorting : MonoBehaviour
{
    [SerializeField]
    private int SortOrderBase;
    [SerializeField]
    private int Offset;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    private void lateUpdate()
    {
        sr.sortingOrder = (int)(SortOrderBase - transform.position.y - Offset);
    }
}
