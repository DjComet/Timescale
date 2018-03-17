using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleThroughObjects : TimeManagerScript {

    public int timeLayer = 9;
    public List<GameObject> objectsInTimeLayer;
    
    public int iterated = 0;
	// Use this for initialization
	void Awake () {

        timeScaleControl = gameObject.GetComponent<TimeScaleControl>();
        GameObject[] goArray = FindObjectsOfType<GameObject>();
        for(int i = 0; i < goArray.Length; i++)
        {
            if(goArray[i].layer == timeLayer)
            {
                objectsInTimeLayer.Add(goArray[i]);
                iterated++;
            }
        }
        
}
	
	// Update is called once per frame
	void Update () {
		
        /*for(int i = 0; i< objectsInTimeLayer.Count; i++)
        {
            
            objectsInTimeLayer[i].GetComponent<ScaledDeltaTime>().scaledDT = Time.deltaTime * timeScaleControl.ownTimeScale; //Otra forma de recorrer la lista (Igual de ineficiente¿? No se...)
            
        }*/

        foreach(GameObject go in objectsInTimeLayer)
        {
            if (go.GetComponent<ObjectTimeLine>() != null)
            {
                go.GetComponent<ObjectTimeLine>().scaledDT = Time.deltaTime * timeScaleControl.ownTimeScale;
                go.GetComponent<ObjectTimeLine>().ownTimeScale = timeScaleControl.ownTimeScale;
                go.GetComponent<ObjectTimeLine>().actualTarget = timeScaleControl.targetValue;
                go.GetComponent<ObjectTimeLine>().previousTarget = timeScaleControl.previousTargetValue;
            }


            if (go.GetComponent<RewindScript>() != null)
            {
                if (timeScaleControl.rewindHasBeenPressed)
                {
                    go.GetComponent<RewindScript>().isRewinding = true;
                }
                else 
                {
                    go.GetComponent<RewindScript>().isRewinding = false;
                }
            }


            
        }

	}
}
