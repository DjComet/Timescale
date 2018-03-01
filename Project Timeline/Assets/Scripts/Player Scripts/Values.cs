using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Values : MonoBehaviour {

    public float maxSpeed = 13.0f;
    public float acceleration = 75.0f;
    public float runningMultiplier = 1.75f;
    public float maxLadderSpeed = 13.0f;
    public float ladderAcceleration = 80.0f;

    public float stickToGroundForce = 20.0f;
    public float maxAirSpeed = 5.0f;
    public float airAcceleration = 30.0f;

    public float maxJumpForce = 30.0f;
    public float jumpDeceleration = 50.0f;//this value doesn't matter, in fact, this should be in the Locomotion script, not here.
    public float maxJumpDeceleration = 100;
    public float minJumpDeceleration = 40;
    public float jumpDecelerationRate = 700;
    public float gravity = 40.0f;
    public float coyoteTime = 0.2f;      //Time elapsed after you are no longer on a structure, but can still jump.
    public float deltaGround = 2.2f;     //half of the capsule's height + a lil bit extra for good measure.
    public float maxFallSpeed = 80.0f;
    
    public bool grounded = false;
    public bool hitCeiling = false;
    

    public float deltaWallClimb = 1.6f; //MaxDistance from a wall at which you can engage in the "grabbing the slope" action.

    public float camHorizontalSpeed = 100.0f;
    public float camVerticalSpeed = 70.0f;


    private Vector3 front = new Vector3(0.0f, 0.0f, 0.5f);
    private Vector3 back = new Vector3(0.0f, 0.0f, -0.5f);
    private Vector3 left = new Vector3(-0.5f, 0.0f, 0.0f);
    private Vector3 right = new Vector3(0.5f, 0.0f, 0.0f);

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        grounded = (Physics.Raycast(transform.position, -transform.up, deltaGround)         ||
                    Physics.Raycast(transform.position + front, -transform.up, deltaGround) ||
                    Physics.Raycast(transform.position + back, -transform.up, deltaGround)  ||
                    Physics.Raycast(transform.position + left, -transform.up, deltaGround)  ||
                    Physics.Raycast(transform.position + right, -transform.up, deltaGround));


        hitCeiling = Physics.Raycast(transform.position, transform.up, deltaGround);
    }
}
