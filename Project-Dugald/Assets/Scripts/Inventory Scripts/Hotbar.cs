using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour {

    public List<GameObject> slots = new List<GameObject>();
    public Inventory inventory;
    private ItemIconPrefabs iconPrefabs;

    private int slotSelected = 0;
    public ItemList.ItemType selectedItem;
    public Item selectedItemScript;

    GameObject tempSlot1;
    GameObject tempSlot2;
    GameObject tempSlot3;
    GameObject tempSlot4;
    GameObject tempSlot5;
    GameObject tempSlot6;
    GameObject tempSlot7;
    GameObject tempSlot8;
    GameObject tempSlot9;

    // Use this for initialization
    void Awake () {
        iconPrefabs = GameObject.FindGameObjectWithTag("GM").GetComponent<ItemIconPrefabs>();
        inventory = GetComponent<Inventory>();
        inventory.hotbar = this;
        SetupThings();
	}
    private void Start()
    {
        SelectSlot(1);
    }

    public void UpdateHotbar()
    {
        SelectSlot(slotSelected);
        if (slots.Count == 9)
        {
            for (int i = 0; i < 8 - 1; i++)
            {
                GameObject _slot = slots[i ];
                _slot.GetComponentInChildren<Text>().text = inventory.slots[i].item.StackSize.ToString();
                if(!(inventory.slots[i].item.item == ItemList.ItemType.None))
                {
                    
                    _slot.GetComponent<Image>().sprite = iconPrefabs.itemIcons[iconPrefabs.TurnItemTypeIntoINT(inventory.slots[i].item.item)];
                }
                
            }
        }else
        {
            StartCoroutine(wait());
        }
            
          
        
    }

    IEnumerator wait()
    {

        yield return new WaitForSeconds(1);
        UpdateHotbar();
    }

    IEnumerator wait2(int _slotID1)
    {

        yield return new WaitForSeconds(1);
        SelectSlot(_slotID1);
    }
    void SetupThings()
    {
        foreach (GameObject _go in GameObject.FindGameObjectsWithTag("Slot"))
        {
            if(_go.GetComponent<RandomHotbarIDCheckInefficiencyThing>().slotID == 1)
            {
                tempSlot1 = _go;
            }
            if (_go.GetComponent<RandomHotbarIDCheckInefficiencyThing>().slotID == 2)
            {
                tempSlot2 = _go;
            }
            if (_go.GetComponent<RandomHotbarIDCheckInefficiencyThing>().slotID == 3)
            {
                tempSlot3 = _go;
            }
            if (_go.GetComponent<RandomHotbarIDCheckInefficiencyThing>().slotID == 4)
            {
                tempSlot4 = _go;
            }
            if (_go.GetComponent<RandomHotbarIDCheckInefficiencyThing>().slotID == 5)
            {
                tempSlot5 = _go;
            }
            if (_go.GetComponent<RandomHotbarIDCheckInefficiencyThing>().slotID == 6)
            {
                tempSlot6 = _go;
            }
            if (_go.GetComponent<RandomHotbarIDCheckInefficiencyThing>().slotID == 7)
            {
                tempSlot7 = _go;
            }
            if (_go.GetComponent<RandomHotbarIDCheckInefficiencyThing>().slotID == 8)
            {
                tempSlot8 = _go;
            }
            if (_go.GetComponent<RandomHotbarIDCheckInefficiencyThing>().slotID == 9)
            {
                tempSlot9 = _go;
            }
        }

        slots.Add(tempSlot1);
        slots.Add(tempSlot2);
        slots.Add(tempSlot3);
        slots.Add(tempSlot4);
        slots.Add(tempSlot5);
        slots.Add(tempSlot6);
        slots.Add(tempSlot7);
        slots.Add(tempSlot8);
        slots.Add(tempSlot9);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSlot(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSlot(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSlot(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectSlot(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectSlot(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectSlot(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectSlot(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SelectSlot(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SelectSlot(9);
        }
    }

    void SelectSlot(int _slotID)
    {
        if(inventory.slots.Count == 9)
        {
            slotSelected = _slotID;
            DisableSlotAllImages();
            EnableSelectedSlotImage(_slotID);
            if (inventory.slots[_slotID - 1].item.noItem)
            {
                selectedItem = ItemList.ItemType.None;
                selectedItemScript = null;
                return;
            }
            else
            {
                selectedItem = inventory.slots[_slotID - 1].item.item;
                selectedItemScript = inventory.slots[_slotID - 1].item;
            }
        }else
        {
            StartCoroutine(wait2(_slotID));
        }
    }

    void DisableSlotAllImages()
    {
        tempSlot1.GetComponent<RandomHotbarIDCheckInefficiencyThing>().selected.SetActive(false);
        tempSlot2.GetComponent<RandomHotbarIDCheckInefficiencyThing>().selected.SetActive(false);
        tempSlot3.GetComponent<RandomHotbarIDCheckInefficiencyThing>().selected.SetActive(false);
        tempSlot4.GetComponent<RandomHotbarIDCheckInefficiencyThing>().selected.SetActive(false);
        tempSlot5.GetComponent<RandomHotbarIDCheckInefficiencyThing>().selected.SetActive(false);
        tempSlot6.GetComponent<RandomHotbarIDCheckInefficiencyThing>().selected.SetActive(false);
        tempSlot7.GetComponent<RandomHotbarIDCheckInefficiencyThing>().selected.SetActive(false);
        tempSlot8.GetComponent<RandomHotbarIDCheckInefficiencyThing>().selected.SetActive(false);
        tempSlot9.GetComponent<RandomHotbarIDCheckInefficiencyThing>().selected.SetActive(false);
    }

    void EnableSelectedSlotImage(int _slotID)
    {
        if(_slotID == 1)
        {
            tempSlot1.GetComponent<RandomHotbarIDCheckInefficiencyThing>().selected.SetActive(true);
        }
        if (_slotID == 2)
        {
            tempSlot2.GetComponent<RandomHotbarIDCheckInefficiencyThing>().selected.SetActive(true);
        }
        if (_slotID == 3)
        {
            tempSlot3.GetComponent<RandomHotbarIDCheckInefficiencyThing>().selected.SetActive(true);
        }
        if (_slotID == 4)
        {
            tempSlot4.GetComponent<RandomHotbarIDCheckInefficiencyThing>().selected.SetActive(true);
        }
        if (_slotID == 5)
        {
            tempSlot5.GetComponent<RandomHotbarIDCheckInefficiencyThing>().selected.SetActive(true);
        }
        if (_slotID == 6)
        {
            tempSlot6.GetComponent<RandomHotbarIDCheckInefficiencyThing>().selected.SetActive(true);
        }
        if (_slotID == 7)
        {
            tempSlot7.GetComponent<RandomHotbarIDCheckInefficiencyThing>().selected.SetActive(true);
        }
        if (_slotID == 8)
        {
            tempSlot8.GetComponent<RandomHotbarIDCheckInefficiencyThing>().selected.SetActive(true);
        }
        if (_slotID == 9)
        {
            tempSlot9.GetComponent<RandomHotbarIDCheckInefficiencyThing>().selected.SetActive(true);
        }
    }
}
