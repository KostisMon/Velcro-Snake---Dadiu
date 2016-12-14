using UnityEngine;
using System.Collections;
using FullInspector;

[RequireComponent(typeof(Rigidbody))]
public class StickyItem : BaseObject, ICollidable{
	public Rigidbody rigid;
	public bool Stuck {get;set;}
    private bool canStick = true;
	public Sprite icon;
	[InspectorDisabled]
	public int UUID = -1;

	protected override void LateAwake() {
		this.gameObject.tag = "Sticky";
        this.gameObject.layer = 9;
		rigid = GetComponent<Rigidbody>();
	}

	public bool TryStickToSnake(Collision col, Rigidbody snakeJoint){
		if(!Stuck && canStick){
			Stuck = true;
	        CharacterJoint cj = gameObject.AddComponent<CharacterJoint>();
	        cj.autoConfigureConnectedAnchor = false;
	        cj.anchor = transform.InverseTransformPoint(col.contacts[0].point);
	        cj.connectedAnchor = col.transform.InverseTransformPoint(col.contacts[0].point);
	        cj.connectedBody = snakeJoint;
	        return true;
		}
		else return false;
	}

    public void StopBeingSticky(float t)
    {
        canStick = false;
        Stuck = false;
        Destroy(GetComponent<CharacterJoint>());
        if(this.gameObject.activeSelf)
        	StartCoroutine(StartBeingSticky(t));
    }

    IEnumerator StartBeingSticky(float t)
    {
        yield return new WaitForSeconds(t);
        canStick = true;
    }

	[InspectorButton]
	public void GenerateUniqueID(){
		UUID = this.gameObject.GetHashCode();
	}
}
