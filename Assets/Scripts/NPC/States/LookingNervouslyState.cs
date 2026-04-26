using UnityEngine;

public class LookingNervouslyState : State
{
    private const float duration = 6f;
    private float currentDuration;

    public LookingNervouslyState(NPCBase npc) : base(npc) { }

    public override void EnterState()
    {
        animator.SetBool("isLookingNervously", true);
        agent.isStopped = true;
        currentDuration = duration;
    }

    public override void QuitState()
    {
        animator.SetBool("isLookingNervously", false);
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