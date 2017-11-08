using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MovementRecord))]

public class CloneController : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float physicsCutoff;
    [SerializeField] private float maxAccel;
    [SerializeField] private float airAccel;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float castHeight;
    [SerializeField] private float castRadius;
    [SerializeField] private LayerMask castMask;
    [SerializeField] private float interactDistance;
    [SerializeField] private LayerMask interactMask;
    [SerializeField] private Vector3 interactOffset;
    [SerializeField] private Vector3 interactEuler;

    private float jumpVel;
    private bool grounded;
    private MovementRecord record;
    private Rigidbody rb;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private bool canMove;

    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        record = GetComponent<MovementRecord>();
        jumpVel = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * jumpHeight);
        grounded = false;
        originalPos = transform.position;
        originalRot = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 targetVel;
        Quaternion rot;
        bool jumping = false;
        bool interacting = false;
        byte actions = record.getTarget(out targetVel, out rot);

        if (!canMove)
        {
            targetVel = Vector3.zero;
        }
        else
        {
            jumping = (actions & 1) != 0;
            interacting = (actions & 2) != 0;
        }

        transform.rotation = rot;

        if(interacting)
        {
            interact();
        }

        Vector3 difference = new Vector3(targetVel.x - rb.velocity.x, 0, targetVel.z - rb.velocity.z);

        rb.useGravity = true;

        if(difference.sqrMagnitude < physicsCutoff * physicsCutoff)
        {
            if(rb.velocity.sqrMagnitude < physicsCutoff * physicsCutoff)
            {
                rb.velocity = Vector3.up * rb.velocity.y;
                if(grounded)
                {
                    rb.useGravity = false;
                }
            }
            else
            {
                rb.AddForce(difference, ForceMode.VelocityChange);
            }
        }
        else if(grounded && difference.sqrMagnitude > maxAccel * maxAccel * Time.fixedDeltaTime * Time.fixedDeltaTime)
        {
            rb.AddForce(difference.normalized * maxAccel, ForceMode.Acceleration);
        }
        else if(difference.sqrMagnitude > airAccel * airAccel * Time.fixedDeltaTime * Time.fixedDeltaTime)
        {
            rb.AddForce(difference.normalized * airAccel, ForceMode.Acceleration);
        }
        else
        {
            rb.AddForce(difference * Time.fixedDeltaTime, ForceMode.Acceleration);
        }

        if(grounded && jumping)
        {
            grounded = false;
            rb.AddForce(Vector3.up * (jumpVel - rb.velocity.y), ForceMode.VelocityChange);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(Physics.CheckSphere(transform.TransformPoint(new Vector3(0, castHeight, 0)), castRadius, castMask))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }

    void OnCollisionExit(Collision col)
    {
        if(grounded)
        {
            if(Physics.CheckSphere(transform.TransformPoint(new Vector3(0, castHeight, 0)), castRadius, castMask))
            {
                grounded = true;
            }
            else
            {
                grounded = false;
            }
        }
    }

    void interact()
    {
        Vector3 p = transform.TransformPoint(interactOffset + new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 d = transform.rotation * Quaternion.Euler(interactEuler) * Vector3.forward;
        RaycastHit hit;
        bool hasHit = Physics.Raycast(p, d, out hit, interactDistance, interactMask);
        if(hasHit)
        {
            if(hit.collider.gameObject.tag == "Interactable")
            {
                hit.collider.gameObject.GetComponent<InteractControl>().doInteraction(this.transform) ;//temporary
            }
        }
    }

    public void die()
    {
        gameObject.SetActive(false);
    }

    public void reset()
    {
        record.reset();
        transform.position = originalPos;
        transform.rotation = originalRot;
        gameObject.SetActive(true);
    }
}
