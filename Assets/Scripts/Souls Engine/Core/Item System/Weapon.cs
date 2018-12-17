using System;
using UnityEngine;
using SoulsEngine.Utility;
using SoulsEngine.Utility.Combat;

namespace SoulsEngine.Core.Combat {
    [Serializable]
    public class Weapon : Item
    {
        [SerializeField]
        private float _damage;
        public float Damage
        {
            get { return _damage; }
            set { _damage = Mathf.Clamp(value, 0, System.Int32.MaxValue); }
        }

        [SerializeField]
        private int _durability;
        public int Durability
        {
            get { return _durability; }
            set { _durability = Mathf.Clamp(value, 0, System.Int32.MaxValue); }
        }

        [SerializeField]
        public WeaponType WeaponType { get; set; }

        public Weapon()
        {

        }

        public Weapon(float __damage, int __durability, WeaponType _weaponType)
        {
            ItemType = ItemType.WEAPON;
            WeaponType = _weaponType;
            Damage = __damage;
            Durability = __durability;
        }
        
        public event Action<Actor> OnWeaponHit;

        Actor actor;

        private void Start()
        {
            actor = transform.parent.gameObject.GetComponent<Actor>();
        }

        private void OnTriggerEnter2D(Collider2D c)
        {
            //actor.Controller.velocity = Vector2.zero;

            var a = c.gameObject.GetComponent<Actor>();
            if (a != null)
            {
                if (OnWeaponHit != null)
                    OnWeaponHit(a);
            }
        }

    }
}