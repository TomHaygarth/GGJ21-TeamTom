using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    [SerializeField]
    PlayerMovementController m_movementController = null;

    [SerializeField]
    Animator m_animator = null;

    //m_animator = GetComponent<Animator>();



    // Update is called once per frame
    void Update()
    {
        m_animator.SetFloat("CurrentSpeed", m_movementController.CurrentSpeed);

        // m_animator.SetBool("DiggingRightNow", m_diddgingController.Digging);
       
        // m_animator.SetTrigger("Dig", m_diggingController.Dig);

        // if (test dig = true) { testdig = false} m_animator.SetTrigget.Dig;


        /*
        if (Input.GetKey("w"))
        {
            m_animator.SetBool;
        }

        if (Input.GetKey("space"))
        {
            m_animator.SetBool "DigggingRightNow" = false;
        }
        */
    }

    
}
