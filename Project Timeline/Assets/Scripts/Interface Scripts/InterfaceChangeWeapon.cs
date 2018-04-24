using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceChangeWeapon : MonoBehaviour
{
    private Inputs inputs;
    private MainPlayerController playerController;
    public GameObject weaponPanel;
    private GameObject[] weaponPanelOptions = new GameObject [8];
    public Vector3 SelectorPos = new Vector3(0,0,0);
    public GameObject weaponSelector;
    public float counter;
    public float posSelector;
    public float weaponActive;
    private float t;

    // Use this for initialization
    void Start()
    {        
        inputs = GameObject.FindGameObjectWithTag("Player").GetComponent<Inputs>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<MainPlayerController>();
        weaponPanel = GameObject.Find("WeaponPanel");

        for (int i = 0; i < weaponPanel.transform.childCount; i++)
        {
            Debug.Log(i);
            GameObject child = weaponPanel.transform.GetChild(i).gameObject;
            if (child.name == "WeaponIcon01")
            {
                weaponPanelOptions[i] = child;
            }
            else if (child.name == "WeaponIcon02")
            {
                weaponPanelOptions[i] = child;
            }
            else if (child.name == "WeaponIcon03")
            {
                weaponPanelOptions[i] = child;
            }
            else if (child.name == "WeaponIcon04")
            {
                weaponPanelOptions[i] = child;
            }
            else if (child.name == "WeaponSelector")
            {
                weaponSelector = child;
            }

        }
        counter = 0;
        weaponActive = playerController.weaponSelector;
        SelectorPos = weaponSelector.transform.position;

    }

    // Update is called once per frame
    void Update()
    {       
        if(counter != 0)
        {
            weaponPanel.SetActive(true);
            counter--;
        }
        else
        {
            weaponPanel.SetActive(false);
        }

        checkInputs();
        checkSelectorPos();

        weaponSelector.transform.position = SelectorPos; 
        
    }

    void checkInputs()
    {
        
        if(inputs.mouseScroll != 0 || inputs.weap1 || inputs.weap2 || inputs.weap3 || inputs.weap4)
        {
            counter = 100.0f;
        }
    }

    void checkSelectorPos()
    {
        float weaponSelected = playerController.weaponSelector;

        if(weaponActive != weaponSelected)
        {
            changePosSelector();
        }
        else
        {
            t = 0;
            weaponActive = playerController.weaponSelector;
        }

    }

    void changePosSelector()
    {
        t += Time.deltaTime / 3.0f;
        float xPos = Mathf.Lerp(weaponSelector.transform.position.x, weaponPanelOptions[playerController.weaponSelector].transform.position.x, t);
        SelectorPos = new Vector3(xPos, SelectorPos.y, 0);
    }

}
