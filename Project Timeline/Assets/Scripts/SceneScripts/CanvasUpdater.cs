﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CanvasUpdater : MonoBehaviour {
    private Inputs inputs;
    private GameObject playerCanvas;
    private Text timeMultIndicator;
    private Text mouseClickHint;
    private Slider energySlider;
    private Text currentTime;
    private Image crosshair;

    private GameObject player;
    private TimeScaleControl timeScaleControl;
    private TimeManagerScript timeManagerScript;
    private LookAndInteract lookAndInteract;

    private float energyAmount;

    // Use this for initialization
    void Start () {
        playerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas");
        player = GameObject.FindGameObjectWithTag("Player");
        inputs = player.GetComponent<Inputs>();
        timeScaleControl = player.GetComponent<TimeScaleControl>();
        lookAndInteract = player.GetComponent<LookAndInteract>();
        timeManagerScript = gameObject.GetComponent<TimeManagerScript>();

        for (int i = 0; i < playerCanvas.transform.childCount; i++)
        {
            GameObject child = playerCanvas.transform.GetChild(i).gameObject;
            if (child.name == "TimeMultIndicator")
            {
                timeMultIndicator = child.GetComponent<Text>();
            }
            else if (child.name == "EnergySlider")
            {
                energySlider = child.GetComponent<Slider>();
                if (timeScaleControl != null && timeScaleControl.energy != null)
                {
                    energySlider.maxValue = timeScaleControl.energy.maxEnergyAmt;

                    energyAmount = timeScaleControl.energy.maxEnergyAmt;
                }
            }
            else if (child.name == "MouseClickHint")
            {
                mouseClickHint = child.GetComponent<Text>();
            }
            else if (child.name == "CurrentTime")
            {
                currentTime = child.GetComponent<Text>();
            }
            else if(child.name == "Crosshair")
            {
                crosshair = child.GetComponent<Image>();
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        float acceleratedTimeValue = timeManagerScript.timeScaleControl.timeValues[4];
        float normalTimeValue = timeManagerScript.timeScaleControl.timeValues[3];
        float slowedTimeValue = timeManagerScript.timeScaleControl.timeValues[2];
        float pausedTimeValue = timeManagerScript.timeScaleControl.timeValues[1];
        float rewindTimeValue = timeManagerScript.timeScaleControl.timeValues[0];


        if (timeScaleControl != null && timeScaleControl.energy != null)
        {
            changeUI();
            currentTime.text = timeManagerScript.currentTime.ToString("#.00");
        }

        changeCrosshairColor();
	}

    void changeUI()
    {
        switch (timeScaleControl.i)
        {
            case 0:
                timeMultIndicator.text = ("<< REWIND");
                break;
            case 1:
                timeMultIndicator.text = ("|| PAUSE");
                break;
            case 2:
                timeMultIndicator.text = ("|> SLOW");
                break;
            case 3:
                timeMultIndicator.text = (" > NORMAL");
                break;
            case 4:
                timeMultIndicator.text = (">> FAST FORWARD");
                break;
        }
        energyAmount = timeScaleControl.energy.energyAmount;
        energySlider.value = energyAmount;

        if (inputs.leftClick)
        {
            mouseClickHint.text = ("Q: ||         E: |>\nPause      Slow  ");
        }
        else if (inputs.rightClick)
        {
            mouseClickHint.text = ("Q: <<         E: >>\n    Rewind    Accelerate  ");
        }
        else mouseClickHint.text = ("");

    }

    void changeCrosshairColor()
    {
        if(lookAndInteract.rayHit)
        {
            crosshair.color = Color.green;
            
        }
        else
        {
            crosshair.color = Color.black;
        }
    }
}
