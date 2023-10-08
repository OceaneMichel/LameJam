using UnityEngine;

public class Wings : MonoBehaviour
{
    [SerializeField] private Animation m_wingsAnim;
    [SerializeField] private AnimationClip m_openClip;
    [SerializeField] private AnimationClip m_closeClip;

    private WindColumn currentWindColumn;
    public WindColumn CurrentWindColumn => currentWindColumn;
    private float accumulatedGravity = 0f;
    public float AccumulatedGravity => accumulatedGravity;
    
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
            this.currentWindColumn = column;
            accumulatedGravity = Time.time;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out WindColumn column))
        {
            accumulatedGravity = ((Time.time - accumulatedGravity)*currentWindColumn.GravityInside)/Time.deltaTime;
            this.currentWindColumn = null;
        }
    }
}
