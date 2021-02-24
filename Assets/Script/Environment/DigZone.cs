using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigZone : MonoBehaviour
{
    public delegate void ArtifactTakenEvent(DigZone zone);
    public event ArtifactTakenEvent OnArtifactTaken = delegate { };

    [SerializeField]
    private Transform m_artifactSpawnPoint;

    public Transform ArtifactSpawnPoint { get { return m_artifactSpawnPoint; } }

    private ArtifactItemData m_artifact = null;

    public ArtifactItemData CurrentArtifact { get { return m_artifact; } }

    public bool HasArtifact { get { return m_artifact != null; } }

    public void GiveArtifact(ArtifactItemData artifact)
    {
        m_artifact = artifact;
    }

    public ArtifactItemData TakeArtifact()
    {
        ArtifactItemData artifact = m_artifact;
        m_artifact = null;

        OnArtifactTaken(this);

        return artifact;
    }

    private void Start()
    {
        Renderer mesh_renderer = GetComponent<Renderer>();
        mesh_renderer.enabled = false;

        GameController.Instance().RegisterDigZone(this);
    }

    // Start is called before the first frame update
    void OnTriggerEnter(Collider col)
    {
        PlayerDigController player = col.GetComponent<PlayerDigController>();
        if (player != null)
        {
            player.SetDigZone(this);
        }
        else if (HasArtifact == true)
        {
            ArtifactDetector detector = col.GetComponent<ArtifactDetector>();
            if(detector != null)
            {
                Debug.Log("Enabling Detected visual");
                m_artifact.DetectedVisualController.enabled = true;
                GameController.Instance().ArtifactDetected(this);
            }
        }
    }

    // Update is called once per frame
    void OnTriggerExit(Collider col)
    {
        // if the player has just left our digzone and didn't enter another one in the meantime
        PlayerDigController player = col.GetComponent<PlayerDigController>();
        if (player != null && player.ActiveDigZone == this)
        {
            player.SetDigZone(null);
        }
    }
}
