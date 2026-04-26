using UnityEngine;
using UnityEngine.AI;

public class PanicState : State
{
    private const float speed = 5f;
    public PanicState(NPCBase npc) : base(npc) { }

    public override void EnterState()
    {
        animator.SetBool("isPanicing", true);
        animator.SetFloat("Speed", speed);
        agent.isStopped = false;
        agent.speed = speed;
        npc.SetRandomDestination();
        animator.applyRootMotion = false;
    }

    public override void QuitState()
    {
        animator.SetBool("isPanicing", false);
        agent.isStopped = true;
    }

    public override void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            npc.SetRandomDestination();
        }
    }
}