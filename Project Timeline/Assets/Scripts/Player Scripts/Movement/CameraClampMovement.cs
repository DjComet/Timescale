﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClampMovement : MonoBehaviour
{
    public float sensitivityX = 15.0f;
    public float sensitivityY = 15.0f;

    public float minimumX = -360.0f;
    public float maximumX = 360.0f;

    public float minimumY = -60.0f;
    public float maximumY = 60.0f;

    private float xAngle = 0.0f;
    private float yAngle = 0.0f;

    Quaternion originalRotation;
    
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.freezeRotation = true;
        }
        originalRotation = transform.localRotation;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //Gets rotational input from the mouse
        xAngle += Input.GetAxis("MouseX") * sensitivityX;
        yAngle += Input.GetAxis("MouseY") * sensitivityY;

        //Clamp rotation angles
        xAngle = ClampAngle(xAngle, minimumX, maximumX);
        yAngle = ClampAngle(yAngle, minimumY, maximumY);

        //Create rotations around axis
        Quaternion yQuaternion = Quaternion.AngleAxis(yAngle, Vector3.left);
        Quaternion xQuaternion = Quaternion.AngleAxis(xAngle, Vector3.up);

        //Rotate
        transform.localRotation = originalRotation * xQuaternion * yQuaternion;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360.0f) && (angle <= 360.0f))
        {
            if (angle < -360.0f)
            {
                angle += 360.0f;
            }
            if (angle > 360.0f)
            {
                angle -= 360.0f;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }
}