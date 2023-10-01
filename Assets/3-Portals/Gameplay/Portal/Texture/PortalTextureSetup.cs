using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    [SerializeField] private Camera m_cameraB;
    [SerializeField] private Material m_cameraMatB;

    private void Start()
    {
        if (m_cameraB.targetTexture != null)
        {
            m_cameraB.targetTexture.Release();
        }

        m_cameraB.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        m_cameraMatB.mainTexture = m_cameraB.targetTexture;
    }
}
