using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColliderCache{
	public static Dictionary<Collider, BaseObject> cachedObjects = new Dictionary<Collider, BaseObject>();
	public static void CacheObject(Collider col, BaseObject obj){
		cachedObjects[col] = obj;
	}
}
