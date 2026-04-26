using UnityEngine;
using UnityEngine.AI;

public class WalkingState : State
{
    private const float speed = 2f;
    public WalkingState(NPCBase npc) : base(npc) { }

    public override void EnterState()
    {
        animator.SetBool("isWalking", true);
        agent.isStopped = false;
        agent.speed = speed;
        npc.SetRandomDestination();
        animator.applyRootMotion = false;
    }

    public override void QuitState()
    {
        animator.SetBool("isWalking", false);
        animator.SetFloat("Speed", 0f);
        agent.isStopped = true;
    }

    public override void Update()
    {
        Vector3 velocity = agent.velocity;
        float speed = velocity.magnitude;
        animator.SetFloat("Speed", speed);
        
        if ((agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending))
        {
            agent.isStopped = true;
            npc.SwitchToState(npc.getDefaultState());
        }
    }
}