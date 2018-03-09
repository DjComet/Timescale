using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PointInTime
{

    public readonly Vector3 position;
    public readonly Quaternion rotation;
    public readonly Vector3 velocity;
    public readonly Vector3 angularVelocity;

    public PointInTime(Transform t, Vector3 v, Vector3 aV)
    {
        position = t.position;
        rotation = t.rotation;
        velocity = v;
        angularVelocity = aV;
    }
}

public class RewindScript : MonoBehaviour {

    private Rigidbody rb;
    private PointInTime[] pointsInTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Record()
    {

    }

    void Rewind()
    {
         
    }

    public void StartRewind()
    {

    }

    public void StopRewind()
    {

    }

    void ReapplyForces()
    {
        rb.position = pointsInTime[0].position;
        rb.rotation = pointsInTime[0].rotation;
        rb.velocity = pointsInTime[0].velocity;
        rb.angularVelocity = pointsInTime[0].angularVelocity;
    }
}
