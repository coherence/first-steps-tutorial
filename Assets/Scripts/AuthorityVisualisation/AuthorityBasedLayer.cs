using Coherence.Toolkit;
using UnityEngine;

/// <summary>
/// Monitors the authority state of a Coherence Sync. When authority changes,
/// it moves the <see cref="targetRenderers"/> to specific layers, so that when
/// the URP Renderer changes (see <see cref="AuthorityViewer"/>),
/// they are rendered with a different colour.
/// </summary>
public class AuthorityBasedLayer : MonoBehaviour
{
    public CoherenceSync referenceSync;
    public GameObject[] targetRenderers;

    public string stateAuthorityLayer = "StateAuthority";
    public string stateRemoteLayer = "StateRemote";
    public string orphanedLayerName = "Orphaned";

    private void Awake()
    {
        CheckAuthority();
        referenceSync.CoherenceBridge.onLiveQuerySynced.AddListener(OnLiveQuerySynced);
        referenceSync.OnStateAuthority.AddListener(OnStateAuthority);
        referenceSync.OnStateRemote.AddListener(OnStateRemote);
    }

    private void OnLiveQuerySynced(CoherenceBridge bridge) => CheckAuthority();

    private void CheckAuthority()
    {
        if(referenceSync.HasStateAuthority) OnStateAuthority();
        else OnStateRemote();
    }

    private void OnStateAuthority()
    {
        SetObjectsToLayer(LayerMask.NameToLayer(stateAuthorityLayer));
    }

    private void OnStateRemote()
    {
        if(referenceSync.EntityState.IsOrphaned)
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
        if(referenceSync.CoherenceBridge != null) referenceSync.CoherenceBridge.onLiveQuerySynced.RemoveListener(OnLiveQuerySynced);
        referenceSync.OnStateAuthority.RemoveListener(OnStateAuthority);
        referenceSync.OnStateRemote.RemoveListener(OnStateRemote);
    }
}
