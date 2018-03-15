using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisRotation : MonoBehaviour {

    private ScaledDeltaTime scaledDeltaTime;

    public bool sameRotationOnAllAxis = false;
    public float rotationForAllAxis = 5.0f;

    public float rotationSpeedX = 0.0f;
    public float rotationSpeedY = 0.0f;
    public float rotationSpeedZ = 0.0f;

    public bool rotationX = false;
    public bool rotationY = true;
    public bool rotationZ = false;

	// Use this for initialization
	void Start () {
        scaledDeltaTime = gameObject.GetComponent<ScaledDeltaTime>();
	}
	
	// Update is called once per frame
	void Update () {
        float dt = Time.deltaTime * scaledDeltaTime.ownTimeScale;
        if(sameRotationOnAllAxis)
        {
            transform.Rotate((rotationX ? rotationForAllAxis * dt : 0f), (rotationY ? rotationForAllAxis * dt : 0f), (rotationZ ? rotationForAllAxis * dt : 0f));
        }
        else
        {
            transform.Rotate((rotationX ? rotationSpeedX * dt : 0f), (rotationY ? rotationSpeedY * dt : 0f), (rotationZ ? rotationSpeedZ * dt : 0f));
        }
        


    }
}
