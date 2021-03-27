using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateobject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float horizontalSpeed = 2.0F;
    public float verticalSpeed = 2.0F;

     void Update()
     {

        if (Input.GetMouseButton(0))
        {
         float h = horizontalSpeed * Input.GetAxis("Mouse X") *-1;
         float v = verticalSpeed * Input.GetAxis("Mouse Y");
         transform.Rotate(v, h, 0);
        }

     }
 
 
}
