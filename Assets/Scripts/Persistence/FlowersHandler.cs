using Coherence;
using Coherence.Toolkit;
using TMPro;
using UnityEngine;

public class FlowersHandler : MonoBehaviour
{
    public int maxFlowers = 30;
    public TextMeshProUGUI flowersCountText;

    private Counter _counter;
    private CoherenceSync _counterSync;
    private Flower[] _allFlowers;
    private CoherenceMonoBridge _monoBridge;

    private void Awake()
    {
        _monoBridge = FindObjectOfType<CoherenceMonoBridge>();
        _monoBridge.onLiveQuerySynced.AddListener(MonoBridgeOnOnLiveQuerySynced);
    }

    private void MonoBridgeOnOnLiveQuerySynced(CoherenceMonoBridge obj)
    {        
        _counter = FindObjectOfType<Counter>();
        
        _counterSync = _counter.GetComponent<CoherenceSync>();
        _counter.CounterChanged += OnCounterChanged;

        UpdateText();
    }

    private void OnDisable()
    {
        if (_counter != null) _counter.CounterChanged -= OnCounterChanged;
        _monoBridge.onLiveQuerySynced.RemoveListener(MonoBridgeOnOnLiveQuerySynced);
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
    /// When invoked by <see cref="CoherenceMonoBridge"/> on disconnect, it just clears the remaining GameObjects because the player is not online anymore.
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
