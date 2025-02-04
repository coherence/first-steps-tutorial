using UnityEngine;

namespace Coherence.FirstSteps
{
    /// <summary>
    /// Reacts to animation events invoked from within player's Animation Clips, to play particles.
    /// </summary>
    public class PlayerAnimationEvents : MonoBehaviour
    {
        public ParticleSystem runParticles;
        public ParticleSystem jumpParticles;
        public ParticleSystem landParticles;

        Transform _landParticlesTransform;

        private void Awake()
        {
            _landParticlesTransform = landParticles.transform;
        }
    
        public void PlayRunParticles() => runParticles.Play();
        public void StopRunParticles() => runParticles.Stop();
        public void PlayLandParticles()
        {
            // Calculate the landing particles position before playing them,
            // to account for the fact that remote players might still be in the air when this is invoked
            Ray ray = new(transform.position + Vector3.up * .5f, Vector3.down);
            Physics.Raycast(ray, out RaycastHit raycastHit, 1.3f);
            _landParticlesTransform.position = raycastHit.point + Vector3.up * .1f;
        
            landParticles.Play();
        }

        public void PlayJumpParticles()
        {
            jumpParticles.Play();
            StopRunParticles();
        }
    }
}
