using UnityEngine;

public class LookingAtWatchState : State
{
    private const float duration = 14f;
    private float currentDuration;

    public LookingAtWatchState(NPCBase npc) : base(npc) { }

    public override void EnterState()
    {
        animator.SetBool("isLookingAtWatch", true);
        agent.isStopped = true;
        currentDuration = duration;
    }

    public override void QuitState()
    {
        animator.SetBool("isLookingAtWatch", false);
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