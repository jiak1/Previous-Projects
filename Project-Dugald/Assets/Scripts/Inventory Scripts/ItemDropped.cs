using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropped : MonoBehaviour
{

    public int bagID;
    public DropManager dm;
    public int ItemAmount;
    public ItemList.ItemType itemType;

    private void Awake()
    {
       dm = GameObject.FindGameObjectWithTag("GM").GetComponent<DropManager>();
        dm.items.Add(this);
        bagID = dm.items.Count;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Hotbar").GetComponent<Inventory>().AddItem(itemType, ItemAmount);
            Destroy(this.gameObject);
        }
        else
        {
            if (collision.tag == "Bag")
            {
                if(collision.gameObject.GetComponent<ItemDropped>().itemType == itemType)
                {
                    
                    if(bagID < collision.GetComponent<ItemDropped>().bagID)
                    {
                        AddItem(collision.GetComponent<ItemDropped>().itemType, collision.GetComponent<ItemDropped>().ItemAmount);

                        Destroy(collision.gameObject);
                    }
                }
            }
        }
    }

    public void AddItem(ItemList.ItemType _itemType, int amt)
    {
            ItemAmount = ItemAmount + amt;
            return;
        
    }
}
