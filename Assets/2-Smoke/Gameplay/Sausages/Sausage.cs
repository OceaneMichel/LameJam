using System.Collections;
using UnityEngine;

public class Sausage : MonoBehaviour
{
    [SerializeField] private float m_cookingDuration;
    [SerializeField] private float m_burningDuration;
    [SerializeField] private SnapItem m_sausageSnapItem;
    [SerializeField] private Renderer m_sausageRend;
    
    [SerializeField] private Color m_rawColor;
    [SerializeField] private Color m_cookedColor;
    [SerializeField] private Color m_burnedColor;

    [Header("VFX")]
    [SerializeField] private GameObject m_mainVFX;
    [SerializeField] private Transform[] m_vfx;
    
    private float m_cookingElapsedTime = 0f;
    private bool m_onBarbecue;
    private bool m_burnt;
    
    private void Start()
    {
        m_sausageSnapItem.OnSnapped.AddListener(OnBarbecueSnapped);
        m_sausageSnapItem.OnUnsnapped.AddListener(OnBarbecueUnSnapped);
    }

    private void Update()
    {
        if (!m_onBarbecue || m_burnt)
        {
            return;
        }

        m_cookingElapsedTime += Time.deltaTime;
        UpdateSausageColor();
        if (m_cookingElapsedTime >= m_cookingDuration)
        {
            m_burnt = true;
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
        
    }
}
