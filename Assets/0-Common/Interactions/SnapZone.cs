using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SnapZone : MonoBehaviour
{
    [SerializeField] private string m_acceptedSnapType;
    [SerializeField] private Transform[] m_snapSpots;
    
    public UnityEvent m_OnFirstItemSnapped;
    public UnityEvent m_OnItemSnapped;
    public UnityEvent m_OnItemUnsnapped;
    public UnityEvent m_OnLastItemUnsnapped;
    
    private Dictionary<Transform, bool> m_spotsAvailability;

    private void Awake()
    {
        m_spotsAvailability = new Dictionary<Transform, bool>();
        foreach (var spot in m_snapSpots)
        {
            m_spotsAvailability.Add(spot, false);
        }
    }

    public bool TrySnapItem(out Transform spotTransform)
    {
        // Check the first spot available
        foreach (var spot in m_spotsAvailability)
        {
            if (!spot.Value)
            {
                spotTransform = spot.Key;
                m_spotsAvailability[spot.Key] = true;
                m_OnItemSnapped?.Invoke();
                if (GetNumberOfItemsSnapped() == 1) m_OnFirstItemSnapped?.Invoke();
                return true;
            }
        }

        spotTransform = null;
        return false;
    }

    private int GetNumberOfItemsSnapped()
    {
        return m_spotsAvailability.Count(e => e.Value);
    }

    public void FreeSpot(Transform spotTransform)
    {
        m_spotsAvailability[spotTransform] = false;
        m_OnItemUnsnapped?.Invoke();
        if (GetNumberOfItemsSnapped() == 0) m_OnLastItemUnsnapped?.Invoke();
    }
}
