using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showtutorial : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject menuobject;
    public GameObject tutobject;

    public bool menuActive = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        { 
           print("info");
           //menuobject.active = false;
           //tutobject.active = true;
            menuActive = !menuActive;


        }else if (Input.GetKeyUp("space"))
        {
           // menuobject.active = true;
           // tutobject.active = false;
        }

        if (menuActive == true)
        { 
           //print("info");
           menuobject.active = false;
           tutobject.active = true;

        }else if (menuActive == false)
        {
            menuobject.active = true;
            tutobject.active = false;
        }

        //menuActive

    }
}
