using System.Collections.Generic;
using Coherence.Entities;
using Coherence.Toolkit;
using UnityEngine;
using UnityEngine.UI;

namespace Coherence.Samples
{
    /// <summary>
    /// A simple UI to request authority over existing player network entities.
    /// This script checks for each available entity in the scene, and instantiates and destroys buttons accordingly.
    /// </summary>
    public class AuthorityRequestUi : MonoBehaviour
    {
        [SerializeField] private GameObject authTransferUiTemplate;
        [SerializeField] private GameObject noEntitiesMessage;
      
        private CoherenceBridge coherenceBridge;
        private Dictionary<Entity, GameObject> allAuthTransferUIs = new();

        private void Awake()
        {
            CoherenceBridgeStore.TryGetBridge(gameObject.scene, out coherenceBridge);
        }

        private void Update()
        {
            // Cycle over all network entities known to the CoherenceBridge
            foreach (KeyValuePair<Entity, NetworkEntityState> entity in coherenceBridge.EntitiesManager)
            {
                CoherenceSync sync = entity.Value.Sync as CoherenceSync;

                // Filtering out non-player entities
                if (sync == null || sync.GetComponent<SimpleMovement>() == null) continue;

                bool hasAuthority = entity.Value.HasStateAuthority;

                bool transferUIExists = allAuthTransferUIs.TryGetValue(entity.Key, out GameObject authTransferUi);
                
                // Create or destroy an authority transfer UI, based on the entity's authority state
                switch (hasAuthority)
                {
                    case true when transferUIExists:
                        {
                            Destroy(authTransferUi);
                            allAuthTransferUIs.Remove(entity.Key);
                            break;
                        }
                    case false when !transferUIExists:
                        {
                            var instance = Instantiate(authTransferUiTemplate, transform);
                            instance.SetActive(true);
                            instance.GetComponentInChildren<Text>().text = sync.name;
                            instance.GetComponentInChildren<Button>().onClick.AddListener(() => OnRequestAuthority(sync));
                            allAuthTransferUIs.Add(entity.Key, instance);
                            break;
                        }
                }
            }
            
            noEntitiesMessage.SetActive(allAuthTransferUIs.Count == 0);
        }

        private void OnRequestAuthority(CoherenceSync sync)
        {
            sync.RequestAuthority(AuthorityType.Full);
            
            // Authority transfer is set to "Steal" on our player Prefabs,
            // so this request should automatically succeed
        }
    }
}

