using UnityEngine;
using UnityEngine.UI;
using SoulsEngine;
using SoulsEngine.Utility;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;
    public Image[] slots;
    public GameObject parent;

    private void Start()
    {
        //inventory = GodManager.Player.Inventory;
        inventory.OnInventoryChange += UpdateUI;

        var c = parent.GetComponentsInChildren<Button>();

        slots = new Image[c.Length];

        for(int i = 0; i < c.Length; i++)
        {
            slots[i] = Utility.GetComponentInChildren<Image>(c[i].gameObject);
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(i < inventory.inventory.Count)
            {
                slots[i].sprite = inventory.inventory[i].ItemIcon;
                slots[i].enabled = true;
            }
            else
            {
                slots[i].sprite = null;
                slots[i].enabled = false;
            }
            var c = GetComponentInChildren<Button>();
        }
    }
}
