using UnityEngine;
using System.Collections;
[RequireComponent(typeof(SnakeMovement))]
public class SnakePauseBehaviour : BaseObject, IPauseEnter, IPauseExit {
	public SnakeMovement snakeMovement;
	protected override void LateAwake () {
        snakeMovement = GetComponent<SnakeMovement>();
	}

	public void OnPauseEnter(){
		snakeMovement.enabled = false;
	}
	public void OnPauseExit(){
		snakeMovement.enabled = true;
	}
}
