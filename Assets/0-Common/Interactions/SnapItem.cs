using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Item))]
public class SnapItem : MonoBehaviour
{
    [SerializeField] private string m_snapType;
    private Item m_item;
    private SnapZone m_currentProximitySnapZone;
    private SnapZone m_currentSnapZone;
    private Transform m_currentSnapSpot;

    public UnityEvent OnSnapped;
    public UnityEvent OnUnsnapped;
    private bool m_isSnapped;
    private Rigidbody m_body;
    
    private void Start()
    {
        m_body = GetComponent<Rigidbody>();
        m_item = GetComponent<Item>();
        m_item.m_onDropped.AddListener(OnDroppedHandler);
        m_item.m_onGrabbed.AddListener(OnGrabHandler);
    }

    private void OnGrabHandler()
    {
        // Free the spot if the object was snapped
        if (m_currentSnapZone!=null && m_currentSnapSpot !=null )
        {
            m_currentSnapZone.FreeSpot(m_currentSnapSpot);
            m_currentSnapZone = null;
            m_currentSnapSpot = null;
            OnUnsnapped?.Invoke();
            m_isSnapped = false;
        }
    }
    
    private void OnDroppedHandler()
    {
        if (m_currentProximitySnapZone == null)
        {
            return;
        }

        if (m_currentProximitySnapZone.TrySnapItem(out Transform spotTransform))
        {
            m_currentSnapZone = m_currentProximitySnapZone;
            m_currentSnapSpot = spotTransform;
            transform.SetParent(spotTransform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            if (m_body) m_body.isKinematic = true;
            m_isSnapped = true;
            OnSnapped?.Invoke();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SnapZone zone))
        {
            m_currentProximitySnapZone = zone;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out SnapZone zone) && m_currentProximitySnapZone == zone)
        {
            m_currentProximitySnapZone = null;
        }
    }
}
