using Coherence.Connection;
using Coherence.Toolkit;
using TMPro;
using UnityEngine;

namespace Coherence.FirstSteps
{
    /// <summary>
    /// A manager object that players have to go through to add or remove flowers.
    /// This class has nothing synced over the network, but it relies on the <see cref="Counter"/>
    /// to have a synced picture of how many flowers are there right now.
    /// </summary>
    public class FlowersHandler : MonoBehaviour
    {
        public int maxFlowers = 30;
        public TextMeshProUGUI flowersCountText;

        private Counter _counter;
        private CoherenceSync _counterSync;
        private Flower[] _allFlowers;
        private CoherenceBridge _bridge;

        private void Awake()
        {
            _bridge = FindObjectOfType<CoherenceBridge>();
            _counter = FindObjectOfType<Counter>();
            _counterSync = _counter.GetComponent<CoherenceSync>();
        }

        private void OnEnable()
        {
            _counter.CounterChanged += OnCounterChanged;
            _bridge.onLiveQuerySynced.AddListener(OnLiveQuerySynced);
            _bridge.onDisconnected.AddListener(OnDisconnect);
        }

        private void OnDisconnect(CoherenceBridge bridge, ConnectionCloseReason reason) => RemoveFlowerGameObjects();

        private void OnLiveQuerySynced(CoherenceBridge obj) => UpdateText();

        private void OnDisable()
        {
            _counter.CounterChanged -= OnCounterChanged;
            _bridge.onLiveQuerySynced.RemoveListener(OnLiveQuerySynced);
            _bridge.onDisconnected.RemoveListener(OnDisconnect);
        }

        private void OnCounterChanged(int newValue)
        {
            _counter.count = newValue;
            UpdateText();
        }

        /// <summary>
        /// Invoked by the <see cref="Plant"/> script when creating a new flower GameObject.
        /// </summary>
        public void AddFlower()
        {
            if (_counter.count >= maxFlowers)
            {
                RemoveOldestFlower();
            }
            else
            {
                if (_counterSync.HasStateAuthority)
                {
                    _counter.AddOne();
                }
                else
                    _counterSync.SendCommand<Counter>(nameof(Counter.AddOne), MessageTarget.AuthorityOnly);
            }
        }

        private void RemoveOldestFlower()
        {
            _allFlowers = FindObjectsOfType<Flower>();
            for (int i = _allFlowers.Length - 1; i >= 0; i--)
            {
                Flower flower = _allFlowers[i];
                if (flower != null)
                {
                    RemoveFlower(flower);
                    break;
                }
            }
        }

        private void UpdateText()
        {
            flowersCountText.text = _counter.count.ToString();
        }

        public void ClearFlowersFromUI()
        {
            RemoveFlowerGameObjects();
            if (_counterSync.HasStateAuthority)
            {
                _counter.ResetToZero();
            }
            else
                _counterSync.SendCommand<Counter>(nameof(Counter.ResetToZero), MessageTarget.AuthorityOnly);
        }

        /// <summary>
        /// When pressing the Clear button in the UI, it removes entities from the server.
        /// When invoked by <see cref="CoherenceBridge"/> on disconnect,
        /// it just clears the remaining GameObjects from the scene because the player is not online anymore.
        /// </summary>
        public void RemoveFlowerGameObjects()
        {
            _allFlowers = FindObjectsOfType<Flower>();
            for (int i = 0; i < _allFlowers.Length; i++)
            {
                RemoveFlower(_allFlowers[i]);
            }
        }

        private void RemoveFlower(Flower f)
        {
            CoherenceSync sync = f.GetComponent<CoherenceSync>();
            if (sync.HasStateAuthority)
            {
                Destroy(f.gameObject);
            }
            else
                f.DestroyRemote();
        }
    }
}
