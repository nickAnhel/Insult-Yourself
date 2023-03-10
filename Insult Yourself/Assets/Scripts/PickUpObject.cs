using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickUpObject : MonoBehaviour
{
    public ItemData itemData;
    public InventoryController inventoryController;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                inventoryController.InsertItem(itemData);
                Destroy(gameObject);
            }
        }
    }
}
