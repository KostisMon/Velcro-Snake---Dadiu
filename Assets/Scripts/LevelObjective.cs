using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FullInspector;

[ExecuteInEditMode]
[RequireComponent(typeof(SphereCollider))]
public class LevelObjective : BaseObject , ISnakeItemPickup, ISnakeItemDrop, ISnakeTriggerExit, ISnakeTriggerEnter, IEventInvoker, ICollidable{
	[InspectorDisabled]
	public Color color;
	public enum CompletionType {CompleteOnContact, CompleteOnPickup};
	public enum CompleteState {Uncompleted,Completed};
	public CompletionType completionType;
	public Dictionary<BaseObject, CompleteState> objectives = new Dictionary<BaseObject, CompleteState>();
	SphereCollider collider;
    public bool hasFinished;
    public bool isMainObjective;
    private int collectedCount = 0;
    public bool destroyItemsOnCompletion = true;

    public bool AllItemsCollected{
    	get{
    		if(collectedCount>=objectives.Count)
    			return true;
    		else
    			return false;
    	}
    }

	private static List<Color> colors;
	void OnEnable(){
		if(colors==null){
			colors = new List<Color>();
			colors.Add(Color.cyan);
			colors.Add(Color.red);
			colors.Add(Color.green);
			colors.Add(new Color(255, 165, 0));
			colors.Add(Color.yellow);
			colors.Add(Color.magenta);

		}
		Color rand = colors[Random.Range(0,colors.Count-1)];
		color = rand;
	}

	public void OnSnakeItemPickup(StickyItem obj, SnakeInventory snakeInv){
		if(objectives.ContainsKey(obj) && !hasFinished)
        {
			objectives[obj] = CompleteState.Completed;
			collectedCount++;
			eventManager.InvokeEventFast<IObjectivePickup, LevelObjective, BaseObject>(this, obj);
			if(ObjectivesComplete() && completionType == CompletionType.CompleteOnPickup){
				eventManager.InvokeEventFast<IObjectiveComplete, LevelObjective>(this);
                hasFinished = true;
            }
		}
	}
	public void OnSnakeItemDrop(StickyItem obj, SnakeInventory snakeInv){
		if(objectives.ContainsKey(obj) && !hasFinished)
        {
			objectives[obj] = CompleteState.Uncompleted;
			collectedCount--;
		}
	}
	bool ObjectivesComplete(){
		bool completed = true;
		foreach (var x in objectives.Keys) {
			if(objectives[x] == CompleteState.Uncompleted){
				completed = false;
			}
		}
		if(completed){
			return true;
		}
		else return false;
	}

	public void OnSnakeTriggerEnter(Collider col, SnakeCollider snakeCol){
		BaseObject objective;
		ColliderCache.cachedObjects.TryGetValue(col, out objective);
		if(objective == this){
			if(ObjectivesComplete() && completionType == CompletionType.CompleteOnContact && !hasFinished)
	        {
                foreach(var x in objectives.Keys)
                {

                    if(destroyItemsOnCompletion)
                        x.gameObject.SetActive(false);
                }
                hasFinished = true;
				eventManager.InvokeEventFast<IObjectiveComplete, LevelObjective>(this);
	        }
		}
	}
	public void OnSnakeTriggerExit(Collider col, SnakeCollider snakeCol){

	}
    void OnDrawGizmos() {
    	if(collider!=null){
	    	Color faded = color;
	    	faded.a = 0.75f;
	    	Gizmos.color = faded;
			Gizmos.DrawSphere(transform.position,collider.radius);
    	}
    	Gizmos.color = color;
		if(objectives!=null){
			foreach (var x in objectives.Keys) {
				if(x!=null){
					Gizmos.DrawWireSphere(x.transform.position, 0.3f);
					Gizmos.DrawWireSphere(x.transform.position, 0.34f);
					Gizmos.DrawWireSphere(x.transform.position, 0.38f);
				}
			}
		}
    }
	protected override void LateAwake()
	{
		collider = GetComponent<SphereCollider>();
		collider.isTrigger = true;
	}
}
