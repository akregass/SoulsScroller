using System.Collections.Generic;
using SoulsEngine.Utility;
using SoulsEngine.Utility.Combat;

public class Consumable : Item
{
	public List<StatusEffect> effects;

    public Consumable(string __name, string __description, ItemType __type, int __stackSize, int __id) : base(__name, __description, __type, __stackSize, __id)
    {

    }

    public void Consume(Actor __actor, Inventory __inventory, int __id){
		for(int i=0; i < effects.Count; i++){
			__actor.CurrentStatusEffects.Add(effects[i]);
		}

		if(__inventory.inventoryCount[__id] > 1){
			__inventory.inventoryCount[__id] = -1;
			__inventory.inventory[__id] = null;
		}
	}
}