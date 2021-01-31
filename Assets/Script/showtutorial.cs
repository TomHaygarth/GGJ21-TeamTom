using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showtutorial : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject menuobject;
    public GameObject tutobject;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        { 
           print("info");
           menuobject.active = false;
           tutobject.active = true;

        }else if (Input.GetKeyUp("space"))
        {
            menuobject.active = true;
            tutobject.active = false;
        }

    }
}
