using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactController : MonoBehaviour
{
    [SerializeField]
    private List<ArtifactItemData> m_Artifacts;

    // serailise these list so we can debug/inspect them in the editor
    [SerializeField]
    private List<DigZone> m_inactiveDigZones = new List<DigZone>();
    [SerializeField]
    private List<DigZone> m_activeDigZones = new List<DigZone>();

    public ArtifactItemData CreateNewArtifact()
    {
        if (m_Artifacts.Count == 0 && m_inactiveDigZones.Count > 0)
        {
            return null;
        }

        int rand_artifact_idx = Random.Range(0, m_Artifacts.Count);
        int rand_zone_idx = Random.Range(0, m_inactiveDigZones.Count);

        DigZone zone = m_inactiveDigZones[rand_zone_idx];

        ArtifactItemData new_artifact = Instantiate<ArtifactItemData>(m_Artifacts[rand_artifact_idx],
                                                                      zone.ArtifactSpawnPoint,
                                                                      false);
        new_artifact.transform.localPosition = Vector3.zero;

        zone.GiveArtifact(new_artifact);

        // move the zone from inactive to active
        m_activeDigZones.Add(zone);
        m_inactiveDigZones.Remove(zone);

        return new_artifact;
    }

    public void RegisterDigZone(DigZone zone)
    {
        m_inactiveDigZones.Add(zone);
    }

    public void RefreshActiveDigZones()
    {
        // We're going to move newly inactive Dig zones from the active list to
        // the inactive one. So to avoid index becomeing invalid we'll go from
        // back to front
        for(int i = m_activeDigZones.Count; i > 0; --i)
        {
            DigZone zone = m_activeDigZones[i - 1];

            if (zone.HasArtifact == false)
            {
                m_inactiveDigZones.Add(zone);
                m_activeDigZones.RemoveAt(i - 1);
            }
        }
    }
}
