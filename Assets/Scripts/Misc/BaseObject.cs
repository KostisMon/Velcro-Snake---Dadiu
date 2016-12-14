using UnityEngine;
using System.Collections;

public class BaseObject : FullInspector.BaseBehavior {
	private static EventManager _eventManager;
	protected EventManager eventManager{
		get{
			if(this is IEventInvoker){
				return _eventManager;
			}
			else {
				throw new System.InvalidOperationException(this.gameObject + " does not implement IEventInvoker and cannot touch EventManager");
			}
		}
		private set{
			_eventManager = value;
		}
	}

	private void Awake(){
		base.Awake();
		LinkEvents();
		CacheCollider();
		EarlyAwake();
		LateAwake();
	}

	protected virtual void EarlyAwake () {}
	protected virtual void LateAwake () {}

	private void CacheCollider(){
		//We cache this object in relation to its colliders. Then we get eaiser lookups later without tag comparisons or GetComponent<>, etc.
		if(this is ICollidable)
		{
			var colliders = GetComponentsInChildren<Collider>();
			foreach (var x in colliders) {
				ColliderCache.CacheObject(x, this);
			}
		}

	}

	private void LinkEvents(){
		bool failed = false;
		if(!DoesObjectExist(_eventManager)){
			_eventManager = FindObjectOfType<EventManager>();

			if(!DoesObjectExist(_eventManager)){
				Debug.Log("WARNING: Cannot find Event Manager in scene!!!");
				failed = true;
				return;
			}
			else {
				_eventManager = FindObjectOfType<EventManager>();
			}
		}

		if(!failed){
			EventLinker.LinkEvents(this, _eventManager);
        }
	}

    protected bool DoesObjectExist(Object obj){
		return obj == null ? false : true;
	}
}
