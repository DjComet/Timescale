using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class CameraEffectsController : MonoBehaviour {
    public float tValue;
    private PostProcessingProfile ppp;   
    private Color[] colorValues = new Color[4];//0(azul-pausa),1(rojo-rebobinar),2(amarillo-slow),3(naranja-acelerar)
    public Color color;
    private string lastColor;
    private TimeScaleControl ptScrip;

	// Use this for initialization
	void Start () {
        //Colors
        colorValues[0] = new Color(0.0f,0.0f,1.0f,0.7f);
        colorValues[1] = new Color(1.0f,0.0f,0.0f,0.7f);
        colorValues[2] = new Color(1.0f, 1.0f, 0.0f,0.7f);
        colorValues[3] = new Color(1.0f, 0.4f, 0.0f,0.7f);
        ppp = GetComponent<PostProcessingBehaviour>().profile;
        ptScrip = GetComponentInParent<TimeScaleControl>();

        ppp.vignette.enabled = true;
        ppp.colorGrading.enabled = true;

        resetProfile();

    }
	
	// Update is called once per frame
	void Update () {
        float dt = Time.deltaTime;
        tValue = ptScrip.targetValue;            

        if(tValue!= 1)
        {
            calculateColor();
            var vigSettings = ppp.vignette.settings;
            vigSettings.intensity = Mathf.Lerp(ppp.vignette.settings.intensity,0.35f, 0.9f * dt);
            ppp.vignette.settings = vigSettings;
        }
        else
        {
            resetColor();
            var vigSettings = ppp.vignette.settings;
            vigSettings.intensity = Mathf.MoveTowards(ppp.vignette.settings.intensity,0.0f, 0.05f * dt);
            ppp.vignette.settings = vigSettings;
        }
    }

    //Set color with the player variable tValue
    void calculateColor()
    {        
        if (tValue == -2)
        {
            color = colorValues[1];
            var gradSettings = ppp.colorGrading.settings;
            gradSettings.channelMixer.red.x = Mathf.Lerp(ppp.colorGrading.settings.channelMixer.red.x, 2, 0.9f * Time.deltaTime);
            ppp.colorGrading.settings = gradSettings;
            lastColor = "red";


        }
        else if (tValue == 0)
        {
            color = colorValues[0];            
            var gradSettings = ppp.colorGrading.settings;
            gradSettings.channelMixer.blue.z = Mathf.Lerp(ppp.colorGrading.settings.channelMixer.blue.z, 2, 0.9f * Time.deltaTime);
            ppp.colorGrading.settings = gradSettings;
            lastColor = "blue"; 
        }
        else if (tValue == 0.2f)
        {
            color = colorValues[2];
            var gradSettings = ppp.colorGrading.settings;
            gradSettings.channelMixer.red.x = Mathf.Lerp(ppp.colorGrading.settings.channelMixer.red.x, 2.0f, 0.9f * Time.deltaTime);
            gradSettings.channelMixer.green.y = Mathf.Lerp(ppp.colorGrading.settings.channelMixer.green.y, 2.0f, 0.9f * Time.deltaTime);
            ppp.colorGrading.settings = gradSettings;

            lastColor = "yellow";
        }
        else if (tValue == 2)
        {
            color = colorValues[3];
            var gradSettings = ppp.colorGrading.settings;
            gradSettings.channelMixer.red.x = Mathf.Lerp(ppp.colorGrading.settings.channelMixer.red.x, 2, 0.95f * Time.deltaTime);
            gradSettings.channelMixer.green.y = Mathf.Lerp(ppp.colorGrading.settings.channelMixer.green.y, 1.4f, 0.9f * Time.deltaTime);
            ppp.colorGrading.settings = gradSettings;
            lastColor = "orange";
        }
        var vignetteColor = ppp.vignette.settings;
        vignetteColor.color = color;
        ppp.vignette.settings = vignetteColor;
        
    }


    //Set normal color when the effects end
    void resetColor()
    {
        if(lastColor == "red")
        {
            var gradSettings = ppp.colorGrading.settings;            
            gradSettings.channelMixer.red.x = Mathf.MoveTowards(ppp.colorGrading.settings.channelMixer.red.x, 1, 0.2f * Time.deltaTime);
            ppp.colorGrading.settings = gradSettings;
        }
        else if (lastColor == "blue")
        {
            var gradSettings = ppp.colorGrading.settings;            
            gradSettings.channelMixer.blue.z = Mathf.MoveTowards(ppp.colorGrading.settings.channelMixer.blue.z, 1, 0.2f * Time.deltaTime);
            ppp.colorGrading.settings = gradSettings;           
        }
        else if (lastColor == "yellow")
        {
            var gradSettings = ppp.colorGrading.settings;            
            gradSettings.channelMixer.red.x = Mathf.MoveTowards(ppp.colorGrading.settings.channelMixer.red.x, 1, 0.2f * Time.deltaTime);
            gradSettings.channelMixer.green.y = Mathf.MoveTowards(ppp.colorGrading.settings.channelMixer.green.y, 1, 0.2f * Time.deltaTime);
            ppp.colorGrading.settings = gradSettings;
        }
        else if (lastColor == "orange")
        {
            var gradSettings = ppp.colorGrading.settings;            
            gradSettings.channelMixer.red.x = Mathf.MoveTowards(ppp.colorGrading.settings.channelMixer.red.x, 1, 0.5f * Time.deltaTime);
            gradSettings.channelMixer.green.y = Mathf.MoveTowards(ppp.colorGrading.settings.channelMixer.green.y, 1, 0.2f * Time.deltaTime);
            ppp.colorGrading.settings = gradSettings;
        }
    }


    //Reset to normal values the profile
    void resetProfile()
    {
        var vigSettings = ppp.vignette.settings;
        vigSettings.intensity = 0;
        ppp.vignette.settings = vigSettings;

        var gradSettings = ppp.colorGrading.settings;
        gradSettings.channelMixer.red = new Vector3(1.0f, 0.0f, 0.0f);
        gradSettings.channelMixer.green = new Vector3(0.0f, 1.0f, 0.0f);
        gradSettings.channelMixer.blue = new Vector3(0.0f, 0.0f, 1.0f);
        ppp.colorGrading.settings = gradSettings;
    }

}
