using UnityEngine;
using System.Collections;

public class PlayerInputController : MonoBehaviour, IPlayerInputController, IArtifactCollector
{
    [SerializeField]
    private InputMapping m_inputMap = null;

    private Vector2 m_inputMovementAxis = Vector2.zero;

    public Vector2 MovementAxis { get { return m_inputMovementAxis; } }

    public bool DigPressed { get; private set; }
    public bool DigReleased { get; private set; }

    public float DirectionDeadzone { get { return m_inputMap.DirectionDeadzone; } }

    // Use this for initialization
    void Start()
    {
        ResetInputs();
    }

    private void ResetInputs()
    {
        m_inputMovementAxis = Vector2.zero;
        DigPressed = false;
        DigReleased = false;
    }

    // Update is called once per frame
    void Update()
    {
        ResetInputs();

        if (Input.GetKey(m_inputMap.UpKey) == true)
        {
            m_inputMovementAxis.y = 1.0f;
        }
        else if (Input.GetKey(m_inputMap.DownKey) == true)
        {
            m_inputMovementAxis.y = -1.0f;
        }
        else
        {
            // if no key is pressed check if they're using a gamepad instead
            m_inputMovementAxis.y = Input.GetAxis(m_inputMap.GamepadLeftstickVertical);
        }

        if (Input.GetKey(m_inputMap.LeftKey) == true)
        {
            m_inputMovementAxis.x = -1.0f;
        }
        else if (Input.GetKey(m_inputMap.RightKey) == true)
        {
            m_inputMovementAxis.x = 1.0f;
        }
        else
        {
            // if no key is pressed check if they're using a gamepad instead
            m_inputMovementAxis.x = Input.GetAxis(m_inputMap.GamepadLeftstickHorizontal);
        }

        if (Input.GetKeyUp(m_inputMap.DigKey) == true)
        {
            DigReleased = true;
        }
        if (Input.GetKeyDown(m_inputMap.DigKey) == true)
        {
            DigPressed = true;
        }
    }

    public void OnArtifactCollected(ArtifactItemData artifact)
    {
        // score artifact
        GameController.Instance().CollectedArtifact(artifact, true);
    }
}
