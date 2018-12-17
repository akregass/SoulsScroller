using System.Collections.Generic;
using SoulsEngine.Core;
using SoulsEngine.Utility;
using SoulsEngine.Utility.Combat;

public class Consumable : Item
{
	public List<StatusEffect> effects;

    public void Consume(Actor __actor, Inventory __inventory, int __id){
		for(int i=0; i < effects.Count; i++){
			//__actor.CurrentStatusEffects.Add(effects[i]);
		}

		if(__inventory.inventoryCount[__id] > 1){
			__inventory.inventoryCount[__id] = -1;
			__inventory.inventory[__id] = null;
		}
	}
}