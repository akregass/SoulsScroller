using System;
using System.Collections.Generic;
using UnityEngine;
using SoulsEngine;
using SoulsEngine.Core;
using SoulsEngine.Utility;

public class Inventory : MonoBehaviour
{

    public event Action OnInventoryChange;

	public List<Item> inventory = new List<Item>();
	public List<int> inventoryCount = new List<int>();

    public int maxSlots;

	private ItemDatabase itemDB;
    public GameObject inventoryUI;

	public bool isPlayer;

	void Start()
    {
        itemDB = GodManager.ItemDB;

		isPlayer = false;
	}

	void Update()
    {
        if (isPlayer)
        {
            if(Input.GetButtonDown("Inventory"))
            {
                ToggleInventory();
            }
        }
	}

    public void ToggleInventory()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

	public int AddItem(int _id)
    {
        return AddItem(_id, 1);
	}

	public int AddItem(int _id, int _count)
    {
		if(Contains(_id))
        {
			for(int i=0; i<inventory.Count; i++)
            {
				if(inventory[i].ItemID == _id)
                {
					if((inventoryCount[i] + _count) < inventory[i].ItemStackSize)
                    {
						inventoryCount[i]+=_count;

                        if (OnInventoryChange != null)
                            OnInventoryChange.Invoke();

                        return i;
					}
                    else
					{
                        var d = inventory[i].ItemStackSize - inventoryCount[i];
                        _count -= d;
                        inventoryCount[i] += d;
					}
				}
			}
		}

		for(int i=0; i<inventory.Count; i++)
        {
			if(inventory[i].ItemName == null)
            {
				for(int j=0; j<itemDB.Database.Count; j++)
                {
					if(itemDB.Database[j].ItemID == _id)
                    {
						inventory[i] = itemDB.Database[j];
						inventoryCount[i] = _count;

                        if (OnInventoryChange != null)
                            OnInventoryChange.Invoke();

                        return i;
					}
				}
			}
		}

		return -1;
	}

	public void UseItem(int _id)
    {
        if (inventory[_id].ItemType == ItemType.CONSUMABLE)
        {
            var c = (Consumable)inventory[_id];
            c.Consume(GetComponent<Actor>(), this, _id);
        }

		Debug.Log(inventory[_id].ItemType.ToString() + " used: " + inventory[_id].ItemName);
	}

	public bool Contains(int _id)
    {
        Item item;
        for(int i = 0; i < inventory.Count; i++)
        {
            item = inventory[i];
            if (item.ItemID == _id)
                return true;
        }

		return false;
	}

	public int CountOf(int _id)
    {
		int count = 0;

		for(int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].ItemID == _id)
            {
                count += inventoryCount[i];

                if (!Contains(_id))
                    break;
            }
		}

		return count;
	}

	public void RemoveItem(int _id)
    {
        RemoveItem(_id, 1);
	}

	public void RemoveItem(int _id, int _count)
    {
		if(Contains(_id))
        {
			for(int i = 0; i < inventory.Count; i++)
            {
				if(inventory[i].ItemID == _id)
                {
					if(inventoryCount[i] > _count)  // can we get remove _count from a single slot
                    {
                        inventoryCount[i] -= _count;

                        if (OnInventoryChange != null)
                            OnInventoryChange.Invoke();

                        break;
                    }
					else
                    {
                        _count -= inventoryCount[i];
						inventory[i] = null;
						inventoryCount[i] = 0;
                    }
				}
			}
		}
	}
}