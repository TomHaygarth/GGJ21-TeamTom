using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnim : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    PlayerMovementController m_movementController = null;

    [SerializeField]
    PlayerDigController m_digController = null;

    [SerializeField]
    Animator m_animator = null;

    bool m_wasDigging = false;

    // Update is called once per frame
    void Update()
    {
        // if there's no dig controller we might be in the main menu
        // it's so fun to make the player dig in the main menu :P
        if (m_digController == null)
        {
            if (Input.GetKeyDown("space"))
            {
                m_animator.SetTrigger("dig");
            }//else is not rigging
            if (Input.GetKeyUp("space"))
            {
                m_animator.SetTrigger("stopdig");
            }
            return;
        }

        //m_animator.SetFloat("speed", m_movementController.CurrentSpeed);
        m_animator.SetFloat("speed", m_movementController.CurrentSpeed);

        if (m_wasDigging == false && m_digController.IsDigging == true)
        {
            m_animator.SetTrigger("dig");
        }
        else if (m_wasDigging == true && m_digController.IsDigging == false)
        {
            m_animator.SetTrigger("stopdig");
        }

        m_wasDigging = m_digController.IsDigging;
        ////LinkedList this to is digging
        //if (Input.GetKeyDown("space"))
        //{
        //    m_animator.SetTrigger("dig");
        //}//else is not rigging
        //if (Input.GetKeyUp("space"))
        //{
        //    m_animator.SetTrigger("stopdig");
        //}
        //if dig == win
        //m_animator.SetTrigger("windig");
    }
}
