using UnityEngine;
using UnityEngine.InputSystem;

public class Laser : MonoBehaviour
{
    [SerializeField] private MeshBool2d m_meshBoolPlane;
    [SerializeField] private LineRenderer m_laserLR;
    [SerializeField] private ParticleSystem m_sparklesParticles;
    [SerializeField] private float m_laserSize;

    private JamControls m_controls;
    private InputAction m_mousePressedAction;
    private InputAction m_mousePositionAction;

    private void Awake()
    {
        if (m_controls == null)
        {
            m_controls = new JamControls();
            m_controls.Enable();
            m_mousePressedAction = m_controls.Basic.MouseButton;
            m_mousePositionAction = m_controls.Basic.MousePosition;
        }
    }

    private void Update()
    {
        if (!m_mousePressedAction.IsPressed())
        {
            return;
        }

        MoveLaser();
    }

    private void MoveLaser()
    {
        Ray ray = Camera.main.ScreenPointToRay (m_mousePositionAction.ReadValue<Vector2>());
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            transform.LookAt(hit.point); 

            m_meshBoolPlane.RemoveCircle(hit.point, m_laserSize, 12);
            m_sparklesParticles.transform.position = hit.point;
            m_sparklesParticles.Play();
            m_laserLR.SetPosition(1, Vector3.forward*((hit.point-transform.position).magnitude-0.5f));
        }
    }
    
    public static float ClampAngle(float current, float min, float max)
    {
        float dtAngle = Mathf.Abs(((min - max) + 180) % 360 - 180);
        float hdtAngle = dtAngle * 0.5f;
        float midAngle = min + hdtAngle;
 
        float offset = Mathf.Abs(Mathf.DeltaAngle(current, midAngle)) - hdtAngle;
        if (offset > 0)
            current = Mathf.MoveTowardsAngle(current, midAngle, offset);
        return current;
    }
}