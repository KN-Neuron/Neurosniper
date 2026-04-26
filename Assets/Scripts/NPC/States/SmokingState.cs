using UnityEngine;

public class SmokingState : State
{
    private const float duration = 12f;
    private float currentDuration;

    public SmokingState(NPCBase npc) : base(npc) { }

    public override void EnterState()
    {
        animator.SetBool("isSmoking", true);
        agent.isStopped = true;
        currentDuration = duration;
    }

    public override void QuitState()
    {
        animator.SetBool("isSmoking", false);
    }

    public override void Update()
    {
        currentDuration -= Time.deltaTime;
        if (currentDuration <= 0f)
        {
            npc.SwitchToState(npc.getDefaultState());
        }
    }
}
