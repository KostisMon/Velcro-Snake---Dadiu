using UnityEngine;
using System.Collections;

public class Teleport : BaseObject, ICollidable, ISnakeTriggerEnter , IObjectiveComplete, IEndGameFadeOut, IEndGameFadeIn{
	#region IEndGameEvents implementation
	public void OnFadeOut(global::CameraFader cam)
	{
		cameraFader = cam;

	}
	public void OnFadeIn(global::CameraFader cam)
	{
		MoveSnake();
		//throw new System.NotImplementedException();
	}
	public void OnEnd()
	{
		//throw new System.NotImplementedException();
	}
	#endregion
	#region IObjectiveEvents implementation
	public void OnObjectiveItemPickup(global::LevelObjective lvlObj, global::BaseObject item)
	{
		//throw new System.NotImplementedException();
	}
	public void OnObjectiveComplete(global::LevelObjective lvlObj)
	{
		if(lvlObj.isMainObjective){
			LockSnake(snakeMovement);
		}
	}
	#endregion

	public enum TeleType{Entrance,Exit};
	public TeleType type;
	public Transform destination;
	bool hasTeleported;
	public SnakePoser2000 poser;
	public CameraFader cameraFader;


	public SnakeMovement snakeMovement;


	public void MoveSnake(){
		hasTeleported = true;
		MoveSnake(destination, snakeMovement);
	}

	public void LockSnake(SnakeMovement snakeMovement){
		var cols = snakeMovement.GetComponentsInChildren<SnakeCollider>(true);

		foreach (var x in cols) {
			x.rigid.drag = 3f;
			x.rigid.angularDrag = 1f;
			//x.transform.parent = snakePoser.transform;
		}
		snakeMovement.enabled = false;
	}

	public void OnSnakeTriggerEnter(Collider col, SnakeCollider snakeCol)
	{
		if(snakeMovement==null){
			snakeMovement = snakeCol.rootOfSnake.GetComponent<SnakeMovement>();
		}
	}
	public void OnSnakeTriggerExit(Collider col, SnakeCollider snakeCol){}

	public void MoveSnake(Transform target, SnakeMovement movement){
		movement.enabled = false;
		//var orgParent = movement.transform;
		var orgPosition = transform.position;
		var holder = movement.jointHolder;
		var snakePoser = holder.GetComponent<SnakePoser2000>();
		var cols = holder.GetComponentsInChildren<SnakeCollider>(true);
		var head = holder.GetChild(0);

		foreach (var x in cols) {
			x.rigid.isKinematic = true;
			x.transform.parent = snakePoser.transform;
		}
		//holder.position = head.transform.position;

		//transform.position = target.position;
		//transform.forward = target.forward;

		//foreach (var x in cols) {
		//	x.transform.parent = holder;
		//}

		if(snakePoser!=null)
			poser.Pose(snakePoser);
		else
			Debug.Log("oh god I'm dying snakeposer is not found on snakerig");

		//transform.position = orgPosition;
		//holder.position = target.position;
	}
}
