using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    private bool canOpen = false;

    void Update()
    {
        if (canOpen && Input.GetKeyDown(KeyCode.E))
        {
            FindObjectOfType<ShopManager>().OpenShop();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) canOpen = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) canOpen = false;
    }
}