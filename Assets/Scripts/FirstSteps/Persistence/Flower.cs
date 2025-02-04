using System;
using Coherence.Toolkit;
using UnityEngine;
using UnityEngine.UI;

namespace Coherence.FirstSteps
{
    /// <summary>
    /// Controls the lifetime and visual states of a flower. When planted, the time gets recorded in <see cref="timePlanted"/>,
    /// so that any player that finds the flower as a networked entity later on
    /// is able to calculate the stage <see cref="FlowerState"/> that the flower should be in.
    /// </summary>
    public class Flower : MonoBehaviour
    {
        public FlowerState state = FlowerState.Uninitialised;
        [OnValueSynced(nameof(SetInitialState))] public uint timePlanted;

        [Header("References")]
        public GameObject[] parts;
        public ParticleSystem sparks, pollen;
        public Image pieChart;
        public GameObject canvas;

        [Header("Phase durations")]
        public uint timespanToBloom = 5;
        public uint timespanToWilt = 5;

        private uint _timeToBloom;
        private uint _timeToWilt;
        private Animator _animator;
        private CoherenceSync _sync;

        public enum FlowerState
        {
            Uninitialised,
            Bud,
            Flower,
            Wilted,
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Invoked by coherence when the value of <see cref="timePlanted"/> has been synchronised by the Replication server.
        /// </summary>
        public void SetInitialState(uint oldValue, uint newValue)
        {
            timePlanted = newValue;
            CalculateTimings();

            if (HasToWilt()) ChangeState(FlowerState.Wilted, true);
            else if (HasToBloom()) ChangeState(FlowerState.Flower, true);
            else ChangeState(FlowerState.Bud, true);
        }

        private void Update()
        {
            switch (state)
            {
                case FlowerState.Bud:
                    pieChart.fillAmount = ProgressToBloom();
                    if (HasToBloom()) ChangeState(FlowerState.Flower);
                    break;
            
                case FlowerState.Flower:
                    pieChart.fillAmount = ProgressToWilt();
                    if (HasToWilt()) ChangeState(FlowerState.Wilted);
                    break;
            }
        }

        private void ChangeState(FlowerState newState, bool instantly = false)
        {
            switch (newState)
            {
                case FlowerState.Flower:
                    if (!instantly)
                    {
                        pollen.Play();
                        sparks.Play();
                    }
                    break;
            
                case FlowerState.Wilted:
                    canvas.SetActive(false);
                    if(!instantly)
                        pollen.Play();
                    break;
            }

            state = newState;
        
            pieChart.fillAmount = 0f;
            if(!instantly) _animator.SetInteger("State", (int)state);
            for (int i = 0; i < parts.Length; i++)
            {
                bool isActive = (int)state-1 == i;
                parts[i].SetActive(isActive);
            }
        }

        private uint Now() => (uint)(DateTime.UtcNow - DateTime.UnixEpoch).TotalSeconds;
    
        private bool HasToBloom() => Now() >= _timeToBloom;
        private bool HasToWilt() => Now() >= _timeToWilt;
    
        private float ProgressToBloom() => Mathf.InverseLerp(0, timespanToBloom, Now() - timePlanted);
        private float ProgressToWilt() => Mathf.InverseLerp(timespanToBloom, timespanToBloom + timespanToWilt, Now() - timePlanted);

        /// <summary>
        /// Only called when the prefab is being planted by the player, locally.
        /// </summary>
        public void Plant()
        {
            timePlanted = Now();
            CalculateTimings();
            ChangeState(FlowerState.Bud);
        }

        private void CalculateTimings()
        {
            _timeToBloom = timePlanted + timespanToBloom;
            _timeToWilt = timePlanted + timespanToBloom + timespanToWilt;
        }

        /// <summary>
        /// To be used on remote <see cref="Flower"/> objects. It first requests Authority and, once obtained, destroys the object.
        /// </summary>
        public void DestroyRemote()
        {
            _sync = GetComponent<CoherenceSync>();
            _sync.OnStateAuthority.AddListener(DestroyThis);
            _sync.RequestAuthority(AuthorityType.Full);
        }

        private void DestroyThis()
        {
            _sync.OnStateAuthority.RemoveListener(DestroyThis);
            Destroy(gameObject, .1f);
        }
    }
}
