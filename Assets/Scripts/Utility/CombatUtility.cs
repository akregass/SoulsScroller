using UnityEngine;
using System.Collections;

namespace SoulsEngine.Utility
{
    public enum StatusEffectValueType {
        PERCENT,                                    // 30% damage boost
        COMBINATION                                 // +3 armor
    };

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

    public struct DamageModifier { }

    public class CombatUtility
    {

    }
}