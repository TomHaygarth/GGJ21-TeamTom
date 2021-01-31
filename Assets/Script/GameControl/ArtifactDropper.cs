using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactDropper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance().OnLevelEnd += OnLevelEnd;
        gameObject.SetActive(false);
    }

    private void OnLevelEnd()
    {
        gameObject.SetActive(true);

        var bodies = transform.GetComponentsInChildren<Rigidbody>(true);
        foreach (var body in bodies)
        {
            body.isKinematic = false;
        }
    }
}
