using System.Collections;
using StarterAssets;
using UnityEngine;

public class FlyingState : StateMachineBehaviour
{
    [SerializeField] private float m_minFlyingHeight;
    [SerializeField] private float m_flyingGravity;
    [SerializeField] private float m_movingSpeed;
    
    private Rigidbody rigidbody;
    private ThirdPersonController tpsController;
    private Wings wings;
    private float previousGravity;
    private float previousMoveSpeed;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.TryGetComponent(out Rigidbody rb))
            rigidbody = rb;

        if (animator.TryGetComponent(out ThirdPersonController tps))
        {
            tpsController = tps;
            previousGravity = tps.Gravity;
            previousMoveSpeed = tps.MoveSpeed;
            
            tpsController.Gravity = m_flyingGravity;
            tpsController.MoveSpeed = m_movingSpeed;
        }

        if (Physics.Raycast(animator.transform.position, -Vector3.up, out RaycastHit hit))
        {
            if (Vector3.Distance(hit.point, animator.transform.position) > m_minFlyingHeight)
            {
                wings = animator.GetComponentInChildren<Wings>(true);
                // Open wings
                wings.OpenPad();
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Add upward force
        if (!tpsController)
        {
            return;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Close wings
        if (wings)
        {
            wings.ClosePad();
        }

        if (animator.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
        }

        if (tpsController)
        {
            tpsController.Gravity = previousGravity;
            tpsController.MoveSpeed = previousMoveSpeed;
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
