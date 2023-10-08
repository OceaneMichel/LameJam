using UnityEngine;

public class Wings : MonoBehaviour
{
    [SerializeField] private Animation m_wingsAnim;
    [SerializeField] private AnimationClip m_openClip;
    [SerializeField] private AnimationClip m_closeClip;

    private WindColumn currentWindColumn;
    public WindColumn CurrentWindColumn => currentWindColumn;
    
    private bool isOpen;
    public void OpenPad()
    {
        if (isOpen) return;
        m_wingsAnim.clip = m_openClip;
        m_wingsAnim.Play();
        isOpen = true;
    }

    public void ClosePad()
    {
        if (!isOpen) return;
        
        m_wingsAnim.clip = m_closeClip;
        m_wingsAnim.Play();
        isOpen = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Do not use layer or tag to not pollute future jams
        if (other.TryGetComponent(out WindColumn column))
        {
            currentWindColumn = column;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out WindColumn column))
        {
            currentWindColumn = null;
        }
    }
}
