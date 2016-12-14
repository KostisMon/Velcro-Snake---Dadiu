using UnityEngine;
using System.Collections;
using FullInspector;

public class GenericCollidableObject : BaseObject, ICollidable{
	[InspectorComment("Put this on something that does nothing except generate collision events\nDO NOT USE ALONG WITH OTHER BASEOBJECT SCRIPTS JESUSCHRISTUS")]
	[InspectorDisabled]
	public string empty;

}

public interface ICollidable{

}
