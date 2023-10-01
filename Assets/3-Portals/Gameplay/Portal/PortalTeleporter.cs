using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.Events;

public class PortalTeleporter : MonoBehaviour
{
    // Cam
    [SerializeField] private CinemachineVirtualCamera m_virtualCam;

    [SerializeField] private CharacterController m_playerController;
    [SerializeField] private ThirdPersonController m_tpsController;
    [SerializeField] private Transform m_player;
    [SerializeField] private Transform m_targetReceiver;

    [SerializeField] private Vector3 m_newSize;
    [SerializeField] private float m_newSpeed;
    [SerializeField] private float m_newCameraDistanceToPlayer;

    public UnityEvent OnTeleported;
    
    private bool playerIsOverlapping = false;

    private void Update()
    {
        if (playerIsOverlapping)
        {
            // Check where the player is coming from
            Vector3 portalToPlayer = m_player.position - transform.position;
            float dotProduct = Vector3.Dot(transform.forward, portalToPlayer);
            if (dotProduct < 0f)
            {
                // Teleport 
                float rotationDiff = Quaternion.Angle(transform.rotation, m_targetReceiver.rotation);
                rotationDiff += 180;
                m_player.Rotate(Vector3.up, rotationDiff);
                Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * portalToPlayer;
                
                // Move player controller
                Vector3 playerOldPosition = m_player.position;
                m_playerController.enabled = false;
                m_player.position = m_targetReceiver.position + positionOffset;
                
                // Change player size
                m_player.localScale = m_newSize;
                m_tpsController.MoveSpeed = m_newSpeed;
                m_tpsController.SprintSpeed = m_newSpeed;
                var componentBase = m_virtualCam.GetCinemachineComponent(CinemachineCore.Stage.Body);
                if (componentBase is Cinemachine3rdPersonFollow)
                {
                    (componentBase as Cinemachine3rdPersonFollow).CameraDistance = m_newCameraDistanceToPlayer;
                }
                m_playerController.enabled = true;

                // Move camera
                m_virtualCam.OnTargetObjectWarped(m_player, m_player.position - playerOldPosition);
                m_virtualCam.PreviousStateIsValid = false;
                OnTeleported?.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            playerIsOverlapping = false;
        }
    }
}