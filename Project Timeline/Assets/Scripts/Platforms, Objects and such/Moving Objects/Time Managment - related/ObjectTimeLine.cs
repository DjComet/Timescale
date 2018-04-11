using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTimeLine : MonoBehaviour {

    public TimeManagerScript timeManagerScript;

    public float actualTarget;
    public float previousTarget;
    public float ownTimeScale;
    public float scaledDT;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        actualTarget = timeManagerScript.timeScaleControl.targetValue;
        previousTarget = timeManagerScript.timeScaleControl.previousTargetValue;
        ownTimeScale = timeManagerScript.ownTimeScale;
        scaledDT = timeManagerScript.ownTimeScale * Time.deltaTime;

	}
}
