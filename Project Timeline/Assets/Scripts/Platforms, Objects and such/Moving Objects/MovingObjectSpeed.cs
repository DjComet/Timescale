using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjectSpeed : MonoBehaviour {

    public float speedMagnitude;
    public Vector3 speedVector;

	// Use this for initialization
	void Start () {
        if (!gameObject.GetComponent<ApplyMovementToPlayer>())
        {
            gameObject.AddComponent<ApplyMovementToPlayer>();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
