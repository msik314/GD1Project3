﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MovementRecord))]
[RequireComponent(typeof(AudioSource))]
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
    [SerializeField] private float suffocateTime;
    [SerializeField] private float crushTime;
    [SerializeField] private float verticalClimbAngle;

    public bool canMove;
    public bool hasBucket;

    private bool canRotate;
    private Quaternion originalRot;
    private Vector3 originalPos;
    private Vector3 targetVel;
    private Rigidbody rb;
    private MovementRecord record;
    private bool grounded;
    private bool jumping;
    private bool interacting;
    private float jumpVel;
	private Animator anim;
    private CloneManager manager;
    private AudioSource source;
    private InteractControl interactScript;
    private Vector3 vertComp;
    private bool dead;
    
    // Use this for initialization
    void Awake()
    {
		anim = GetComponent<Animator> ();
        rb = GetComponent<Rigidbody>();
        record = GetComponent<MovementRecord>();
        record.init();
        jumpVel = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * jumpHeight);
        grounded = false;
        canMove = true;
        canRotate = true;
        hasBucket = false;
        targetVel = Vector3.zero;
        originalPos = transform.position;
        originalRot = transform.rotation;
        vertComp = new Vector3(0, -1/Mathf.Tan(verticalClimbAngle), 0);
        source = GetComponent<AudioSource>();
        dead = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        targetVel = maxSpeed * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
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
        if(canRotate)
        {
            transform.Rotate(0, lookSensitivity * Input.GetAxisRaw("MouseX") * Time.deltaTime, 0);
        }
        
        if(Input.GetButtonDown("Reset"))
        {
            manager.reset();
        }

    }
    
    void FixedUpdate()
    {
        if(targetVel.sqrMagnitude > Mathf.Epsilon)
        {
            targetVel = transform.rotation * targetVel;
        }
        
        byte actions = 0;

        if (!canMove)
        {
            targetVel = Vector3.zero;
        }
        else
        {
            if (interacting)
            {
                interact();
                actions |= 2;
            }
            if (jumping)
            {
                actions |= 1;
            }
        }
        
        record.addTarget(targetVel, new Vector2(Camera.main.gameObject.transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y), actions);
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
            rb.AddForce((difference.normalized + vertComp) * maxAccel, ForceMode.Acceleration);
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

		float totalVelocity = Mathf.Abs(targetVel.x) + Mathf.Abs(targetVel.y) + Mathf.Abs(targetVel.z);
		//print (grounded);

		if (Input.GetKeyDown (KeyCode.U)) {
			GameObject testing = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			Destroy(testing.GetComponent<Rigidbody> ());
			testing.transform.localScale = testing.transform.localScale * castRadius;
			print (castHeight);
			testing.transform.position = new Vector3 (transform.position.x, transform.position.y + castHeight, transform.position.z);

		}


		anim.SetFloat ("curVelocity", totalVelocity);

    }
    
    void OnCollisionStay(Collision col)
    {
		
		if(Physics.CheckSphere(new Vector3(transform.position.x, transform.position.y + castHeight, transform.position.z), castRadius))
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
        if (hasBucket) {
            throwWater();
			anim.SetBool ("throwWater", true);
			anim.SetBool ("bucket", hasBucket);
        }
        else{
            Ray r = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0));
            RaycastHit hit;
            bool hasHit = Physics.Raycast(r, out hit, interactDistance, interactMask);
            if (hasHit)
            {
                if (hit.collider.gameObject.tag == "Interactable")
                {
                    hit.collider.gameObject.GetComponent<InteractControl>().doInteraction(this.transform);
					anim.SetBool ("bucket", hasBucket);
					anim.SetBool ("deb", !canMove);
                }
            }
        }
    }
    
    void throwWater(){
        Ray r = Camera.main.ScreenPointToRay(new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2, 0));
        RaycastHit hit;
        bool hasHit = Physics.Raycast(r, out hit, interactDistance, interactMask);
        if (hasHit){
            if(hit.collider.gameObject.tag == "Fire"){
                hit.collider.gameObject.GetComponent<InteractControl>().doInteraction(this.transform);
            }
        }
        hasBucket = false;
    }
    
    public void die()
    {
        if(!dead)
        {
            dead = true;
            source.Play();
            anim.Play("Death");
            canMove = false;
            canRotate = false;
            StartCoroutine(dieWait());
        }
    }
    
    public void crush()
	{
        if(!dead)
        {
            dead = true;
            anim.SetBool ("death", true);
            canMove = false;
            canRotate = false;
            StartCoroutine(crushWait());
        }
    }
    
    public void reset()
    {
		anim.SetBool ("throwWater", false);
		anim.SetBool ("bucket", false);
		anim.SetBool ("deb", false);
        source.Stop();
		anim.SetBool ("death", false);
		anim.Play ("Idle");
        record.clear();
        canMove = true;
        canRotate = true;
        hasBucket = false;
        transform.position = originalPos;
        transform.rotation = originalRot;
        rb.velocity = Vector3.zero;
    }
    
    public void setManager(CloneManager cloneManager)
    {
        manager = cloneManager;
    }
    
    IEnumerator dieWait()
    {
        yield return new WaitForSeconds(suffocateTime);
        dead = false;
        manager.cycle();
    }
    
    IEnumerator crushWait()
    {
        yield return new WaitForSeconds(crushTime);
        dead = false;
        manager.cycle();
    }
}
