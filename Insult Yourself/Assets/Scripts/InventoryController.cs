using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [HideInInspector]
    private ItemGrid selectedItemGrid;

    public ItemGrid SelectedItemGrid { 
        get => selectedItemGrid;
        set 
        { 
            selectedItemGrid = value;
            inventoryHighlight.SetParent(value);
        }
    }

    InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Transform canvasTransform;

    [SerializeField] GameObject panel;
    [SerializeField] GameObject weaponHolder;
    [SerializeField] ItemGrid defaultGrid;
    [SerializeField] ItemGrid smallGrid;
    [SerializeField] ItemGrid largeGrid;
    [SerializeField] ItemGrid additGrid;

    InventoryHighlight inventoryHighlight;

    private void Awake()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
        selectedItemGrid = defaultGrid;
    }

    private void Start()
    {
        InsertItem(items[0]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            panel.SetActive(!panel.activeInHierarchy);
            weaponHolder.SetActive(!weaponHolder.activeInHierarchy);
        }
        
        if (panel.activeInHierarchy == false) { return; }

        ItemIconDrag();

        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateItem();
        }

        if (selectedItemGrid == null) 
        {
            inventoryHighlight.Show(false);
            return; 
        }

        HandleHighlight();

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }
    }

    private void RotateItem()
    {
        if (selectedItem == null) { return; }

        selectedItem.Rotate();
    }

    public void InsertItem(ItemData item)
    {
        if (selectedItemGrid == null) 
        {
            selectedItemGrid = defaultGrid;
        }

        CreateItem(item);
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);
    }

    private void InsertItem(InventoryItem itemToInsert)
    {
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert);

        if (posOnGrid == null) { return; }

        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    Vector2Int oldPosition;
    InventoryItem itemToHighlight;

    private void HandleHighlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        
        if (oldPosition == positionOnGrid) { return; }

        oldPosition = positionOnGrid;
        
        if (selectedItem == null)
        { 
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x, positionOnGrid.y);

            if (itemToHighlight != null)
            {
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            }
            else
            {
                inventoryHighlight.Show(false);
            }
        }
        else
        {
            inventoryHighlight.Show(selectedItemGrid.BoundryCheck(
                positionOnGrid.x, 
                positionOnGrid.y, 
                selectedItem.WIDTH, 
                selectedItem.HEIGHT)
                );

            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem, positionOnGrid.x, positionOnGrid.y);
        }
    }

    private void CreateItem(ItemData item)
    {
        InventoryItem inventoryItem = Instantiate(itemPrefab).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        rectTransform.SetAsLastSibling();

        inventoryItem.Set(item);
    }

    private void LeftMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.WIDTH - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.HEIGHT - 1) * ItemGrid.tileSizeHeight / 2;
        }
        
        if (position.x < 0)
        {
            position.x = 0;
        }

        if (position.y < 0)
        {
            position.y = 0;
        }

        return selectedItemGrid.GetTileGridPosition(position);
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool complete = selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y, ref overlapItem);
        if (complete)
        {
            if (selectedItemGrid != defaultGrid)
            {
                SpawnUsable();
            }

            selectedItem = null;
            if (overlapItem != null)
            {
                selectedItem = overlapItem;
                DestroyUsable();
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
                rectTransform.SetAsLastSibling();
            }
        }
    }

    private void SpawnUsable()
    {
        if (selectedItemGrid == smallGrid)
            weaponHolder.GetComponent<WeaponSwitcher>().SpawnWeapon(selectedItem.itemData.usableObject, 0);
        else if (selectedItemGrid == largeGrid) 
            weaponHolder.GetComponent<WeaponSwitcher>().SpawnWeapon(selectedItem.itemData.usableObject, 1);
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();

            if (selectedItemGrid != defaultGrid)
            {
                DestroyUsable();
            }
        }
    }

    private void DestroyUsable()
    {
        weaponHolder.GetComponent<WeaponSwitcher>().DestroyWeapon(selectedItem.itemData.usableObject);
    }

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }
}
