using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleThroughObjects : MonoBehaviour {

    public int timeLayer = 9;
    public List<GameObject> objectsInTimeLayer;
    private TimeScaleControl timeScaleControl;
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
            go.GetComponent<ScaledDeltaTime>().scaledDT = Time.deltaTime * timeScaleControl.ownTimeScale;
        }

	}
}
