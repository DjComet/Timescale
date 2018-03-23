using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CanvasUpdater : MonoBehaviour {
    private Inputs inputs;
    private GameObject playerCanvas;
    private Text timeMultIndicator;
    private Text mouseClickHint;
    private Slider energySlider;
    private GameObject player;
    private TimeScaleControl timeScaleControl;

    private float energyAmount;

    // Use this for initialization
    void Start () {
        playerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas");
        player = GameObject.FindGameObjectWithTag("Player");
        inputs = player.GetComponent<Inputs>();
        timeScaleControl = player.GetComponent<TimeScaleControl>();

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
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(timeScaleControl != null && timeScaleControl.energy != null)
        changeUI();
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
}
