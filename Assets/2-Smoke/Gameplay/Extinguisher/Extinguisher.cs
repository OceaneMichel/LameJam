using UnityEngine;

public class Extinguisher : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_particleSystem;
    [SerializeField] private Transform m_extinguisherParent;
    
    [SerializeField] private GameObject m_hose;
    [SerializeField] private GameObject m_extremity;
    [SerializeField] private GameObject m_etremityCollider;
    
    [SerializeField] private Vector3 m_leftHandExtremityPos;
    [SerializeField] private Vector3 m_LeftHandExtremityRot;

    private Vector3 m_startExtremityPos;
    private Vector3 m_startExtremityRot;
    
    private void Start()
    {
        m_startExtremityPos = m_extremity.transform.localPosition;
        m_startExtremityRot = m_extremity.transform.localEulerAngles;
    }

    public void Grab()
    {
        m_hose.SetActive(false);
        m_extremity.transform.SetParent(PlayerInteraction.GetInstance().LeftHandTransform);
        m_extremity.transform.localPosition = m_leftHandExtremityPos;
        m_extremity.transform.localEulerAngles = m_LeftHandExtremityRot;
    }

    public void Drop()
    {
        m_hose.SetActive(true);
        m_extremity.transform.SetParent(m_extinguisherParent);
        m_extremity.transform.localPosition = m_startExtremityPos;
        m_extremity.transform.localEulerAngles = m_startExtremityRot;
        Deactivate();
    }

    public void Activate()
    {
        m_particleSystem.Play();
        m_etremityCollider.SetActive(true);
    }

    public void Deactivate()
    {
        m_particleSystem.Stop();
        m_etremityCollider.SetActive(false);
    }
}
