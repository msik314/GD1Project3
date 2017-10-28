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
    
    private float jumpVel;
    private bool grounded;
    private MovementRecord record;
    private Rigidbody rb;
    
    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        record = GetComponent<MovementRecord>();
        jumpVel = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * jumpHeight);
        grounded = false;
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        
        Vector3 targetVel;
        Quaternion rot;
        bool jumping = record.getTarget(out targetVel, out rot);
        transform.rotation = rot;
        
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
            rb.AddForce(Vector3.up * jumpVel, ForceMode.VelocityChange);
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
}
