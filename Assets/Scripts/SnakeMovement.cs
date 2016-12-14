using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[RequireComponent (typeof(MovementVisualizer))]
[RequireComponent(typeof(SnakeInventory))]
public class SnakeMovement : BaseObject, ISnakeCollisionEnter, ISnakeItemPickup, ISnakeItemDrop, IEventInvoker{

    public Camera theCamera;
    float speed = Physics.gravity.magnitude * 0.5f; //MAGIC NUMBER!! DO NOT TOUCH!!
    float fakeDrag = 0;
    Rigidbody rb;
    float forceDistribution = 2; //Used for distributing the force applied on the snake
    float attachedMass; //Total mass of all objects sticking to the snake
    float snakeMass; //The total mass of the snake
    float jointAmount; //How many joints the snake has
    List<Transform> joints = new List<Transform>();
    float maxJumpForce = 1.8f;
    public SnakeInventory snakeInventory;
    MovementVisualizer moveVisual;
    public Transform jointHolder;
    float jumpCooldown = 0;
    CameraFollow cameraFollow;

	// Use this for initialization
	protected override void LateAwake () {
        rb = GetComponent<Rigidbody>();
        if(theCamera==null){
            theCamera = GameObject.FindObjectOfType<CameraFollow>().GetComponent<Camera>();
        }
        cameraFollow = theCamera.transform.GetComponent<CameraFollow>();
        foreach(Transform t in jointHolder)
        {
            snakeMass += t.GetComponent<Rigidbody>().mass;
            joints.Add(t);
            jointAmount++;
            moveVisual = GetComponent<MovementVisualizer>();
        }
	}

	// Update is called once per frame
	void Update ()
    {
        jumpCooldown -= Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && jumpCooldown <= 0)
        {
            bool UIblockingRay = false;
            UnityEngine.EventSystems.EventSystem ct = UnityEngine.EventSystems.EventSystem.current;
            if(ct!=null)
            if(ct.currentSelectedGameObject != null){
                if (ct.currentSelectedGameObject.GetComponent<UnityEngine.UI.Button>() != null ){
                    UIblockingRay = true;
                }
            }

            if(!UIblockingRay)
            {
                bool jumped = false;
                LayerMask ignoreSnakeMask = (1 << 0);
                foreach (Transform child in jointHolder)
                {
                    RaycastHit[] kasteSfaere = Physics.SphereCastAll(child.position, 0.1f, Vector3.down, 0.15f, ignoreSnakeMask);
                    foreach(RaycastHit hit in kasteSfaere)
                    {
                        if(hit.transform.gameObject.layer != 8)
                        {
                            SnakeJump();
                            jumped = true;
                            break;
                        }

                    }

                    if (jumped) break;
                }
                if (!jumped)
                {
                    foreach (StickyItem si in snakeInventory.inventory)
                    {
                        RaycastHit[] kasteSfaere = Physics.SphereCastAll(si.transform.position, 0.1f, Vector3.down, 0.15f, ignoreSnakeMask);
                        foreach (RaycastHit hit in kasteSfaere)
                        {
                            if (hit.transform.gameObject.layer != 8)
                            {
                                SnakeJump();
                                jumped = true;
                                break;
                            }

                        }
                        if (jumped) break;
                    }
                }
            }
        }
	}

    void SnakeJump()
    {
        jumpCooldown = 0.2f;
        RaycastHit hit;
        Ray screenRay = theCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(screenRay, out hit))
        {
            cameraFollow.SnakeMoved(hit.point);
            moveVisual.SuccessfulJumpRequest(hit);
            eventManager.InvokeEvent<ISnakeEvents>(SnakeEvent.Jump);
            joints[0].GetComponent<Rigidbody>().AddForce(CalculateForce(joints[0], hit.point), ForceMode.VelocityChange);
            for (int i = 1; i < joints.Count; i++)
            {
                if (joints[i].position.y >= joints[i - 1].position.y - 0.05)
                {
                    joints[i].GetComponent<Rigidbody>().AddForce(CalculateForce(joints[i], hit.point), ForceMode.VelocityChange);
                }
                else
                {
                    float dist = joints[0].position.y - joints[i].position.y;
                    Vector3 calculatedForce = Vector3.up * dist * dist;
                    joints[i].GetComponent<Rigidbody>().AddForce(calculatedForce, ForceMode.VelocityChange);
                    if (snakeInventory.jointInventory.ContainsKey(joints[i].GetComponent<SnakeCollider>()))
                    {
                        foreach (StickyItem si in snakeInventory.jointInventory[joints[i].GetComponent<SnakeCollider>()])
                        {
                            float objMass = si.rigid.mass;
                            si.rigid.AddForce(calculatedForce * objMass, ForceMode.VelocityChange);
                        }
                    }
                }
            }
        }
        forceDistribution = 2;
    }

    Vector3 CalculateForce(Transform joint, Vector3 hitPoint)
    {
        float jointMass = joint.GetComponent<Rigidbody>().mass;
        float dist = Vector3.Distance(joint.position, hitPoint);
        Vector3 forDir = (hitPoint - joint.position).normalized;
        Vector3 calculatedForce = forDir * speed + Vector3.up * Mathf.Min(dist, Physics.gravity.magnitude);
        float attachedMassJoint = 0;
        if (snakeInventory.jointInventory.ContainsKey(joint.GetComponent<SnakeCollider>()))
        {
            foreach (StickyItem si in snakeInventory.jointInventory[joint.GetComponent<SnakeCollider>()])
            {
                float objMass = si.rigid.mass;
                attachedMassJoint += objMass;
                si.rigid.AddForce(calculatedForce * objMass, ForceMode.VelocityChange);
            }
        }
        calculatedForce = calculatedForce * forceDistribution;
        //calculatedForce = calculatedForce.normalized * Mathf.Min(calculatedForce.magnitude, maxJumpForce + attachedMassJoint);
        forceDistribution *= 0.75f; //Magic number, sorry. Haven't figured this one out yet...
        return calculatedForce;
    }

    void FixedUpdate()
    {
        foreach(Transform child in jointHolder)
        {
            child.GetComponent<Rigidbody>().velocity -= child.GetComponent<Rigidbody>().velocity * fakeDrag;
            fakeDrag = Mathf.Clamp(fakeDrag - Time.fixedDeltaTime, 0, 1);
        }
    }

    public void OnSnakeCollisionEnter(Collision col, SnakeCollider snakeCol)
    {
        BaseObject obj;
        ColliderCache.cachedObjects.TryGetValue(col.collider, out obj);

        //SnakeCollider snakeCol = obj as SnakeCollider;
        if (!(obj is SnakeCollider))
        {
            //fakeDrag = 1; //Uncomment to get extreme friction, when hitting stuff...
        }
    }

    public void OnSnakeCollisionExit(Collision col, SnakeCollider snakeCol)
    {
        //throw new NotImplementedException();
    }

    public void OnSnakeItemPickup(StickyItem obj, SnakeInventory snakeInv)
    {
        attachedMass += obj.rigid.mass;
        snakeInventory = snakeInv;
    }

    public void OnSnakeItemDrop(StickyItem obj, SnakeInventory snakeInv)
    {
        attachedMass -= obj.rigid.mass;
    }
}
