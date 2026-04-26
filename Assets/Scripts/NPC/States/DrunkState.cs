using UnityEngine;

public class DrunkState : State
{
    private const float speed = 0.6f;
    public DrunkState(NPCBase npc) : base(npc) { }

    public override void EnterState()
    {
        animator.SetBool("isDrunkWalking", true);
        agent.isStopped = false;
        agent.speed = speed;
        npc.SetRandomDestination();
        animator.applyRootMotion = false;
    }
    public override void QuitState()
    {
        animator.SetBool("isDrunkWalking", false);
        animator.SetFloat("Speed", 0f);
        agent.isStopped = true;
    }

    public override void Update()
    {
        if ((agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending))
        {
            agent.isStopped = true;
            npc.SwitchToState(npc.getDefaultState());
        }
    }
}
