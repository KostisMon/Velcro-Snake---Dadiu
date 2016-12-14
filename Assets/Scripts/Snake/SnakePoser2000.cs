using UnityEngine;
using System.Collections;

public class SnakePoser2000 : BaseObject {
	public Transform head01, head02, jaw, neck01, neck02, back01, back02, back03, tail01, tail02;

	public void Pose(SnakePoser2000 incoming){
		incoming.head01.parent = head01;
		incoming.head02.parent = head02;
		incoming.jaw.parent    = jaw;
		incoming.neck01.parent = neck01;
		incoming.neck02.parent = neck02;
		incoming.back01.parent = back01;
		incoming.back02.parent = back02;
		incoming.back03.parent = back03;
		incoming.tail01.parent = tail01;
		incoming.tail02.parent = tail02;

		incoming.head01.position = head01.position;
		incoming.head02.position = head02.position;
		incoming.jaw.position    = jaw.position;
		incoming.neck01.position = neck01.position;
		incoming.neck02.position = neck02.position;
		incoming.back01.position = back01.position;
		incoming.back02.position = back02.position;
		incoming.back03.position = back03.position;
		incoming.tail01.position = tail01.position;
		incoming.tail02.position = tail02.position;

		incoming.head01.rotation = head01.rotation;
		incoming.head02.rotation = head02.rotation;
		incoming.jaw.rotation = jaw.rotation;
		incoming.neck01.rotation = neck01.rotation;
		incoming.neck02.rotation = neck02.rotation;
		incoming.back01.rotation = back01.rotation;
		incoming.back02.rotation = back02.rotation;
		incoming.back03.rotation = back03.rotation;
		incoming.tail01.rotation = tail01.rotation;
		incoming.tail02.rotation = tail02.rotation;
	}
}
