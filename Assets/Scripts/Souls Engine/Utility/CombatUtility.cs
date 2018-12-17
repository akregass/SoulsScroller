using UnityEngine;
using SoulsEngine.Core.Combat;

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
        float _health;
        public float Health
        {
            get { return _health; }
            set { _health = value; }
        }

        [SerializeField]
        float _moveSpeed;
        public float MoveSpeed
        {
            get { return _moveSpeed; }
            set { _moveSpeed = value; }
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
    
    [System.Serializable]
    public enum WeaponType
    {
        SWORD,
        AXE,
        SPEAR,
        HAMMER
    }

    [System.Serializable]
    public struct Equipment
    {
        [SerializeField]
        private Weapon _leftH;
        public Weapon LeftH
        {
            get { return _leftH; }
            set { _leftH = value; }
        }

        [SerializeField]
        private Weapon _rightH;
        public Weapon RightH
        {
            get { return _rightH; }
            set { _rightH = value; }
        }

        public Equipment(Weapon __leftH, Weapon __rightH)
        {
            _leftH = __leftH;
            _rightH = __rightH;
        }

    }

    #endregion

    public struct Moveset { }

    public struct DamageModifier { }

    public class CombatUtility
    {

    }
}