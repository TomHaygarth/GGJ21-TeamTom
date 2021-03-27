using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changebg : MonoBehaviour
{
    // Start is called before the first frame update
   public void changeBG()
    {
        Camera.main.backgroundColor = Random.ColorHSV();
    }
}
