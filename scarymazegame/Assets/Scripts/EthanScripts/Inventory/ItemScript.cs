using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public enum ItemType
    {
        Trap,
        Craftable,
        Consumable
    };

    [Header("General")]
    public string itemName;
    public string itemDescription;
    public ItemType itemType;
    public int maxStack;

    [Header("Crafting Mats")]
    public List<GameObject> m_CraftingItems;

    [Header("Advanced - Do Not Modify")]
    public bool canBe_equipped;
    public bool canBe_consumed;
    public bool canBe_dropped;
    
    public bool Craft(GlobalInventory _playerInventory)
    {
        if (m_CraftingItems.Count > 0)    // a craftable item.
        {
            // Check if both items exist in inventory
            foreach (GameObject item in m_CraftingItems)
            {
                if (!_playerInventory.SearchForItem(item)) return false;
            }

            // If item exists but there is no space...
            for (int i = 0; i < _playerInventory.maxSpace; i++) 
            {
                if (_playerInventory.backpack[i] == gameObject && _playerInventory.backpackItemCount[i] == maxStack) return false;
                // Otherwise, add item to backpack
                else if (_playerInventory.backpack[i] == gameObject && _playerInventory.backpackItemCount[i] != maxStack)
                {
                    _playerInventory.backpackItemCount[i]++;

                    // Delete items
                    foreach (GameObject item in m_CraftingItems)
                    {
                        _playerInventory.DropItemGameObject(item);
                    }

                    return true;
                }
            }

            // If item doesn't exist but theres no space...
            if (_playerInventory.backpack.Count < maxStack) return false;

            // Otherwise... add this item to the inventory
            for (int i = 0; i < _playerInventory.backpack.Count; i++)
            {
                if (_playerInventory.backpack[i] == null)
                {
                    _playerInventory.backpack[i] = gameObject;
                    _playerInventory.backpackItemCount[i]++;

                    // Delete items
                    foreach (GameObject item in m_CraftingItems)
                    {
                        _playerInventory.DropItemGameObject(item);
                    }
                    
                    return true;
                }
            }
        }
        return false;
    }

    void Start()
    {
        // Set item attributes
        switch (itemType)
        {
            case ItemType.Trap:
                canBe_equipped = true;
                canBe_consumed = false;
                canBe_dropped = true;
                break;
            case ItemType.Craftable:
                canBe_equipped = false;
                canBe_consumed = false;
                canBe_dropped = true;
                break;
            case ItemType.Consumable:
                canBe_equipped = false;
                canBe_consumed = true;
                canBe_dropped = true;
                break;
        }
    }
}
