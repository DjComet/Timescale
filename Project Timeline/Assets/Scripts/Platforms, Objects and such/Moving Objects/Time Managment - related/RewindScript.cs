using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PointInTime
{

    public readonly Vector3 position;
    public readonly Quaternion rotation;
    public readonly Vector3 velocity;
    public readonly Vector3 angularVelocity;
    public readonly float recordingTime;
    
    

    public PointInTime(Transform t, Vector3 v, Vector3 angV, float cT)
    {
        position = t.position;
        rotation = t.rotation;
        velocity = v;
        angularVelocity = angV;
        recordingTime = cT;
    }
}

public class RewindScript : MonoBehaviour {

    private Rigidbody rb;
    private ObjectTimeLine objectTimeline;
    private PositiveTimeScript positiveTimeScript;

    private float recordInterval = 0.1f;
    private List<PointInTime> pointsInTime;
    public float currentTime;
    public bool isRewinding = false;
    public float recordTime = 5f;
    public bool hasAppliedStop = true;
    public float counter = 0;
    public float t = 0;
    public bool canInitializeLerp = true;

    Vector3 initialPos;
    Quaternion initialRot;
    float initialTime;
    float targetTime;

    // Use this for initialization
    void Start () {
        pointsInTime = new List<PointInTime>();
        rb = gameObject.GetComponent<Rigidbody>();
        objectTimeline = gameObject.GetComponent<ObjectTimeLine>();
        positiveTimeScript = gameObject.GetComponent<PositiveTimeScript>();
        
	}

    // Update is called once per frame
    void Update()
    {
        currentTime += objectTimeline.scaledDT;

        if (objectTimeline.ownTimeScale < 0)
            isRewinding = true;
        else if(objectTimeline.actualTarget != -1)
        {
            isRewinding = false;
        }


        if (isRewinding)
        {
            rb.isKinematic = true;
            Rewind();
            hasAppliedStop = false;
            //positiveTimeScript.enabled = false;
        }
        else if(!isRewinding)
        {
            if (!hasAppliedStop)
            {
                StopRewind();
                hasAppliedStop = true;
            }
            Record();
            //positiveTimeScript.enabled = true;
        }

    }

    

    void Record()
    {
        
        counter += Mathf.Abs(objectTimeline.scaledDT);//Abs is a safeguard against negative scaledDeltaTimes when coming back from rewind to normal time
        if (counter >= recordInterval)
        {
            if (pointsInTime.Count > Mathf.Round(recordTime / Time.deltaTime))
            {
                pointsInTime.RemoveAt(pointsInTime.Count - 1);
            }

            pointsInTime.Insert(0, new PointInTime(transform, rb.velocity, rb.angularVelocity, currentTime));
            counter = 0;
        }
        
    }
    
    

    void Rewind()
    {
        if (pointsInTime.Count > 1)
        {
            //first take the initial position and time and the target position and time
            

            PointInTime pointInTime = pointsInTime[0];
            //transform.position = pointInTime.position;
            //transform.rotation = pointInTime.rotation;
            if (currentTime != pointInTime.recordingTime && canInitializeLerp)
            {
                initialPos = transform.position;
                initialRot = transform.rotation;
                initialTime = currentTime;
                canInitializeLerp = false;
            }
            
            //Extract the value from 0 to 1 in between the time at which the lerp commenced and the time at which it will end
            t += (1 / Mathf.Abs(pointInTime.recordingTime - initialTime)) * (objectTimeline.scaledDT * -1);
            t = Mathf.Clamp01(t);

            transform.position = Vector3.Lerp(initialPos, pointInTime.position, t);
            transform.rotation = Quaternion.Slerp(initialRot, pointInTime.rotation, t);

            if (pointsInTime != null && currentTime <= pointInTime.recordingTime)
            {
                t = 0;
                pointsInTime.RemoveAt(0);
                canInitializeLerp = true;
            }
           
        }
        else
        {
            
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            
        }
        

    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        t = 0;
        rb.isKinematic = false;
        ReapplyForces();
    }

    void ReapplyForces()
    {
        rb.position = pointsInTime[0].position;
        rb.rotation = pointsInTime[0].rotation;
        rb.velocity = pointsInTime[0].velocity;
        rb.angularVelocity = pointsInTime[0].angularVelocity;
    }
}
