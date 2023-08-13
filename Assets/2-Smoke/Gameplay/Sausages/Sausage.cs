using System;
using System.Collections;
using UnityEngine;

public class Sausage : MonoBehaviour
{
    [SerializeField] private float m_cookingDuration;
    [SerializeField] private float m_burningDuration;
    [SerializeField] private float m_putOffFireDuration;

    [SerializeField] private SnapItem m_sausageSnapItem;
    [SerializeField] private Renderer m_sausageRend;
    
    [SerializeField] private Color m_rawColor;
    [SerializeField] private Color m_cookedColor;
    [SerializeField] private Color m_burnedColor;

    [Header("VFX")]
    [SerializeField] private GameObject m_mainVFX;
    [SerializeField] private Transform[] m_vfx;
    [SerializeField] private GameObject m_smokeVFX;
    
    private float m_cookingElapsedTime = 0f;
    private float m_puttingOffElapsedTime = 0f;
    private bool m_onBarbecue;
    private bool m_burnt;
    private bool m_puttingOffFire;
    private bool m_fireExtinguished;
    
    private void Start()
    {
        m_sausageSnapItem.OnSnapped.AddListener(OnBarbecueSnapped);
        m_sausageSnapItem.OnUnsnapped.AddListener(OnBarbecueUnSnapped);
    }

    private void Update()
    {
        if (!m_onBarbecue || m_fireExtinguished)
        {
            return;
        }

        if (m_burnt)
        {
            if (m_puttingOffFire)
            {
                // Count elapsed time to extinguish fire
                m_puttingOffElapsedTime += Time.deltaTime;
                Extinguish();

                if (m_puttingOffElapsedTime >= m_putOffFireDuration)
                {
                    // Fire extinguished!!
                    // Add smoke and kill fire
                    m_fireExtinguished = true;
                    m_mainVFX.SetActive(false);
                    m_smokeVFX.SetActive(true);
                }
            }

            return;
        }

        // Cook the sausage
        m_cookingElapsedTime += Time.deltaTime;
        UpdateSausageColor();
        if (m_cookingElapsedTime >= m_cookingDuration)
        {
            // Trigger fire!
            StartCoroutine(BurningRoutine());
        }
    }

    private void OnBarbecueSnapped()
    {
        // Add cooking time and update visual
        m_onBarbecue = true;
    }

    private void OnBarbecueUnSnapped()
    {
        m_onBarbecue = false;
    }

    private void UpdateSausageColor()
    {
        m_sausageRend.material.color = Color.Lerp(m_rawColor, m_cookedColor, m_cookingElapsedTime / m_cookingDuration);
    }

    private IEnumerator BurningRoutine()
    {
        m_mainVFX.SetActive(true);
        float elapsed = 0f;
        while (elapsed < m_burningDuration)
        {
            foreach (var vfx in m_vfx)
            {
                vfx.localScale = Vector3.one * elapsed / m_burningDuration;
            }
            
            m_sausageRend.material.color = Color.Lerp(m_cookedColor, m_burnedColor, elapsed / m_burningDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        foreach (var vfx in m_vfx)
        {
            vfx.localScale = Vector3.one;
        }
        m_burnt = true;
    }

    private void Extinguish()
    {
        foreach (var vfx in m_vfx)
        {
            vfx.localScale = Vector3.one * (1- m_puttingOffElapsedTime/m_putOffFireDuration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_fireExtinguished)
        {
            return;
        }
        // wanna be quick with dev yup
        if (other.name.Equals("ConeCollider"))
        {
            StopAllCoroutines();
            m_puttingOffFire = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_fireExtinguished)
        {
            return;
        }
        if (other.name.Equals("ConeCollider"))
        {
            m_puttingOffFire = false;
        }
    }
}
