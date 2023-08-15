using System;
using UnityEngine;

public class Extinguisher : MonoBehaviour
{
    [SerializeField] private Item m_item;
    [SerializeField] private ParticleSystem m_particleSystem;
    [SerializeField] private Transform m_extinguisherParent;
    
    [SerializeField] private GameObject m_hose;
    [SerializeField] private GameObject m_extremity;
    [SerializeField] private GameObject m_etremityCollider;
    
    [SerializeField] private Vector3 m_leftHandExtremityPos;
    [SerializeField] private Vector3 m_LeftHandExtremityRot;

    [Header("SFX")]
    [SerializeField] private AudioSource m_source;
    [SerializeField] private AudioClip m_onStartClip;
    [SerializeField] private AudioClip m_runningClip;
    [SerializeField] private AudioClip m_onStopClip;
    
    
    private Vector3 m_startExtremityPos;
    private Vector3 m_startExtremityRot;
    
    private void Start()
    {
        m_startExtremityPos = m_extremity.transform.localPosition;
        m_startExtremityRot = m_extremity.transform.localEulerAngles;
        m_item.m_onActionStarted.AddListener(ExtinguisherStarted);
        m_item.m_onActionRunning.AddListener(ExtinguisherRunning);
        m_item.m_onActionEnded.AddListener(ExtinguisherStopped);
    }

    private void OnDestroy()
    {
        m_item.m_onActionStarted.RemoveListener(ExtinguisherStarted);
        m_item.m_onActionRunning.RemoveListener(ExtinguisherRunning);
        m_item.m_onActionEnded.RemoveListener(ExtinguisherStopped);    }

    private void ExtinguisherStarted()
    {
        m_source.loop = false;
        m_source.clip = m_onStartClip;
        m_source.Play();
    }

    private void ExtinguisherRunning()
    {
        if (m_source.isPlaying)
        {
            return;
        }
        m_source.loop = true;
        m_source.clip = m_runningClip;
        m_source.Play();
    }

    private void ExtinguisherStopped()
    {
        m_source.loop = false;
        m_source.clip = m_onStopClip;
        m_source.Play();
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
