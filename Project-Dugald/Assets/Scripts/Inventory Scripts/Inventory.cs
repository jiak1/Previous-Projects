using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public GameObject slotPrefab;

    public List<Slot> slots = new List<Slot>();
    public int maxSlots;

    public bool isHotbar = false;
    public Hotbar hotbar;
    // Use this for initialization
    void Start()
    {

        for (int i = 0; i < maxSlots; i++)
        {
            GameObject _GOslot = Instantiate(slotPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            _GOslot.AddComponent<Slot>();
            _GOslot.transform.parent = this.transform;
            _GOslot.name = "Slot: " + i;
            Slot _slot = _GOslot.GetComponent<Slot>();
            _slot.slotID = i;
            _slot.inventory = this;
            slots.Add(_slot);
        }

    }

    // Update is called once per frame
    public void AddItem(ItemList.ItemType _item, int numItem)
    {
        int _numItem = numItem;
        if (slots.Count == maxSlots)
        {

            if (slots[maxSlots - 1].item == null)
            {
                StartCoroutine(wait(_item, _numItem));
            }
            else
            {
                //All slots are spawned
                foreach (Slot _slot in slots)
                {
                    if(_numItem == 0)
                    {
                        return;
                    }
                    if (_slot.item.item == _item || _slot.item.noItem == true)
                    {
                        if (_slot.item.item == _item)
                        {
                            //Debug.Log("_numItem =" + _numItem + "For starting with slot" + _slot.slotID);
                            //Debug.Log("-1");
                            //4 already 16 stacksize we wanted to add 14
                            //4+14-16
                            if (_slot.item.MaxStackSize > (_slot.item.StackSize + _numItem))
                            {
                                //Debug.Log("0");
                                _slot.item.item = _item;
                                _slot.item.StackSize = _slot.item.StackSize + _numItem;
                                _slot.item.noItem = false;
                                _numItem = 0;
                                if (isHotbar)
                                {
                                    hotbar.UpdateHotbar();
                                    
                                    return;
                                }

                                return;
                            }
                            else
                            {
                               // Debug.Log("1");
                                _slot.item.item = _item;
                                _slot.item.noItem = false;
                                _numItem = (_slot.item.StackSize + _numItem) - _slot.item.MaxStackSize;
                                _slot.item.StackSize = _slot.item.MaxStackSize;
                                if (isHotbar)
                                {
                                    hotbar.UpdateHotbar();

                                }
                            }
                        }

                        if (_slot.item.noItem == true)
                        {
                           // Debug.Log("2");

                            //Debug.Log("Max Stack Size:" + _slot.item.MaxStackSize + "Num Items:" + _numItem);
                            if (_slot.item.MaxStackSize > _numItem)
                            {
                                //Max stack size or 16 is greater then the number of items we want to add so therefore we don't need to go to the next slot
                                //Debug.Log("3");
                                _slot.item.item = _item;
                                _slot.item.StackSize = _numItem;
                                _slot.item.noItem = false;
                                _numItem = 0;
                                if (isHotbar)
                                {
                                    hotbar.UpdateHotbar();
                                    return;
                                }
                                
                                return;
                            }
                            else
                            {
                                //Debug.Log("4");
                                _slot.item.item = _item;
                                _slot.item.noItem = false;
                                _numItem = (_slot.item.StackSize + _numItem) - _slot.item.MaxStackSize;
                                _slot.item.StackSize = _slot.item.MaxStackSize;
                                //Debug.Log("Set _numItem To:" + _numItem + "For slot" + _slot.slotID);
                                if (isHotbar)
                                {
                                    hotbar.UpdateHotbar();

                                }
                            }

                        }
                    }else
                    {
                        if(_slot.slotID == 9)
                        {
                            return;
                        }
                    }
                }

                if (isHotbar)
                {
                    hotbar.UpdateHotbar();
                }
            }
        }
        else
        {
            StartCoroutine(wait(_item, _numItem));
        }

    }


    IEnumerator wait(ItemList.ItemType _item, int numItem)
    {

        yield return new WaitForSeconds(1);
        AddItem(_item, numItem);
    }

}
