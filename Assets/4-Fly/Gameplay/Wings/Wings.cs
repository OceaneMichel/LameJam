using UnityEngine;

public class Wings : MonoBehaviour
{
    [SerializeField] private Animation m_wingsAnim;
    [SerializeField] private AnimationClip m_openClip;
    [SerializeField] private AnimationClip m_closeClip;

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
}
