using UnityEngine;
using UnityEngine.InputSystem;

public class Laser : MonoBehaviour
{
    [SerializeField] private MeshBool2d m_meshBoolPlane;
    [SerializeField] private float m_laserSize;
    [SerializeField] private float m_sensitivity;
    [SerializeField] private Vector2 m_clampX;
    [SerializeField] private Vector2 m_clampY;
    
    private JamControls m_controls;
    private InputAction m_moveAction;

    private void Awake()
    {
        if (m_controls == null)
        {
            m_controls = new JamControls();
            m_controls.Enable();
            m_moveAction = m_controls.Basic.Movement;
        }
    }

    private void Update()
    {
        if (!m_moveAction.IsPressed())
        {
            return;
        }

        MoveLaser();
    }

    private void MoveLaser()
    {
        // Debug.Log("Move laser");
        var moveValue = m_moveAction.ReadValue<Vector2>();
        transform.Rotate( new Vector2(-moveValue.y, moveValue.x) * (m_sensitivity * Time.deltaTime));
        
        // clamp rotations
        float xAngle = ClampAngle(transform.localEulerAngles.x, m_clampX.x, m_clampX.y);
        float yAngle = ClampAngle(transform.localEulerAngles.y, m_clampY.x, m_clampY.y);
        transform.localEulerAngles = new Vector3(xAngle, yAngle, 0);
        
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
        {
            m_meshBoolPlane.RemoveCircle(hit.point, m_laserSize, 40);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward*1000);
        Gizmos.DrawCube(transform.position, Vector3.one * 0.2f);
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