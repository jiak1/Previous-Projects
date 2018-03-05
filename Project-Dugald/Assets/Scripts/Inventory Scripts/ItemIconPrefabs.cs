using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ItemIconPrefabs : MonoBehaviour {

    public Sprite[] itemIcons;

    public int TurnItemTypeIntoINT(ItemList.ItemType _itemType )
    {
        
        int toInteger = (int)_itemType;
        return toInteger;
    }

    public ItemList.ItemType TurnINTIntoItemType(int _int)
    {
        ItemList.ItemType _item = (ItemList.ItemType)_int;
        return _item;
    }
}
