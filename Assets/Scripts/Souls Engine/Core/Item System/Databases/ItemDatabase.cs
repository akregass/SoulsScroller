using System.Collections.Generic;
using UnityEngine;
using SoulsEngine.Core;

[System.Serializable]
[CreateAssetMenu]
public class ItemDatabase : ScriptableObject
{
    [SerializeField]
    private List<Item> _database = new List<Item>();
    [SerializeField]
    private List<Item> _weaponDatabase = new List<Item>();
    [SerializeField]
    private List<Item> _shieldDatabase = new List<Item>();
    [SerializeField]
    private List<Item> _armorDatabase = new List<Item>();
    [SerializeField]
    private List<Item> _consumableDatabase = new List<Item>();

    public List<Item> Database
    {
        get { return _database; }
        set { _database = value; }
    }

    public List<Item> WeaponDatabase
    {
        get { return _weaponDatabase; }
        set { _weaponDatabase = value; }
    }

    public List<Item> ShieldDatabase
    {
        get { return _shieldDatabase; }
        set { _shieldDatabase = value; }
    }

    public List<Item> ArmorDatabase
    {
        get { return _armorDatabase; }
        set { _armorDatabase = value; }
    }

    public List<Item> ConsumableDatabase
    {
        get { return _consumableDatabase; }
        set { _consumableDatabase = value; }
    }
}