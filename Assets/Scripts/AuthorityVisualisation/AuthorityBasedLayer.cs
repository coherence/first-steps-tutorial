using Coherence.Toolkit;
using UnityEngine;

public class AuthorityBasedLayer : MonoBehaviour
{
    public CoherenceSync referenceSync;
    public GameObject[] targetRenderers;

    public string stateAuthorityLayer = "StateAuthority";
    public string stateRemoteLayer = "StateRemote";
    public string orphanedLayerName = "Orphaned";

    private void Awake()
    {
        referenceSync.OnStateAuthority.AddListener(OnStateAuthority);
        referenceSync.OnStateRemote.AddListener(OnStateRemote);
    }

    private void OnStateAuthority()
    {
        SetObjectsToLayer(LayerMask.NameToLayer(stateAuthorityLayer));
    }

    private void OnStateRemote()
    {
        if(referenceSync.isOrphaned)
            SetObjectsToLayer(LayerMask.NameToLayer(orphanedLayerName));
        else
            SetObjectsToLayer(LayerMask.NameToLayer(stateRemoteLayer));
    }
    
    private void SetObjectsToLayer(int newLayer)
    {
        foreach (GameObject go in targetRenderers)
            go.layer = newLayer;
    }

    private void OnDestroy()
    {
        referenceSync.OnStateAuthority.RemoveListener(OnStateAuthority);
        referenceSync.OnStateRemote.RemoveListener(OnStateRemote);
    }
}
