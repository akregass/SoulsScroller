using UnityEngine;
using System.Collections.Generic;
using SoulsEngine;
using SoulsEngine.Utility;

public class Inventory : MonoBehaviour
{

    public delegate void OnInventoryChange();
    public OnInventoryChange inventoryChangeCallback;

	public List<Item> inventory = new List<Item>();
	public List<int> inventoryCount = new List<int>();

    public int maxSlots;

	private ItemDatabase itemDB;
    public GameObject inventoryUI;

	public bool isPlayer;

	void Start()
    {
		itemDB = GameObject.FindGameObjectWithTag("God Manager").GetComponent<GodManager>().ItemDB;

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
		if(Contains(_id))
        {
			for(int i=0; i<inventory.Count; i++)
            {
				if(inventory[i].ItemID == _id)
                {
					if(inventoryCount[i] < inventory[i].ItemStackSize)
                    {
						inventoryCount[i]++;

                        if (inventoryChangeCallback != null)
                            inventoryChangeCallback.Invoke();

						return i;
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
						inventoryCount[i] = 1;

                        if (inventoryChangeCallback != null)
                            inventoryChangeCallback.Invoke();

                        return i;
					}
				}
			}
		}

		return -1;
	}

	public int AddItem(int __id, int __count)
    {
		if(Contains(__id))
        {
			for(int i=0; i<inventory.Count; i++)
            {
				if(inventory[i].ItemID == __id)
                {
					if((inventoryCount[i] + __count) < inventory[i].ItemStackSize)
                    {
						inventoryCount[i]+=__count;

                        if (inventoryChangeCallback != null)
                            inventoryChangeCallback.Invoke();

                        return i;
					}
                    else
					{
					    __count -= inventory[i].ItemStackSize - inventoryCount[i];
                        inventoryCount[i] += inventory[i].ItemStackSize - inventoryCount[i];
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
					if(itemDB.Database[j].ItemID == __id)
                    {
						inventory[i] = itemDB.Database[j];
						inventoryCount[i] = __count;

                        if (inventoryChangeCallback != null)
                            inventoryChangeCallback.Invoke();

                        return i;
					}
				}
			}
		}

		return -1;
	}

	public void UseItem(int __id)
    {
        if (inventory[__id].ItemType == ItemType.CONSUMABLE)
        {
            var c = (Consumable)inventory[__id];
            c.Consume(gameObject.GetComponent<Actor>(), this, __id);
        }

		Debug.Log(inventory[__id].ItemType.ToString() + " used: " + inventory[__id].ItemName);
	}

	public bool Contains(int __id)
    {
		foreach(Item i in inventory)
        {
			if(i.ItemID == __id)
				return true;
		}

		return false;
	}

	public int CountOf(int __id)
    {
		int count = 0;

		for(int i = 0; i < inventory.Count; i++)
        {
			if(inventory[i].ItemID == __id)
				count += inventoryCount [i];
		}

		return count;
	}

	public void RemoveItem(int __id)
    {
		if(Contains(__id))
        {
			for(int i=0; i<inventory.Count; i++)
            {
				if(inventory[i].ItemID == __id)
                {
                    if (inventoryCount[i] > 1)
                    {
                        inventoryCount[i]--;

                        if (inventoryChangeCallback != null)
                            inventoryChangeCallback.Invoke();
                    }
                    else
                    {
                        inventory[i] = new Item();
                        inventoryCount[i] = 0;

                        if (inventoryChangeCallback != null)
                            inventoryChangeCallback.Invoke();
                    }
				}
			}
		}
	}

	public void RemoveItem(int __id, int __count)
    {
		if(Contains(__id))
        {
			for(int i=0; i<inventory.Count; i++)
            {
				if(inventory[i].ItemID == __id)
                {
					if(inventoryCount[i] > 1)
                    {
                        inventoryCount[i]--;

                        if (inventoryChangeCallback != null)
                            inventoryChangeCallback.Invoke();
                    }
					else
                    {
						inventory[i] = new Item();
						inventoryCount[i] = 0;

                        if (inventoryChangeCallback != null)
                            inventoryChangeCallback.Invoke();
                    }
				}
			}
		}
	}
}