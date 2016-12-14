using UnityEngine;
using System.Collections;

public class GroceryItemMultiObject : GroceryItem, IObjectivePickup, ISnakeItemDrop{
	int currentPickedUpItems = -1;
	public int itemNumber;

	public override void OnObjectiveItemPickup(LevelObjective lvlObj, BaseObject item)
	{
		StickyItem obj = item as StickyItem;
		if(connectedItem != null && obj != null){
			if(obj.UUID == connectedItem.UUID && connectedItem.UUID!= -1){
				connectedObjective = lvlObj;
				if(lvlObj.isMainObjective){
					completionSound = awardMain;
				}
				else {
					completionSound = awardSide;
				}
				currentPickedUpItems ++;
				if(currentPickedUpItems == (itemNumber))
				{
					image.sprite = connectedItem.icon;
					FadeOpen(false);
				}
			}
		}
	}
	public void OnSnakeItemDrop(global::StickyItem obj, global::SnakeInventory snakeInv)
	{
		if(obj.UUID == connectedItem.UUID && connectedItem.UUID!= -1){
			if(connectedObjective!=null){
				if(!connectedObjective.hasFinished){
					image.sprite = defaultIcon;
					currentPickedUpItems = -1;
				}
			}

		}

	}
	public void OnSnakeItemPickup(global::StickyItem obj, global::SnakeInventory snakeInv)
	{

	}
}
