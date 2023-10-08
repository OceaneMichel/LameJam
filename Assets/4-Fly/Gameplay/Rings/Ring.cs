using TMPro;
using UnityEngine;

public class Ring : MonoBehaviour
{
    [SerializeField] private Collider m_ringCollider;
    [SerializeField] private Animation m_ringAnim;
    [SerializeField] private TextMeshPro m_text;
    [SerializeField] private PlaySound m_playSound;
    
    public void OnRingEntered()
    {
        var ringManager = RingManager.GetInstance();
        ringManager.CollectRing();
        
        // Show FX
        m_ringCollider.enabled = false;

        m_text.text = $"{ringManager.GetCollectedRings()} / {ringManager.GetTotalRings()}";
        
        // Activate effect
        m_ringAnim.Play();
        m_playSound.Play();
    }
}
