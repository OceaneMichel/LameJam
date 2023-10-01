using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    [SerializeField] private Camera m_cameraA;
    [SerializeField] private Camera m_cameraB;
    [SerializeField] private Material m_cameraMatA;
    [SerializeField] private Material m_cameraMatB;

    private void Start()
    {
        if (m_cameraA.targetTexture != null) m_cameraA.targetTexture.Release();
        if (m_cameraB.targetTexture != null) m_cameraB.targetTexture.Release();

        m_cameraA.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        m_cameraB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        m_cameraMatA.mainTexture = m_cameraA.targetTexture;
        m_cameraMatB.mainTexture = m_cameraB.targetTexture;
    }
}
