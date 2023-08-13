using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Item m_currentItemInHand;

    [SerializeField] private List<Item> m_closestItems;

    private JamControls m_controls;
    private InputAction m_grabAction;
    private bool m_grabbing;

    private void Start()
    {
        if (m_controls == null)
        {
            m_controls = new JamControls();
            m_controls.Enable();
        }

        // Base controls
        m_grabAction ??= m_controls.Basic.GrabAction;
        m_grabAction.performed += GrabActionPerformedHandler;
        m_closestItems = new List<Item>();
    }

    private void GrabActionPerformedHandler(InputAction.CallbackContext callbackContext)
    {
        if (m_grabbing)
        {
            // Ungrab current
            if (m_currentItemInHand != null)
            {
                m_currentItemInHand.IsGrabbed = false;
                m_grabbing = false;
            }

            return;
        }
        
        // Grab closest
        if (m_closestItems.Count > 0)
        {
            var closestItem = m_closestItems.OrderBy(t=>(t.transform.position - transform.position).sqrMagnitude)
                .FirstOrDefault();
            if (closestItem)
            {
                m_currentItemInHand = closestItem;
                m_grabbing = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("trigger enter");
        if (other.TryGetComponent(out Item item) && !m_closestItems.Contains(item))
        {
            m_closestItems.Add(item);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("trigger exit");
        if (other.TryGetComponent(out Item item) && m_closestItems.Contains(item))
        {
            m_closestItems.Remove(item);
        }
    }
}