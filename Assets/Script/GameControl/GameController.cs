using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private int m_maxArtifacts = 8;

    [SerializeField]
    private Rect m_spawnZone;

    [SerializeField]
    private ArtifactController m_artifactController = null;

    private List<ArtifactItemData> m_spawnedArtifacts = new List<ArtifactItemData>();

    private void Awake()
    {
        // Force a max target of 60fps
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnInitialArtifacts();
    }

    private void SpawnInitialArtifacts()
    {
        for (int i = 0; i < m_maxArtifacts; ++i)
        {
            ArtifactItemData artifact
                = m_artifactController.CreateNewArtifact(m_spawnZone);

            m_spawnedArtifacts.Add(artifact);
        }
    }
}
