using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
   [SerializeField] private Transform m_playerCamera;
   [SerializeField] private Transform m_portal;
   [SerializeField] private Transform m_otherPortal;

   private void Update()
   {
      Vector3 playerOffsetFromPortal = m_playerCamera.position - m_otherPortal.position;
      transform.position = m_portal.position + playerOffsetFromPortal;
      
      // angle difference
      float angularDifferenceBetweenPortalRotations = Quaternion.Angle(m_portal.rotation, m_otherPortal.rotation);
      Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
      Vector3 newCameraDir = portalRotationalDifference * m_playerCamera.forward;
      transform.rotation = Quaternion.LookRotation(newCameraDir, Vector3.up);
   }
}
