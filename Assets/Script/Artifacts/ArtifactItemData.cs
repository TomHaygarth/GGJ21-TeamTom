using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactItemData : MonoBehaviour
{
    [SerializeField]
    private ArtifactItemType m_ItemType;
    [SerializeField]
    private int m_CorrectItemScore = 100;
    [SerializeField]
    private int m_IncorrectItemScore = -100;

    [SerializeField]
    private ArtifactDetectedVisualController m_detectedVisualController = null;
    public ArtifactDetectedVisualController DetectedVisualController { get { return m_detectedVisualController; } }
}
