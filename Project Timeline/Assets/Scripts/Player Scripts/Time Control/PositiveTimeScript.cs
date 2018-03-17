using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositiveTimeScript : MonoBehaviour {

    bool first = true;
    bool applied = true;
    private Rigidbody rb;
    private ObjectTimeLine objectTimeline;
    public float direction;

    private Vector3 previousVelocity;
    private Vector3 previousAngVelocity;
    private float previousMass;


    [SerializeField]
    private float _timeScale = 1;
    public float timeScale //This is where the magic happens
    {
        get { return _timeScale; }
        set
        {
            if (!first)
            {
                rb.mass *= timeScale;
                rb.velocity /= timeScale;
                rb.angularVelocity /= timeScale;
                /*Debug.Log(scaledDeltaTime.ownTimeScale);
                Debug.Log(rb.mass);
                Debug.Log(rb.velocity);
                Debug.Log(rb.angularVelocity);*/
            }
            first = false;

            _timeScale = Mathf.Abs(value);
          
            rb.mass /= timeScale;
            rb.velocity *= timeScale;
            rb.angularVelocity *= timeScale;
            
        }
    }

    void Awake()
    {
        
        rb = gameObject.GetComponent<Rigidbody>();
        objectTimeline = gameObject.GetComponent<ObjectTimeLine>();
        timeScale = _timeScale;
    }


    void Update()
    {
        direction = Mathf.Sign(objectTimeline.actualTarget - objectTimeline.previousTarget);

        //Different TimeScale values: Normal, Slow and Fast, Pause.

        if (objectTimeline.ownTimeScale == 1)
        {//             Normal
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        else if ((objectTimeline.ownTimeScale > 0 && objectTimeline.ownTimeScale < 1) || objectTimeline.ownTimeScale > 1)
        {//                                       Slow                                                Fast         
            rb.useGravity = false;
            rb.isKinematic = false;

            if (objectTimeline.ownTimeScale > 0.08)      //This is to avoid crazy velocity spikes when the time differential is near-infinite (when going back from 0 to normal time, the divisor is smaller than the dividend, 
                timeScale = objectTimeline.ownTimeScale; //and being both smaller than 1, this causes extremely high numbers when very close to 0. 
                                                          //Testing has concluded that the minimum value that timeScale should be is 0.08).

        }
        else if (objectTimeline.ownTimeScale == 0)
        {//                   Pause
            rb.isKinematic = true;
            applied = false;
        }

        //Store rigidbody's velocity, angVelocity and mass before making it kinematic...
        if(direction < 0 && objectTimeline.actualTarget == 0 && objectTimeline.ownTimeScale > 0)
        {
            previousVelocity = rb.velocity;
            previousAngVelocity = rb.angularVelocity;
            previousMass = rb.mass;
        }
        else if(direction > 0 && objectTimeline.previousTarget == 0)
        {//... and reapply them ONCE after no longer being paused.
            if(!applied)
            {
                rb.velocity = previousVelocity;
                rb.angularVelocity = previousAngVelocity;
                rb.mass = previousMass;
                applied = true;
            }
            
        }
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime * timeScale;
       
        rb.velocity += Physics.gravity/ rb.mass * dt;
    }
}
