using System;
using Coherence.Toolkit;
using UnityEngine;

[RequireComponent(typeof(CoherenceSync))]
public class Counter : MonoBehaviour
{
    [OnValueSynced(nameof(OnValueSyncedRemotely))] public int count;
    
    public event Action<int> CounterChanged;

    public void OnValueSyncedRemotely(int oldValue, int newValue)
    {
        CounterChanged?.Invoke(newValue);
    }

    public void ResetToZero() => SetTo(0);
    public void AddOne() => SetTo(count + 1);

    private void SetTo(int newValue)
    {
        count = newValue;
        CounterChanged?.Invoke(newValue);
    }

    private void OnDestroy()
    {
        CounterChanged = null;
    }
}
