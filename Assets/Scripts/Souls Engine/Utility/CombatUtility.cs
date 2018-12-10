using UnityEngine;
using System.Collections;

namespace SoulsEngine.Utility.Combat
{
    #region STATS

    [System.Serializable]
    public struct Stats
    {

        [SerializeField]
        float _baseHealth;
        public float BaseHealth
        {
            get { return _baseHealth; }
            set { _baseHealth = value; }
        }

        [SerializeField]
        float _baseStamina;
        public float BaseStamina
        {
            get { return _baseStamina; }
            set { _baseStamina = value; }
        }


        [SerializeField]
        float _health;
        public float Health
        {
            get { return _health; }
            set { _health = value; }
        }

        [SerializeField]
        float _stamina;
        public float Stamina
        {
            get { return _stamina; }
            set { _stamina = value; }
        }

        [SerializeField]
        float _moveSpeed;
        public float MoveSpeed
        {
            get { return _moveSpeed; }
            set { _moveSpeed = value; }
        }


        [SerializeField]
        float _vitality;
        public float Vitality
        {
            get { return _vitality; }
            set { _vitality = value; }
        }

        [SerializeField]
        float _endurance;
        public float Endurance
        {
            get { return _endurance; }
            set { _endurance = value; }
        }

    }

    public struct StatusEffect
    {
        private string _attribute;
        public string Attribute
        {
            get { return _attribute; }
            set { _attribute = value; }
        }

        private float _value;
        public float Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private StatusEffectValueType _valueType;
        public StatusEffectValueType ValueType
        {
            get { return _valueType; }
            set { _valueType = value; }
        }

        private float _duration;
        public float Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        public StatusEffect(string __attribute, float __value, StatusEffectValueType __valueType, float __duration)
        {
            _attribute = __attribute;
            _value = __value;
            _valueType = __valueType;
            _duration = __duration;
        }
    }

    public enum StatusEffectValueType
    {
        PERCENT,                                    // 30% damage boost
        COMBINATION                                 // +3 armor
    };

    #endregion

    #region EQUIPMENT

    public struct Equipment
    {
        private Item _leftH;
        public Item LeftH
        {
            get { return _leftH; }
            set { _leftH = value; }
        }

        private Item _rightH;
        public Item RightH
        {
            get { return _rightH; }
            set { _rightH = value; }
        }

        private Item _armor;
        public Item Armor
        {
            get { return _armor; }
            set { _armor = value; }
        }

        public Equipment(Item __leftH, Item __rightH, Item __armor)
        {
            _leftH = __leftH;
            _rightH = __rightH;
            _armor = __armor;
        }

    }

    #endregion

    public struct Moveset { }

    public struct DamageModifier { }

    public class CombatUtility
    {

    }
}