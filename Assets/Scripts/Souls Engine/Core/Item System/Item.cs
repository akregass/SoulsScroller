using UnityEngine;
using SoulsEngine.Utility;

namespace SoulsEngine.Core
{

    [System.Serializable]
    public class Item : MonoBehaviour
    {
        private int _itemId;
        [SerializeField] private string _itemName;
        [SerializeField] private string _itemDescription;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private Sprite _itemIcon;
        [SerializeField] private int _itemStackSize;

        public string ItemName
        {
            get { return _itemName; }
            set { _itemName = value; }
        }
        public string ItemDescription
        {
            get { return _itemDescription; }
            set { _itemDescription = value; }
        }
        public ItemType ItemType
        {
            get { return _itemType; }
            set { _itemType = value; }
        }
        public Sprite ItemIcon
        {
            get { return _itemIcon; }
            set { _itemIcon = value; }
        }
        public int ItemID
        {
            get { return _itemId; }
            set { _itemId = value; }
        }
        public int ItemStackSize
        {
            get { return _itemStackSize; }
            set { _itemStackSize = value; }
        }


        public Item(string __name, string __description, ItemType __type, int __stackSize, int __id)
        {
            ItemName = __name;
            ItemDescription = __description;
            ItemType = __type;
            ItemIcon = Resources.Load<Sprite>("Textures/Icons/" + __name);
            ItemStackSize = __stackSize;
            ItemID = __id;
        }

        public Item()
        {
            ItemName = "";
            ItemDescription = "";
            ItemType = ItemType.WEAPON;
            ItemStackSize = 1;
        }
    }

}