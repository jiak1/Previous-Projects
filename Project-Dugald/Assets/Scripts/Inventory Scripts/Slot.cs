using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{

    public Item item;
    public Inventory inventory;

    public int slotID;
    public int row;
    public int column;

    private void Start()
    {
        item = gameObject.AddComponent<Item>();
        item.slot = this;
        item.noItem = true;
        item.item = ItemList.ItemType.None;
        item.MaxStackSize = 16;
        item.StackSize = 0;
    }
}
