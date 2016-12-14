using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public interface IEvent{

}

public interface ILevelEnd : IEvent {
	void OnLevelEnd();
}

public interface ILevelStart : IEvent {
	void OnLevelStart();
}

public interface ISnakeItemPickup : IEvent {
	void OnSnakeItemPickup(StickyItem obj, SnakeInventory snakeInv);
}
public interface ISnakeItemDrop : IEvent {
	void OnSnakeItemDrop(StickyItem obj, SnakeInventory snakeInv);
}

public interface ISnakeTriggerEnter : IEvent {
	void OnSnakeTriggerEnter(Collider col, SnakeCollider snakeCol);
}

public interface ISnakeTriggerExit : IEvent {
	void OnSnakeTriggerExit(Collider col, SnakeCollider snakeCol);
}

public interface ISnakeCollisionEnter : IEvent {
	void OnSnakeCollisionEnter(Collision col, SnakeCollider snakeCol);
}

public interface ISnakeCollisionExit : IEvent {
	void OnSnakeCollisionExit(Collision col, SnakeCollider snakeCol);
}

public interface IPauseEnter : IEvent {
	void OnPauseEnter();
}
public interface IPauseExit : IEvent {
	void OnPauseExit();
}
public interface IObjectivePickup : IEvent {
    void OnObjectiveItemPickup(LevelObjective lvlObj, BaseObject item);
}
public interface IObjectiveComplete : IEvent {
    void OnObjectiveComplete(LevelObjective lvlObj);
}

public enum SnakeEvent {Jump};
public interface ISnakeEvents : IEvent {
    void OnSnakeEvent(SnakeEvent snakeEvent);
}

public interface IEndGameFadeOut : IEvent {
	void OnFadeOut(CameraFader cam);
}
public interface IEndGameFadeIn : IEvent {
	void OnFadeIn(CameraFader cam);
}
public interface IEndGameEnd : IEvent {
	void OnEnd();
}
