using Coherence.Toolkit;
using UnityEngine;

namespace Coherence.Samples
{
    public class ActivateLegend : MonoBehaviour
    {
        private void Awake()
        {
            CoherenceBridgeStore.TryGetBridge(gameObject.scene, out var coherenceBridge);
            coherenceBridge.onConnected.AddListener(_ =>
            {
                gameObject.SetActive(true);
            });
            coherenceBridge.onDisconnected.AddListener((_,__) =>
            {
                gameObject.SetActive(false);
            });
            
            gameObject.SetActive(false);
        }
    }
}
