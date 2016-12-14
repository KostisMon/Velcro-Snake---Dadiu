using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// The script teleports the snake from A to B.
/// </summary>

public class SnakeTransportInc : BaseObject
{
    public Transform GrabHook;
    public static bool ReleaseTheSnake;
    public float ToggleColliderTime;

    private Transform _originalVelcroSnake;
    private Rigidbody _rigid;
    private bool
        _initialParentSet,
        _readyToReleaseSnake,
        _toggleCollider;

    AudioSource audioS;

    void Awake()
    {
        audioS = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (ReleaseTheSnake && _readyToReleaseSnake)
        {
            Debug.Log("Release is called by plane");
            _readyToReleaseSnake = false;
            ActivateRelease();
        }
        if (_toggleCollider)
        {
            Debug.Log("Colliders are toggled back on");
            _toggleCollider = false;
            StartCoroutine("ReenableColliders", ToggleColliderTime);
        }
    }

    public void OnSnakeCollisionEnter(Collision col, SnakeCollider snakeCol)
    {
        _rigid = snakeCol.GetComponent<Rigidbody>();
        if (!_initialParentSet)
        {
            _initialParentSet = true;
            _originalVelcroSnake = snakeCol.transform.parent.parent;
        }
        // Objects stuck to the snake.
        foreach (var x in snakeCol.rootOfSnake.GetComponent<SnakeInventory>().inventory)
        {
            x.transform.parent = GrabHook;
        }
        _originalVelcroSnake.parent = GrabHook;

        for (int i = 0; i < _rigid.transform.parent.childCount; i++)
        {
            if (_rigid.transform.parent.GetChild(i).GetComponent<Rigidbody>() != null)
            {
                _rigid.transform.parent.GetChild(i).GetComponent<Rigidbody>().isKinematic = true;
                if (_rigid.transform.parent.GetChild(i).GetComponent<Collider>() != null)
                    _rigid.transform.parent.GetChild(i).GetComponent<Collider>().enabled = false;
            }
        }
        for (int i = 0; i < _rigid.transform.childCount; i++)
        {
            if (_rigid.transform.GetChild(i).GetComponent<Rigidbody>() != null)
            {
                _rigid.transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = true;
                if (_rigid.transform.GetChild(i).GetComponent<Collider>() != null)
                    _rigid.transform.GetChild(i).GetComponent<Collider>().enabled = false;
            }
        }
        // Objects stuck to the snake.
        foreach (var x in _rigid.GetComponent<SnakeCollider>().rootOfSnake.GetComponent<SnakeInventory>().inventory)
        {
            x.rigid.isKinematic = true;
            if (x.GetComponent<Collider>() != null)
                x.GetComponent<Collider>().enabled = false;
        }
        _readyToReleaseSnake = true;
        _toggleCollider = true;
    }

    public void OnSnakeCollisionExit(Collision col, SnakeCollider snakeCol)
    {

    }

    void ActivateRelease()
    {
        _originalVelcroSnake.parent = null;

        // Objects stuck to the snake.
        foreach (var x in _rigid.GetComponent<SnakeCollider>().rootOfSnake.GetComponent<SnakeInventory>().inventory)
        {
            x.transform.parent = null;
        }
        for (int i = 0; i < _rigid.transform.parent.childCount; i++)
        {
            if (_rigid.transform.parent.GetChild(i).GetComponent<Rigidbody>() != null)
            {
                _rigid.transform.parent.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
                _rigid.transform.parent.GetChild(i).GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                _rigid.transform.parent.GetChild(i).GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
            }
        }
        for (int i = 0; i < _rigid.transform.childCount; i++)
        {
            if (_rigid.transform.GetChild(i).GetComponent<Rigidbody>() != null)
            {
                _rigid.transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
                _rigid.transform.GetChild(i).GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
                _rigid.transform.GetChild(i).GetComponent<Rigidbody>().angularVelocity = new Vector3(0f, 0f, 0f);
            }
        }
        foreach (var x in _rigid.GetComponent<SnakeCollider>().rootOfSnake.GetComponent<SnakeInventory>().inventory)
        {
            x.rigid.isKinematic = false;
            x.rigid.angularVelocity = new Vector3(0f, 0f, 0f);
            x.rigid.velocity = new Vector3(0f, 0f, 0f);
        }
    }
    IEnumerator ReenableColliders(float time)
    {
        yield return new WaitForSeconds(time);
        for (int i = 0; i < _rigid.transform.parent.childCount; i++)
        {
            if (_rigid.transform.parent.GetChild(i).GetComponent<Rigidbody>() != null)
            {
                if (_rigid.transform.parent.GetChild(i).GetComponent<Collider>() != null)
                    _rigid.transform.parent.GetChild(i).GetComponent<Collider>().enabled = true;
            }
        }
        for (int i = 0; i < _rigid.transform.childCount; i++)
        {
            if (_rigid.transform.GetChild(i).GetComponent<Rigidbody>() != null)
            {
                if (_rigid.transform.GetChild(i).GetComponent<Collider>() != null)
                    _rigid.transform.GetChild(i).GetComponent<Collider>().enabled = true;
            }
        }
        foreach (var x in _rigid.GetComponent<SnakeCollider>().rootOfSnake.GetComponent<SnakeInventory>().inventory)
        {
            if (x.GetComponent<Collider>() != null)
                x.GetComponent<Collider>().enabled = true;
        }
    }
}