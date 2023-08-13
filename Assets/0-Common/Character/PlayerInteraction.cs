using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform m_handTransform;
    [SerializeField] private Item m_currentItemInHand;

    [SerializeField] private List<Item> m_closestItems;

    private JamControls m_controls;
    private InputAction m_grabAction;
    private bool m_grabbing;
    private Item m_previousHighlighted;

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
                m_currentItemInHand.transform.SetParent(m_currentItemInHand.BaseParent);
                m_currentItemInHand.Grab(false);
                m_grabbing = false;
                m_currentItemInHand = null;
            }

            return;
        }

        // Grab closest
        if (m_closestItems.Count > 0)
        {
            if (!TryGetClosest(out Item closestItem))
            {
                return;
            }

            m_currentItemInHand = closestItem;
            m_currentItemInHand.Grab(true);
            m_currentItemInHand.Highlight(false);
            m_grabbing = true;

            closestItem.transform.SetParent(m_handTransform);
            closestItem.transform.localPosition = Vector3.zero;
            closestItem.transform.localRotation = quaternion.identity;
        }
    }

    private bool TryGetClosest(out Item item)
    {
        item = m_closestItems.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude)
            .FirstOrDefault();
        return item != null;
    }

    private void Update()
    {
        if (m_grabbing)
        {
            return;
        }

        HighlightClosest();
    }

    private void HighlightClosest()
    {
        if (m_closestItems.Count > 0)
        {
            if (TryGetClosest(out Item closest))
            {
                if (m_previousHighlighted != null && closest != m_previousHighlighted)
                {
                    // Unhilight previous
                    m_previousHighlighted.Highlight(false);
                }

                closest.Highlight(true);
                m_previousHighlighted = closest;
            }
        }
        else if (m_previousHighlighted != null)
        {
            m_previousHighlighted.Highlight(false);
            m_previousHighlighted = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Item item) && !m_closestItems.Contains(item))
        {
            m_closestItems.Add(item);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Item item) && m_closestItems.Contains(item))
        {
            m_closestItems.Remove(item);
        }
    }
}