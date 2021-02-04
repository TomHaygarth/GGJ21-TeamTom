using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDigController : MonoBehaviour
{
    private DigZone m_activeDigZone = null;

    [SerializeField]
    private GameObject m_digVisual = null;

    [SerializeField]
    private TimerController m_digTimer = null;

    [SerializeField]
    private PlayerMovementController m_movementController = null;

    [SerializeField]
    private IPlayerInputController m_input = null;

    private bool m_isDigging = false;

    public bool IsDigging { get { return m_isDigging; } }

    private void Start()
    {
        m_digTimer.OnTimerFinished += CompleteDig;
        m_input = GetComponent<IPlayerInputController>();
    }

    public DigZone ActiveDigZone { get { return m_activeDigZone; } }

    public void SetDigZone(DigZone zone)
    {
        m_activeDigZone = zone;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isDigging == true)
        {
            // player has released the dig key so stop the dig early
            if (m_input.DigReleased == true)
            {
                EndDig();
            }

            return;
        }

        if (m_input.DigPressed == true)
        {
            StartDig();
        }
    }

    private void StartDig()
    {
        m_movementController.PausePlayerpMovement();
        m_digVisual.SetActive(true);
        m_digTimer.StartTimer();
        m_isDigging = true;

    }
    private void EndDig()
    {
        m_movementController.ResumePlayerpMovement();
        m_digTimer.Pause();
        m_digVisual.SetActive(false);
        m_isDigging = false;
    }

    private void CompleteDig()
    {
        EndDig();

        // Check for artifact
        if (m_activeDigZone != null && m_activeDigZone.HasArtifact == true)
        {
            // award score
            ArtifactItemData artifact = m_activeDigZone.TakeArtifact();
            GameController.Instance().CollectedArtifact(artifact);
        }
    }
}
