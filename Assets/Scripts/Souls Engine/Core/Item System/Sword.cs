using SoulsEngine.Core.Combat;
using SoulsEngine.Utility;

[System.Serializable]
public class Sword : Weapon
{
    public Sword(string name, string description, ItemType type, int stackSize, int id) : base(name, description, type, stackSize, id)
    {

    }
}