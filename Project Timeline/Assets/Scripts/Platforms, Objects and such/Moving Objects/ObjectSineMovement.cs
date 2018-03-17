﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSineMovement : MonoBehaviour {
    float dt;

    //Linear movement, sine movement, axis of movement
    private TimeScaleControl timeScale;
    private ObjectTimeLine scaledDeltaTime;

    public bool worldSpace;

    public float sineFrequency = 1f;
    public float sineMagnitude = 10f;
    public bool inverted = false;
    public bool isRight = true;
    public bool isUp = false;
    public bool isForward = false;
    float realtime;
    public float delay = 0f;
    Vector3 axis;
    Vector3 position;


    // Use this for initialization
    void Start () {
        scaledDeltaTime = gameObject.GetComponent<ObjectTimeLine>();
        position = transform.position;
        
        realtime = sineMagnitude / 2;
        realtime += delay;
        if (inverted)
            sineFrequency *= -1;
    }
	
	// Update is called once per frame
	void Update () {
        int right, up, forward; 
        right = (isRight ? 1 : 0);
        up = (isUp ? 1 : 0);
        forward = (isForward ? 1 : 0);

        if (scaledDeltaTime != null)
            dt = scaledDeltaTime.scaledDT;
        else
            dt = Time.deltaTime;

        if(worldSpace)
        {
            axis = Vector3.right * right + Vector3.up * up + Vector3.forward * forward;
        }
        else
        {
            axis = transform.right * right + transform.up * up + transform.forward * forward;
        }
        
        sine();
	}
    void sine()
    {
        realtime += dt;

        transform.position = position + axis * Mathf.Sin(realtime* 2 * Mathf.PI * sineFrequency) * sineMagnitude;
    }
}
