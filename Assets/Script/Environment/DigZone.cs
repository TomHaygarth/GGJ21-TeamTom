using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigZone : MonoBehaviour
{
    [SerializeField]
    private Transform m_artifactSpawnPoint;

    public Transform ArtifactSpawnPoint { get { return m_artifactSpawnPoint; } }
    private void Start()
    {
        Renderer mesh_renderer = GetComponent<Renderer>();
        mesh_renderer.enabled = false;
    }

    // Start is called before the first frame update
    void OnTriggerEnter(Collider col)
    {
        PlayerMovementController player = col.GetComponent<PlayerMovementController>();
        if (player != null)
        {
            // 
        }
    }

    // Update is called once per frame
    void OnTriggerExit(Collider col)
    {
        PlayerMovementController player = col.GetComponent<PlayerMovementController>();
        if (player != null)
        {
        }
    }
}
