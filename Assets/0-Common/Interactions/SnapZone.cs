using System.Collections.Generic;
using UnityEngine;

public class SnapZone : MonoBehaviour
{
    [SerializeField] private string m_acceptedSnapType;
    [SerializeField] private Transform[] m_snapSpots;

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
                return true;
            }
        }

        spotTransform = null;
        return false;
    }

    public void FreeSpot(Transform spotTransform)
    {
        m_spotsAvailability[spotTransform] = false;
    }
}
