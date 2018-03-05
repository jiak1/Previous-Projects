using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    public ItemList.ItemType item;
    public Slot slot;

    public int data = 0;
    public bool noItem = true;

    public int MaxStackSize;
    public int StackSize;
}
