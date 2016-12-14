using UnityEngine;
using System.Collections;
using System;

public class Rotator : BaseObject, ISnakeTriggerEnter,ISnakeTriggerExit {

    public Rigidbody hinge;
    private bool snakeOnTrigger;
    private Quaternion tarAngle;
    public bool ceilingFanOn = false;
    public bool rotateDoorOn = false;
    public bool menuDoorOn = false;
    public float rotSpeed = 0.5f;
    //public float rotAngle = 180f;

    public void OnSnakeTriggerEnter(Collider col, SnakeCollider snakeCol)
    {

        snakeOnTrigger = true;
    }

    public void OnSnakeTriggerExit(Collider col, SnakeCollider snakeCol)
    {

    }

    void FixedUpdate()
    {
        if (snakeOnTrigger && rotateDoorOn)
        {
            RotateThings(90f, 5f);
        }

        if (ceilingFanOn)
        {

            hinge.transform.Rotate(Vector3.up * rotSpeed);
        }


    }

    public void RotateThings(float rotAngle, float rotSpeed)
    {
        tarAngle = Quaternion.Euler(0, rotAngle, 0);
        hinge.transform.localRotation = Quaternion.Slerp(hinge.transform.localRotation, tarAngle, Time.deltaTime * rotSpeed);

    }


}
