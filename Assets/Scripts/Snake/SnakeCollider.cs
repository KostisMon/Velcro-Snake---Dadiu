using UnityEngine;
using System.Collections;
using FullInspector;


public class SnakeCollider : BaseObject, IEventInvoker{
	[InspectorComment("Add this to every joint in snake!")]
	public Rigidbody rigid;
    public SnakeAttacher snakeAttacher;
    public Collider nextCollider;
    public Transform rootOfSnake;
    public bool isHead;

    protected override void EarlyAwake()
    {
        Collider col = GetComponent<Collider>();
        if (nextCollider != null)
        {
            Physics.IgnoreCollision(col, nextCollider, true);
        }
    }

    protected override void LateAwake() {
		if(rigid==null){
			rigid = GetComponent<Rigidbody>();
		}
	}

	void OnCollisionEnter(Collision col){
		eventManager.InvokeEventFast<ISnakeCollisionEnter, Collision, SnakeCollider>(col, this);
	}
	void OnCollisionExit(Collision col){
		eventManager.InvokeEventFast<ISnakeCollisionExit, Collision, SnakeCollider>(col, this);
	}

	void OnTriggerEnter(Collider col){
		eventManager.InvokeEventFast<ISnakeTriggerEnter, Collider, SnakeCollider>(col, this);
	}
	void OnTriggerExit(Collider col){
		eventManager.InvokeEventFast<ISnakeTriggerExit, Collider, SnakeCollider>(col, this);
	}
}
