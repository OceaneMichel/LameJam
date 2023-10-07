using UnityEngine;

public class RingManager : Singleton<RingManager>
{
    [SerializeField] private Ring[] m_rings;

    private int collectedRings;
    
    public void CollectRing()
    {
        collectedRings++;
    }

    public int GetCollectedRings()
    {
        return collectedRings;
    }
    
    public int GetTotalRings()
    {
        return m_rings.Length;
    }
}
