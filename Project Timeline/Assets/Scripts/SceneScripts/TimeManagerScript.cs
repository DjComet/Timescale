using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagerScript : MonoBehaviour {

    protected Inputs inputs;
    protected GameObject player;
    protected TimeScaleControl timeScaleControl;

    public float elapsedTime;
    public float ownTimeScale;


    // Use this for initialization
    void Start () {
        
        player = GameObject.FindGameObjectWithTag("Player");
        inputs = player.GetComponent<Inputs>();
        timeScaleControl = player.GetComponent<TimeScaleControl>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
