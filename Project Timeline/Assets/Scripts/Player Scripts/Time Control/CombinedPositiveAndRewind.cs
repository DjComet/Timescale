using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CombinedPositiveAndRewind : MonoBehaviour {

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
    public float previousTarget;
    public float timeScaleDifferential;

    
    public float recordInterval = 0.01f;
    private List<PointInTime> pointsInTime;
    public float elapsedTime;
    public bool isRewinding = false;
    public float recordTime = 5f;
    public bool hasAppliedStop = true;
    public float counter = 0;

    // Use this for initialization
    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
        scaledDeltaTime = gameObject.GetComponent<ScaledDeltaTime>();
        pointsInTime = new List<PointInTime>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        dt = scaledDeltaTime.scaledDT;
        ownTimescale = scaledDeltaTime.ownTimeScale;//newTimeScale


        timeScaleDifferential = Mathf.Abs(ownTimescale / previousTimeScale);



        velocityMagnitude = Vector3.Magnitude(rb.velocity);
        velocity = rb.velocity;
        angularMagnitude = Vector3.Magnitude(rb.angularVelocity);
        angVelocity = rb.angularVelocity;

        if (ownTimescale == 1)
        {
            rb.isKinematic = false;
            previousMass = rb.mass;
            previousVelocity = rb.velocity;
            previousAngVelocity = rb.angularVelocity;
            previousDrag = rb.drag;
            previousAngDrag = rb.angularDrag;

        }
        else if (ownTimescale == 0 && scaledDeltaTime.actualTarget == 0)
        {
            //Debug.Log("Co que cojones");
            rb.isKinematic = true;
        }
        else if (ownTimescale > Mathf.Epsilon && ownTimescale < 1)
        {
            rb.isKinematic = false;

            if (useDifferential)
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
                rb.angularVelocity = previousAngVelocity * ownTimescale;
                //rb.drag = previousDrag / ownTimescale;
                //rb.angularDrag = previousAngDrag / ownTimescale;

            }





        }
        else if (ownTimescale > 1)
        {
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



        previousTimeScale = ownTimescale;//this stores the previous ownTimeScale value for the next frame.
        previousTimeScale = Mathf.Clamp(previousTimeScale, Mathf.Epsilon, Mathf.Infinity);
    }
}
