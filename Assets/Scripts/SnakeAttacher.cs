using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FullInspector;
//This is for attaching the snake directly to something, eg. airplane, ceiling fan, walls
public class SnakeAttacher : BaseObject {
	private List<SnakeCollider> snakeJoints = new List<SnakeCollider>();
	private List<CharacterJoint> connectedJoints = new List<CharacterJoint>();
	float orgDrag;
	public void Connect(Rigidbody target, Vector3 anchor){
		foreach (var x in snakeJoints) {
			var rigid = x.transform.GetComponent<Rigidbody>();
			if(rigid!=null){
		        CharacterJoint cj = x.gameObject.AddComponent<CharacterJoint>();
		        cj.autoConfigureConnectedAnchor = false;
		        cj.anchor = x.transform.InverseTransformPoint(target.transform.position);
		        cj.connectedAnchor = anchor;
		        cj.connectedBody = target;
		        connectedJoints.Add(cj);
		        orgDrag = rigid.drag;
		        rigid.drag = 2f;
			}

		}
	}

	[InspectorButton]
	public void Disconnect(){
		foreach (var x in connectedJoints) {
			Destroy(x);
		}
		foreach (var x in snakeJoints) {
			var rigid = x.transform.GetComponent<Rigidbody>();
			if(rigid!=null){
		        rigid.drag = orgDrag;
			}

		}
	}

	protected override void EarlyAwake()
	{
		snakeJoints.AddRange(GetComponentsInChildren<SnakeCollider>());
	}
}
