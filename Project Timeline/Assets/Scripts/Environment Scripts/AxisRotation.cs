using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisRotation : MonoBehaviour {
    float dt;
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

        if (scaledDeltaTime != null)
            dt = scaledDeltaTime.scaledDT;
        else
            dt = Time.deltaTime;
        

        float rotX = (rotationX ? 1 : 0);
        float rotY = (rotationY ? 1 : 0);
        float rotZ = (rotationZ ? 1 : 0);





        if (sameRotationOnAllAxis)
        {
            transform.Rotate(rotX * Mathf.Rad2Deg * rotationForAllAxis * dt, rotY * Mathf.Rad2Deg * rotationForAllAxis * dt, rotZ * Mathf.Rad2Deg * rotationForAllAxis * dt);
        }
        else
        {
            transform.Rotate(rotX * Mathf.Rad2Deg * rotationSpeedX * dt, rotY * Mathf.Rad2Deg * rotationSpeedY * dt, rotZ * Mathf.Rad2Deg * rotationSpeedZ * dt);
        }
        


    }
}
