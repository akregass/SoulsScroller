using UnityEngine;
using SoulsEngine.Utility;

namespace SoulsEngine.Core.Combat {
    [System.Serializable]
    public class Weapon : Item
    {

        private float _damage;
        public float Damage
        {
            get { return _damage; }
            set { _damage = Mathf.Clamp(value, 0, System.Int32.MaxValue); }
        }

        private int _durability;
        public int Durability
        {
            get { return _durability; }
            set { _durability = Mathf.Clamp(value, 0, System.Int32.MaxValue); }
        }

        public WeaponType WeaponType { get; set; }

        public Weapon(float __damage, int __durability, WeaponType _weaponType)
        {
            ItemType = ItemType.WEAPON;
            WeaponType = _weaponType;
            Damage = __damage;
            Durability = __durability;
        }

        public Weapon(string name, string description, ItemType type, int stackSize, int id) : base(name, description, type, stackSize, id)
        {

        }
    }
}