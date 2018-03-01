using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour {

    public float horizontal;
    public float vertical;
    public bool jump;
    public bool jumpBool;
    public bool leftClick;
    public bool rightClick;
    public bool actionRight;
    public bool actionLeft;
    public bool modifier;
    public bool activate;
    public bool submit;
    public bool cancel;
    public float mouseX;
    public float mouseY;
    public float mouseScroll;
    public float timeScaleAxis;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        horizontal = Input.GetAxisRaw("Horizontal1");
        vertical = Input.GetAxisRaw("Vertical1");
        jumpBool = Input.GetButtonDown("Jump");
        jump = Input.GetButton("Jump");

        leftClick = Input.GetButton("LeftClick");
        rightClick = Input.GetButton("RightClick");

        actionRight = Input.GetButtonDown("ActionRight");
        actionLeft = Input.GetButtonDown("ActionLeft");
        modifier = Input.GetButton("Modifier");

        activate = Input.GetButtonDown("Activate");
        submit = Input.GetButtonDown("Submit");
        cancel = Input.GetButtonDown("Cancel");

        mouseX = Input.GetAxisRaw("MouseX");
        mouseY = Input.GetAxisRaw("MouseY");
        mouseScroll = Input.GetAxisRaw("MouseScrollWheel");


        

    }
}
