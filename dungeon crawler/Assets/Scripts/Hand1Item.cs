using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand1Item : MonoBehaviour
{
    public void Hold(GameObject PickUp)
    {
        PickUp.transform.position = transform.position;
        PickUp.transform.rotation = Quaternion.identity;
        PickUp.SetActive(true);
    }
}
