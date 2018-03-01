using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    float dt;
        
    public GameObject player;
    private Inputs inputs;
    private Values values;
       

    public bool invertYAxis = false;
    public bool invertXAxis = false;
    private int invertY = 1;
    private int invertX = 1;

    public float sensitivity = 1.0f;
    public float horizontalSpeed = 0.0f;
    public float verticalSpeed = 0.0f;

    // Use this for initialization
    void Start () {

        values = player.GetComponent<Values>();
        inputs = player.GetComponent<Inputs>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        dt = Time.deltaTime;

        if (invertXAxis) { invertX = -1; }
        else invertX = 1;

        if (invertYAxis) { invertY = -1; }
        else invertY = 1;

         


        horizontalSpeed = values.camHorizontalSpeed * sensitivity * inputs.mouseX * invertX;
        verticalSpeed = values.camVerticalSpeed * sensitivity * inputs.mouseY * invertY;


        player.transform.Rotate(horizontalSpeed * Vector3.up * dt);//Horizontally, we rotate the player, not the camera.
        transform.Rotate(verticalSpeed * Vector3.left * dt); 
    }
}
