using UnityEngine;

namespace Coherence.Samples
{
    /// <summary>
    /// This script disables the GameObject on the Simulator, using the pre-compilation directive COHERENCE_SIMULATOR,
    /// added automatically by coherence when building a Simulator.
    /// </summary>
    public class DisableOnSimulator : MonoBehaviour
    {
#if COHERENCE_SIMULATOR
        private void Awake()
        {
            gameObject.SetActive(false);
        }
#endif
    }
}
