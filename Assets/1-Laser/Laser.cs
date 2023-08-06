using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
   [SerializeField] private MeshBool2d m_meshBoolPlane;
   [SerializeField] private float m_laserSize;
   
   private void Update()
   {
      
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.red;
   }
}
