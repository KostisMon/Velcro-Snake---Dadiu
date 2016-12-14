 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FullInspector;

//this goes on top snake part
public class SnakeInventory : BaseObject, ISnakeCollisionEnter, IEventInvoker {
	#region IROFL implementation
	public void OnPoop(string s)
	{
		Debug.Log("SATAN IS UPON IS");
	}
	#endregion
	[InspectorComment("Add this to top of snake hierachy")]
	[ShowInInspector]
	public HashSet<StickyItem> inventory = new HashSet<StickyItem>();
    public Dictionary<SnakeCollider,List<StickyItem>> jointInventory = new Dictionary<SnakeCollider, List<StickyItem>>();

    public void PickUpItem(StickyItem x, SnakeCollider joint){
		if(inventory.Add(x)){
            if(!jointInventory.ContainsKey(joint))
                jointInventory[joint] = new List<StickyItem>();
            jointInventory[joint].Add(x);
            eventManager.InvokeEventFast<ISnakeItemPickup, StickyItem, SnakeInventory>(x, this);
            //eventManager.InvokeSnakeItemPickup(x,this);
        }
	}

    public void DropItem(StickyItem x, SnakeCollider y)
    {
        if (inventory.Contains(x))
        {
            inventory.Remove(x);
            jointInventory.Remove(y);
            eventManager.InvokeEventFast<ISnakeItemDrop, StickyItem, SnakeInventory>(x, this);
        }
    }

    public void DropAllItems()
    {
        foreach (var x in inventory)
        {
            eventManager.InvokeEventFast<ISnakeItemDrop, StickyItem, SnakeInventory>(x, this);
        }
        inventory.Clear();
        jointInventory.Clear();
    }

	public bool ValidatePickup(StickyItem x){
		return true;
		//Here we might want to not pick up some items at some point for some reason sometimes or something
	}

	public void OnSnakeCollisionEnter(Collision col, SnakeCollider joint){
		Collider collider = col.collider;
		BaseObject collidedObject;

		if(ColliderCache.cachedObjects.TryGetValue(collider, out collidedObject))
		{
			StickyItem sticky = collidedObject as StickyItem;
			if(sticky != null)
			{
				if(ValidatePickup(sticky))
				{
					if(sticky.TryStickToSnake(col, joint.rigid))
					{
						PickUpItem(sticky, joint);
					}
				}
			}
		}
	}

	protected override void EarlyAwake() {
		inventory.Clear();
		jointInventory.Clear();
	}

	public void OnSnakeCollisionExit(Collision col, SnakeCollider joint){

	}
}
