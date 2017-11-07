﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MovementRecord))]
public class MovementController : MonoBehaviour
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
    
    private Quaternion originalRot;
    private Vector3 originalPos;
    private Vector3 targetVel;
    private Rigidbody rb;
    private MovementRecord record;
    private bool grounded;
    private bool jumping;
    private bool interacting;
    private float jumpVel;
    private  CloneManager manager;

    private InteractControl interactScript;
    
    // Use this for initialization
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        record = GetComponent<MovementRecord>();
        record.init();
        jumpVel = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * jumpHeight);
        grounded = false;
        targetVel = Vector3.zero;
        originalPos = transform.position;
        originalRot = transform.rotation;
    }
    
    // Update is called once per frame
    void Update()
    {
        targetVel = maxSpeed * new Vector3(Input.GetAxis("Sideways"), 0, Input.GetAxis("Forward"));
        if(targetVel.sqrMagnitude > maxSpeed)
        {
            targetVel = maxSpeed * targetVel.normalized;
        }
        
        if(grounded && Input.GetButtonDown("Jump"))
        {
            jumping = true;
        }
        
        if(Input.GetButtonDown("Interact"))
        {
            interacting = true;
        }
        
        transform.Rotate(0, lookSensitivity * Input.GetAxisRaw("MouseX") * Time.deltaTime, 0);
    }
    
    void FixedUpdate()
    {
        if(targetVel.sqrMagnitude > Mathf.Epsilon)
        {
            targetVel = transform.rotation * targetVel;
        }
        
        byte actions = 0;
        
        if(interacting)
        {
            interact();
            actions |= 2;
        }
        if(jumping)
        {
            actions |= 1;
        }
        
        record.addTarget(targetVel, transform.rotation, actions);
        interacting = false;
        
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
        jumping = false;
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
        Ray r = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth/2, Camera.main.pixelHeight/2, 0));
        Debug.DrawRay(r.origin, new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2,0),Color.black, 5.0f);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(r, out hit, interactDistance, interactMask);
        print(hasHit);
        if(hasHit)
        {
            print("1");
            if(hit.collider.gameObject.tag == "Interactable")
            {
                print("2");
                hit.collider.gameObject.GetComponent<InteractControl>().doInteraction();
            }
        }
    }
    
    
    public void die()
    {
        manager.cycle();
    }
    
    public void reset()
    {
        record.clear();
        transform.position = originalPos;
        transform.rotation = originalRot;
    }
    
    public void setManager(CloneManager cloneManager)
    {
        manager = cloneManager;
    }
}
