using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnim : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    PlayerMovementController m_movementController = null;

    [SerializeField]
    Animator m_animator = null;

    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        //m_animator.SetFloat("speed", m_movementController.CurrentSpeed);
        m_animator.SetFloat("speed", m_movementController.CurrentSpeed);


        //LinkedList this to is digging
        if (Input.GetKeyDown("space"))
        {
            m_animator.SetTrigger("dig");
        }//else is not rigging
        if (Input.GetKeyUp("space"))
        {
            m_animator.SetTrigger("stopdig");
        }
        //if dig == win
        //m_animator.SetTrigger("windig");
    }
}
