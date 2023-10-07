using UnityEngine;
using UnityEngine.Events;

public class ColliderUtility : MonoBehaviour
{
    [SerializeField] private LayerMask m_layer;
    [SerializeField] private UnityEvent m_onTriggerEntered;

    private void OnTriggerEnter(Collider other)
    {
        if ((m_layer.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            m_onTriggerEntered?.Invoke();
        }
    }
}