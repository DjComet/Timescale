using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositiveTimeScript : MonoBehaviour {
    float dt;

    public bool useDifferential;
    private Rigidbody rb;
    private ScaledDeltaTime scaledDeltaTime;

    private Vector3 previousVelocity;
    private Vector3 previousAngVelocity;
    private float previousMass;
    private float previousDrag;
    private float previousAngDrag;

    public float velocityMagnitude;
    public Vector3 velocity;
    public float angularMagnitude;
    public Vector3 angVelocity;

    private float ownTimescale;
    public float previousTimeScale;
    public float timeScaleDifferential;

    public bool reapplyForces = false;
    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        scaledDeltaTime = gameObject.GetComponent<ScaledDeltaTime>();
        
    }
	
	// Update is called once per frame
	void Update () {

        dt = scaledDeltaTime.scaledDT;
        ownTimescale = scaledDeltaTime.ownTimeScale;//newTimeScale
        

        timeScaleDifferential = Mathf.Abs(ownTimescale / previousTimeScale);

        /*if(timeScaleDifferential > 1)
        {
            useDifferential = false;
        }
        else
        {
            useDifferential = true;
        }*/
        

        velocityMagnitude = Vector3.Magnitude(rb.velocity);
        velocity = rb.velocity;
        angularMagnitude = Vector3.Magnitude(rb.angularVelocity);
        angVelocity = rb.angularVelocity;

        if (ownTimescale == 1)
        {//If Normal time
            rb.isKinematic = false;

            previousMass = rb.mass;
            previousVelocity = rb.velocity;
            previousAngVelocity = rb.angularVelocity;
            previousDrag = rb.drag;
            previousAngDrag = rb.angularDrag;
           
        }
        else if (ownTimescale == 0 && scaledDeltaTime.actualTarget == 0)
        {//if PAUSED
            //Debug.Log("Co que cojones");
            rb.isKinematic = true;
            reapplyForces = true;
        }
        else if ((ownTimescale > 0 && ownTimescale < 1) && (scaledDeltaTime.actualTarget == 0.2f || scaledDeltaTime.actualTarget == 0))
        {//if going to PAUSE or going to SLOW
            rb.isKinematic = false;

            if(useDifferential)
            {
                rb.mass /= timeScaleDifferential;
                rb.velocity *= timeScaleDifferential;
                rb.angularVelocity *= timeScaleDifferential;
                //rb.drag /= timeScaleDifferential;
                //rb.angularDrag /= timeScaleDifferential;
            }
            else
            {
                rb.mass = previousMass / ownTimescale;
                rb.velocity = previousVelocity * ownTimescale;
                rb.velocity += Physics.gravity * ownTimescale;
                rb.angularVelocity = previousAngVelocity * ownTimescale;
                //rb.drag = previousDrag / ownTimescale;
                //rb.angularDrag = previousAngDrag / ownTimescale;
            }    
        }
        else if(ownTimescale > 1)
        {//if ACCELERATING
            if (useDifferential)
            {
                rb.mass /= timeScaleDifferential;
                rb.velocity *= timeScaleDifferential;
                rb.angularVelocity *= timeScaleDifferential;
                
            }
            else
            {
                rb.mass = previousMass / ownTimescale;
                rb.velocity = previousVelocity * ownTimescale;
                rb.angularVelocity = previousAngVelocity * ownTimescale;
            }
        }
        
        if(scaledDeltaTime.previousTarget == 0 && scaledDeltaTime.actualTarget > 0)
        {//if just unpaused
            if (reapplyForces)
            {
                rb.mass = previousMass;
                rb.velocity = previousVelocity;
                rb.angularVelocity = previousAngVelocity;
                reapplyForces = false;
            }
        }
        

        previousTimeScale = ownTimescale;//this stores the previous ownTimeScale value for the next frame.
        previousTimeScale = Mathf.Clamp(previousTimeScale, Mathf.Epsilon, 50);
    }
}
