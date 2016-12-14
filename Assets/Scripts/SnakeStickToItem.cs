using UnityEngine;
using System.Collections;
using System;

public class SnakeStickToItem : BaseObject
{

    private SnakeAttacher snakeAttacher;
    private SnakeAttacher snakeOnItem;
    private bool snakeIsStuck = false;
    private Rigidbody itemRb;

    void Start()
    {
        itemRb = GetComponent<Rigidbody>();
    }

    public void OnSnakeCollisionEnter(Collision col, SnakeCollider snakeCol)
    {

        if (col.rigidbody == itemRb && !snakeIsStuck) {

            ContactPoint contact = col.contacts[0];
            snakeOnItem = snakeCol.snakeAttacher;
            snakeCol.snakeAttacher.Connect(itemRb, transform.InverseTransformPoint(contact.point));
            snakeIsStuck = true;
        }
    }

    public void OnSnakeCollisionExit(Collision col, SnakeCollider snakeCol)
    {

    }

    public void DisconnectFromItem()
    {
        StartCoroutine(WaitAndDisconnect(1.0F));
    }

    IEnumerator WaitAndDisconnect(float waitTime)
    {
        snakeOnItem.Disconnect();
        yield return new WaitForSeconds(waitTime);
        snakeIsStuck = false;

    }
}
