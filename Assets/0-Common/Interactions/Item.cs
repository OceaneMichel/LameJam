using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Outline))]
public class Item : MonoBehaviour
{
    [SerializeField] private Usage m_usage;
    
    [SerializeField] private UnityEvent m_onActionStarted;
    [SerializeField] private UnityEvent m_onActionRunning;
    [SerializeField] private UnityEvent m_onActionEnded;

    private JamControls m_controls;
    private InputAction m_usageAction;

    // Item components parameters
    private Rigidbody m_body;
    private bool m_useGravity;
    private Collider m_collider;
    private Outline m_outline;
    
    private bool m_isGrabbed;
    private bool m_actionRunning;

    private enum Usage
    {
        Once,
        Holding,
        Toggle
    }

    private void Awake()
    {
        m_outline = GetComponent<Outline>();
        m_body = GetComponent<Rigidbody>();
        m_collider = GetComponent<Collider>();
        if (m_body)
        {
            m_useGravity = m_body.useGravity;
        }
    }

    private void Start()
    {
        if (m_controls == null)
        {
            m_controls = new JamControls();
            m_controls.Enable();
        }
        
        // Base controls
        m_usageAction ??= m_controls.Basic.MouseButton;

        if (m_usage == Usage.Once)
        {
            m_usageAction.performed += ActionButtonPerformedHandler;
            return;
        }
        
        m_usageAction.started += ActionButtonStarted;
        m_usageAction.canceled += ActionButtonEnded;
    }

    private void Update()
    {
        if (!m_isGrabbed || m_usage != Usage.Holding)
        {
            return;
        }

        PerformActionContinue();
        m_onActionRunning?.Invoke();
    }

    protected virtual void PerformActionContinue()
    {
        Debug.Log("Do action in continue");
    }
    
    private void ActionButtonStarted(InputAction.CallbackContext callbackContext)
    {
        // Start action
        Debug.Log($"Start action on {this.name}");
        m_onActionStarted?.Invoke();
        m_actionRunning = true;
    }

    private void ActionButtonEnded(InputAction.CallbackContext callbackContext)
    {
        // Stop action
        Debug.Log($"Stop action on {this.name}");
        m_onActionEnded?.Invoke();
        m_actionRunning = false;
    }

    private void ActionButtonPerformedHandler(InputAction.CallbackContext callbackContext)
    {
        Debug.Log($"Do single action on {this.name}");
        m_onActionStarted?.Invoke();
    }

    public void Grab(bool grab)
    {
        if (m_body)
        {
            m_body.useGravity = !grab && m_useGravity;
        }

        m_collider.isTrigger = grab;
        m_isGrabbed = grab;
    }

    public void Highlight(bool highlight)
    {
        m_outline.enabled = highlight;
    }
}