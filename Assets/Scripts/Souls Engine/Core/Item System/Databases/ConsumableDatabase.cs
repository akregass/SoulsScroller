using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ConsumableDatabase
{
    private List<Consumable> _consumables;
    public List<Consumable> Consumables
    {
        get { return _consumables; }
        set { _consumables = value; }
    }
}
