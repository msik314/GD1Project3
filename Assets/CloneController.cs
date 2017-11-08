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
    [SerializeField] private float interactVertOffset;
    [SerializeField] private float interactCameraDistance;

    public bool canMove;
    public bool hasBucket;

    [SerializeField] private float verticalClimbAngle;
    
    private float jumpVel;
    private bool grounded;
    private MovementRecord record;
    private Rigidbody rb;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private Vector2 rot;
	private Animator anim;

    private Vector3 vertComp;

    // Use this for initialization
    void Awake()
    {
		anim = GetComponent<Animator> ();
        rb = GetComponent<Rigidbody>();
        record = GetComponent<MovementRecord>();
        jumpVel = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * jumpHeight);
        grounded = false;
        canMove = true;
        hasBucket = false;
        originalPos = transform.position;
        originalRot = transform.rotation;
        vertComp = new Vector3(0, -1/Mathf.Tan(verticalClimbAngle), 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 targetVel;
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

        transform.rotation = Quaternion.Euler(0, rot.y, 0);

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



		float totalVelocity = Mathf.Abs(targetVel.x) + Mathf.Abs(targetVel.y) + Mathf.Abs(targetVel.z);
		anim.SetFloat ("curVelocity", totalVelocity);
    }

    void OnCollisionStay(Collision col)
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
        Vector3 scale = transform.localScale;
        transform.localScale = Vector3.one;
        if (hasBucket){
            throwWater();
        }
        else{
            
            Vector3 p = new Vector3(0, 0, Camera.main.nearClipPlane - interactCameraDistance);
            print(new Vector3(0, interactVertOffset, 0));
            print(Quaternion.Euler(rot.x, 0, 0));
            print(p);
            p = transform.TransformPoint(new Vector3(0, interactVertOffset, 0) + (Quaternion.Euler(rot.x, 0, 0) * p));
            Vector3 d = transform.rotation * Quaternion.Euler(rot.x, 0, 0) * Vector3.forward;
            Debug.DrawRay(p, d * interactDistance, Color.black, 50f);
            RaycastHit hit;
            bool hasHit = Physics.Raycast(p, d, out hit, interactDistance, interactMask);
            if (hasHit)
            {
                if (hit.collider.gameObject.tag == "Interactable")
                {
                    hit.collider.gameObject.GetComponent<InteractControl>().doInteraction(this.transform);
                }
            }
        }
        transform.localScale = scale;
    }

    void throwWater(){
        
        Vector3 p = new Vector3(0, 0, Camera.main.nearClipPlane - interactCameraDistance);
        p = transform.TransformPoint(new Vector3(0, interactVertOffset, 0) + (Quaternion.Euler(rot.x, 0, 0) * p));
        Vector3 d = transform.rotation * Quaternion.Euler(rot.x, 0, 0) * Vector3.forward;
        RaycastHit hit;
        bool hasHit = Physics.Raycast(p, d, out hit, interactDistance, interactMask);
        if (hasHit)
        {
            if (hit.collider.gameObject.tag == "Fire")
            {
                hit.collider.gameObject.GetComponent<InteractControl>().doInteraction(this.transform);
            }
        }
        hasBucket = false;
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
        canMove = true;
        hasBucket = false;
        gameObject.SetActive(true);
        rb.velocity = Vector3.zero;
        canMove = true;
    }
}
