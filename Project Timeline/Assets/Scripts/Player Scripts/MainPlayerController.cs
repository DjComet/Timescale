using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerController : MonoBehaviour {

    
    private Values values;
    private Locomotion locomotion; 
    //private Air air;
    private WallClimb wallClimb;
    private LadderClimb ladderClimb;
    public float oldAcceleration;
    public float oldMaxSpeed;
    float timer = 0.0f;


    public bool isGrounded = true;
    public bool isAirborne = false;
    public bool isOnWallEdge = false;
    public bool isOnLadder = false;


	// Use this for initialization
	void Start () {
        values      = gameObject.GetComponent<Values>();
        locomotion  = gameObject.GetComponent<Locomotion>(); 
        wallClimb   = gameObject.GetComponent<WallClimb>();
        ladderClimb = gameObject.GetComponent<LadderClimb>();
        oldAcceleration = values.acceleration;
        oldMaxSpeed = values.maxSpeed;

    }
	
	// Update is called once per frame
	void Update () {
		
        if(isGrounded)
        {
            values.acceleration = oldAcceleration;
            values.maxSpeed = oldMaxSpeed;

            wallClimb.enabled = false;
            timer = 0.0f;
            
        }

        if (isAirborne)
        {
            timer += Time.deltaTime;
            values.acceleration = values.airAcceleration;
            if (timer >= 0.5)
            {
                values.maxSpeed = values.maxAirSpeed;
            }

        }

        if (isAirborne && isOnWallEdge)
        {

            wallClimb.enabled = true;
        }
        else if(!isOnWallEdge)
        {
            wallClimb.enabled = false;
        }
        

        if(isOnLadder)
        {
            locomotion.enabled = false;
            locomotion.speed = Vector3.zero;//The direction and speed values must be zeroed on exit because if not,
            locomotion.axisDirection = Vector3.zero;//they will be reapplied as soon as player exits the ladder.
            ladderClimb.enabled = true;
        }
        else
        {
            locomotion.enabled = true;
            ladderClimb.enabled = false;
            ladderClimb.speed = Vector3.zero;//The direction and speed values must be zeroed on exit because if not,
            ladderClimb.axisDirection = Vector3.zero;//they will be reapplied as soon as player reenters the ladder.
        }
	}


}
