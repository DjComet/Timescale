using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PointInTime
{

    public readonly Vector3 position;
    public readonly Quaternion rotation;
    public readonly Vector3 velocity;
    public readonly Vector3 angularVelocity;
    public readonly float currentTime;
    

    public PointInTime(Transform t, Vector3 v, Vector3 angV, float cT)
    {
        position = t.position;
        rotation = t.rotation;
        velocity = v;
        angularVelocity = angV;
        currentTime = cT;
    }
}

public class RewindScript : MonoBehaviour {

    private Rigidbody rb;
    private ObjectTimeLine scaledDeltaTime;
    public float recordInterval = 0.01f;
    private List<PointInTime> pointsInTime;
    public float elapsedTime;
    public bool isRewinding = false;
    public float recordTime = 5f;
    public bool hasAppliedStop = true;
    public float counter = 0;

    // Use this for initialization
    void Start () {
        pointsInTime = new List<PointInTime>();
        rb = gameObject.GetComponent<Rigidbody>();
        scaledDeltaTime = gameObject.GetComponent<ObjectTimeLine>();
        
	}

    // Update is called once per frame
    void Update()
    {
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

    void Record()
    {
        
        counter += Mathf.Abs(scaledDeltaTime.scaledDT);//Abs is a safeguard against negative scaledDeltaTimes when coming back from rewind to normal time
        if (counter >= recordInterval)
        {
            if (pointsInTime.Count > Mathf.Round(recordTime / Time.deltaTime))
            {
                pointsInTime.RemoveAt(pointsInTime.Count - 1);
            }

            pointsInTime.Insert(0, new PointInTime(transform, rb.velocity, rb.angularVelocity, elapsedTime));
            counter = 0;
        }
        
    }
    
    

    void Rewind()
    {
        if (pointsInTime.Count > 1)
        {
            
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            if(pointsInTime != null)
            pointsInTime.RemoveAt(0);
            
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
