using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCreator : MonoBehaviour {

    public GameObject PortalPref;

    private Transform camera;
    private GameObject portal;

    // Use this for initialization
    void Start () {
        camera = Camera.main.transform;
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray myRay;

        if (Input.GetMouseButton(0))
        {
            myRay = new Ray(camera.position, camera.forward);

            if (Physics.Raycast(myRay, out hit))
            {
                if (hit.collider.tag == "CanCreatePortal")
                {
                    if(!portal)
                        portal = GameObject.Instantiate(PortalPref, hit.transform.position, Quaternion.Euler(hit.normal));

                    else
                    {
                        portal.transform.position = hit.transform.position;
                        portal.transform.rotation = Quaternion.Euler(hit.normal);
                    }
                }
            }
        }
		
	}
}
