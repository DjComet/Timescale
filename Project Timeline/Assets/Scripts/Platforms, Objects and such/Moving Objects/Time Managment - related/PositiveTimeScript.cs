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
    float temporalTS;


    [SerializeField]
    private float _timeScale = 1;
    public float timeScale //This is where the magic happens
    {
        get { return _timeScale; }
        set
        {
            if (!first)
            {
                //rb.mass *= timeScale;
                rb.velocity /= timeScale;
                rb.angularVelocity /= timeScale;
                
                    //Debug.Log("In Timescale-Mass: " + rb.mass);
                //Debug.Log("In Timescale-Velocity: " + rb.velocity);
                //Debug.Log("In Timescale-AngVelocity: " + rb.angularVelocity);
            }
            first = false;

            _timeScale = Mathf.Abs(value);
          
            //rb.mass /= timeScale;
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
        

        /*
        Debug.Log("ownTimescale: " + objectTimeline.ownTimeScale);
        Debug.Log("Timescale: " + timeScale);
        Debug.Log("Velocity: " + rb.velocity);
        Debug.Log("AngVelocity: " + rb.angularVelocity);
        */

        //Different TimeScale values: Normal, Slow and Fast, Pause.

        if (objectTimeline.ownTimeScale == 1)
        {//             Normal
            rb.isKinematic = false;
            rb.useGravity = true;
            timeScale = 1;
        }
        else if (((objectTimeline.ownTimeScale > 0 && objectTimeline.ownTimeScale < 1) || objectTimeline.ownTimeScale > 1) && objectTimeline.previousTarget != -1 )
        {//                                       Slow                                                Fast         
            rb.useGravity = false;
            rb.isKinematic = false;

            if (objectTimeline.ownTimeScale > 0.08f)
            {
                                                        //This is to avoid crazy velocity spikes when the time differential is near-infinite (when going back from 0 to normal time, the divisor is smaller than the dividend, 
                timeScale = objectTimeline.ownTimeScale;//and being both smaller than 1, this causes extremely high numbers when very close to 0. Testing has concluded that the minimum value that timeScale should be is 0.08).
                temporalTS = timeScale;
            }
            else if (objectTimeline.actualTarget == -1)
            {
                timeScale = 1;//If we don't set timeScale back to one here, afterwards when ownTimeScale reaches 1, the last value of timeScale (something around 0.09 will multiply the current velocity
            }

            


            if (direction < 0)
            Debug.Log("Slow Accel going to pause/rewind");
            else if(direction > 0)  Debug.Log("Slow Accel coming from rewind");
        }
        else if (objectTimeline.ownTimeScale == 0)
        {//                   Pause
            //Debug.Log("Rigids are kinematic: We are in PAUSE");
            rb.isKinematic = true;
            applied = false;
        }

        


        //Store rigidbody's velocity, angVelocity and mass before making it kinematic on pause...
        if (direction < 0 && objectTimeline.actualTarget == 0 && objectTimeline.ownTimeScale > 0)
        {
            Debug.Log("Storing velocities and stuff");
            previousVelocity = rb.velocity;
            previousAngVelocity = rb.angularVelocity;
            previousMass = rb.mass;
        }
        else if(direction > 0 && objectTimeline.previousTarget == 0)
        {//... and reapply them ONCE after no longer being paused or rewinded.
            if(!applied)
            {
                Debug.Log("Applying speeds");
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
        //Debug.Log("Previous Target :" + objectTimeline.previousTarget);


        rb.velocity += Physics.gravity / rb.mass * dt;

    }
}

//Podría escribir una tesis de 100 páginas explicando únicamente cómo funciona este script y la cantidad de mierda que tuve que probar para llegar a hacerlo funcionar en todos los casos.