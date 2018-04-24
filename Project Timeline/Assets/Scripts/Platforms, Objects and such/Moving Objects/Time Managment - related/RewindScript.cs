using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PointInTime
{

    public readonly Vector3 position;
    public readonly Quaternion rotation;
    public readonly Vector3 velocity;
    public readonly Vector3 angularVelocity;
    public readonly float timeWhenRecorded;
    public readonly int number;
    

    public PointInTime(Transform t, Vector3 v, Vector3 angV, float cT, int n)
    {
        position = t.position;
        rotation = t.rotation;
        velocity = v;
        angularVelocity = angV;
        timeWhenRecorded = cT;
        number = n;
    }
}

public class RewindScript : MonoBehaviour {

#region Variables Declaration
    private Rigidbody rb;
    private ObjectTimeLine objectTimeline;
    private PositiveTimeScript positiveTimeScript;
    public bool enableDebug = false;

    //General Variables----------------------------------------------------------------------------------------
    private float recordInterval = 0.1f;
    public List<PointInTime> pointsInTime;
    private float currentTime;
    public bool isRewinding = false;
    public float recordTime = 5f;
    public bool hasAppliedStop = true;
    public float counter = 0;
    private float t = 0;
    private bool canInitializeLerp = true;

    int number = 0;//Debugging variable

    //Lerping stuff--------------------------------------------------------------------------------------------
    Vector3 initialPos;
    Quaternion initialRot;
    float initialTime;
    float targetTime;
    float lerper;//redundant variable, can be replaced by currentTime.
#endregion

    // Use this for initialization
    void Awake () {
        pointsInTime = new List<PointInTime>();
        rb = gameObject.GetComponent<Rigidbody>();
        objectTimeline = gameObject.GetComponent<ObjectTimeLine>();
        positiveTimeScript = gameObject.GetComponent<PositiveTimeScript>();
        counter = 0;
        pointsInTime.Insert(0, new PointInTime(transform, rb.velocity, rb.angularVelocity, currentTime, number));
    }

    // Update is called once per frame
    void Update()
    {
        float pausedTimeValue = objectTimeline.timeManagerScript.timeScaleControl.timeValues[1];
        float rewindTimeValue = objectTimeline.timeManagerScript.timeScaleControl.timeValues[0];

        currentTime = objectTimeline.timeManagerScript.currentTime;

        if (objectTimeline.ownTimeScale < pausedTimeValue)
            isRewinding = true;
        else if(objectTimeline.actualTarget != rewindTimeValue)
        {
            isRewinding = false;
        }



        if (isRewinding)
        {
            rb.isKinematic = true;
            Rewind();
            hasAppliedStop = false;
            
        }
        else if(!isRewinding)
        {
            if (!hasAppliedStop)
            {
                StopRewind();
                hasAppliedStop = true;
            }
            Record();
            
        }

    }

#region Functions    

    void Record()
    {
        
        
        if (counter >= recordInterval)
        { 
            pointsInTime.Insert(0, new PointInTime(transform, rb.velocity, rb.angularVelocity, currentTime, number));
            if (enableDebug) Debug.Log("PointInTimeInserted: pos " + pointsInTime[0].position + " vel " + pointsInTime[0].velocity + " ang vel " + pointsInTime[0].angularVelocity + " timeWhenRecorded " + pointsInTime[0].timeWhenRecorded + " number " + pointsInTime[0].number);

            if (currentTime - pointsInTime[pointsInTime.Count - 1].timeWhenRecorded > recordTime)//If the time elapsed between NOW and the last element on the list is greater than 5 seconds, delete it.
            {
                if (enableDebug) Debug.Log("PointRemoved with time: " + pointsInTime[pointsInTime.Count - 1].timeWhenRecorded);
                pointsInTime.RemoveAt(pointsInTime.Count - 1);
            }

            number += 1;
            counter = 0;

        }
        counter += Mathf.Abs(objectTimeline.scaledDT);//Abs is a safeguard against negative scaledDeltaTimes when coming back from rewind to normal time, which shouldn't happen but hey.
    }
    
    void Rewind()
    {
        if (pointsInTime.Count > 1)
        {
            
            PointInTime pointInTime = pointsInTime[0];

            //First take the initial position and time and the target position and time
            if (currentTime != pointInTime.timeWhenRecorded && canInitializeLerp)
            {
                initialPos = transform.position;
                initialRot = transform.rotation;
                initialTime = currentTime;
                canInitializeLerp = false;
                t = 0;//t must be zero every time we begin a new lerp section
                lerper = initialTime;
            }

            //Calculate T as a fraction of the current Time - initialTime divided by the time between the points A & B of the lerp. the variable Lerper can be replaced by currentTime, and the result is the same, 
            //although I think this way I have more control over what goes into the division. Maybe I'm wrong.

            t = (Mathf.Abs(currentTime - initialTime)) / Mathf.Abs(pointInTime.timeWhenRecorded - initialTime);//Calculation for t

            //lerper -= objectTimeline.scaledDT * -1;//the scaled delta time here is negative, so to remove it from t we must invert it.
            //lerper = Mathf.Clamp(lerper, pointInTime.timeWhenRecorded, initialTime);//after (timeWhenRecorded-initialTime) seconds, lerper == timeWhenRecorded, making t = 1.

            t = Mathf.Clamp01(t);//just in case
            if (enableDebug) Debug.Log("t:" + t);

            transform.position = Vector3.Lerp(initialPos, pointInTime.position, t);
            transform.rotation = Quaternion.Slerp(initialRot, pointInTime.rotation, t);

            if (pointsInTime != null && currentTime <= pointInTime.timeWhenRecorded)
            {
                if (enableDebug) Debug.Log("PointInTimeRemoved: pos " + pointsInTime[0].position + " vel " + pointsInTime[0].velocity + " ang vel " + pointsInTime[0].angularVelocity + " timeWhenRecorded " + pointsInTime[0].timeWhenRecorded + " number " + pointsInTime[0].number);
                pointsInTime.RemoveAt(0);
                t = 0;
                //lerper = initialTime;
                number -= 1;
                canInitializeLerp = true;
            }
           
        }
        else
        {
            
            PointInTime pointInTime = pointsInTime[0];
            //transform.position = pointInTime.position;
            //transform.rotation = pointInTime.rotation;
            
        }
        

    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        
        rb.isKinematic = false;
        ReapplyForces();
        canInitializeLerp = true;//Super important
    }

    void ReapplyForces()
    {
        if (enableDebug) Debug.Log("Applying Velocity:" + pointsInTime[0].velocity + "YOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
        if (enableDebug) Debug.Log("Applying AngularVelocity:" + pointsInTime[0].angularVelocity);
        
        rb.velocity = pointsInTime[0].velocity;
        rb.angularVelocity = pointsInTime[0].angularVelocity;
    }

    #endregion
}
