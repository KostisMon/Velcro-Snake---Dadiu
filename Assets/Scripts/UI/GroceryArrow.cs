using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using FullInspector;

public class GroceryArrow : BaseObject, ISnakeItemDrop, IObjectiveComplete{
	#region ISnakeItemEvents implementation
	public void OnSnakeItemPickup(global::StickyItem obj, global::SnakeInventory snakeInv)
	{

	}
	public void OnSnakeItemDrop(global::StickyItem obj, global::SnakeInventory snakeInv)
	{
		if(completedCount>0){
			completedCount = 0;
			StopPointing();
		}
	}
	#endregion
	public List<GroceryItem> items = new List<GroceryItem>();
	[InspectorDisabled]
	public int completedCount = 0;
	RectTransform rect;
	public float moveAmount;
	public float animationDuration = 1f;
	public float scaleAmount = 2f;
	public Ease easing;
	private Tween tween;
	private Vector3 localPosition;
	private Image image;
	Sequence mySequence;
	public Sprite active, inActive;
	public GroceryItem connectedGroceries;

	protected override void EarlyAwake()
	{
		image = GetComponent<Image>();
		rect = GetComponent<RectTransform>();
		localPosition = rect.localPosition;
		foreach (var x in items) {
			x.itemCompleteEvent += OnItemDone;

		}
	}

	void OnItemDone(bool droppingItem){
		if(!droppingItem)
			completedCount++;
		else
			completedCount--;

		if(completedCount>=items.Count){
			StartPointing();
		}
		else {
			StopPointing();
		}
	}
	[InspectorButton]
	void StartPointing(){
		//DOLocalMoveX/DOLocalMoveY/DOLocalMoveZ(float to, float duration, bool snapping)
		image.sprite = active;
		mySequence = DOTween.Sequence();
		mySequence.Append(rect.DOLocalMoveX(moveAmount, animationDuration, false)
		.SetEase(easing)).SetLoops(-1, LoopType.Yoyo)
		.Insert(0f, rect.DOScale(scaleAmount, animationDuration)
		.SetEase(easing))
		.SetLoops(-1, LoopType.Yoyo);

		//tween = rect.DOLocalMoveX(moveAmount, animationDuration, false).SetEase(easing).SetLoops(-1, LoopType.Yoyo);

	}
	[InspectorButton]
	void StopPointing(){
		mySequence.Kill(false);
		image.sprite = inActive;
		rect.localPosition = localPosition;
	}
	#region IObjectiveEvents implementation
	public void OnObjectiveItemPickup(LevelObjective lvlObj, BaseObject item)
	{

	}
	public void OnObjectiveComplete(LevelObjective lvlObj)
	{
		if(connectedGroceries.connectedObjective == lvlObj){
			StopPointing();
		}
	}
	#endregion
}
