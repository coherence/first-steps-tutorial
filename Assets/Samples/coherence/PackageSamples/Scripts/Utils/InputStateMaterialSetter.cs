using Coherence.Toolkit;
using UnityEngine;
using UnityEngine.Serialization;

namespace Coherence.Samples
{
    /// <summary>
    /// Sets different Materials on a MeshRenderer in response to a change in authority, specifically Input Authority.
    /// </summary>
    public class InputStateMaterialSetter : MonoBehaviour
    {
        [FormerlySerializedAs("_coherenceSync")] public CoherenceSync coherenceSync;
        public Material localInputMaterial;
        public Material remoteInputMaterial;
        
        private MeshRenderer _meshRenderer;
        private bool _isSetup;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            if(coherenceSync.CoherenceBridge.IsConnected) OnAuthorityChanged(); // Force invoke, for remote entities
        }

        private void OnEnable()
        {
            coherenceSync.OnInputAuthority.AddListener(OnAuthorityChanged);
            coherenceSync.OnInputRemote.AddListener(OnAuthorityChanged);
            coherenceSync.OnStateRemote.AddListener(OnAuthorityChanged);
        }

        private void OnDisable()
        {
            coherenceSync.OnInputAuthority.RemoveListener(OnAuthorityChanged);
            coherenceSync.OnInputRemote.RemoveListener(OnAuthorityChanged);
            coherenceSync.OnStateRemote.RemoveListener(OnAuthorityChanged);
        }

        private void OnAuthorityChanged()
        {
            _meshRenderer.enabled = true;

            if (coherenceSync.HasInputAuthority)
            {
                if(!coherenceSync.HasStateAuthority)
                    _meshRenderer.material = localInputMaterial;
            }
            
            else
                _meshRenderer.material = remoteInputMaterial;
        }
    }
}
