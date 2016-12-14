using UnityEngine;
using System.Collections;
using System;

public class BathDoorOpener : BaseObject, ISnakeTriggerEnter,ISnakeTriggerExit {

    public Transform bathDoorHinge;
    public Collider trigger;
    public bool snakeOnTrigger;
    private Quaternion tarAngle;
    public float rotSpeed = 0.5f;
    public float rotAngle = 180f;

    public void OnSnakeTriggerEnter(Collider col, SnakeCollider snakeCol)
    {
        if(col == trigger)
        {
            snakeOnTrigger = true;
        }
    }

    public void OnSnakeTriggerExit(Collider col, SnakeCollider snakeCol)
    {

    }

    void FixedUpdate()
    {
        if (snakeOnTrigger)
        {
            tarAngle = Quaternion.Euler(0, rotAngle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, tarAngle, Time.deltaTime * rotSpeed);
        }
    }


}
