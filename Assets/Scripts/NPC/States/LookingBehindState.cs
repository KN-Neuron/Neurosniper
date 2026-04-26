using UnityEngine;

public class LookingBehindState : State
{
    private const float duration = 4f;
    private float currentDuration;

    public LookingBehindState(NPCBase npc) : base(npc) { }

    public override void EnterState()
    {
        animator.SetBool("isLookingBehind", true);
        agent.isStopped = true;
        currentDuration = duration;
    }

    public override void QuitState()
    {
        animator.SetBool("isLookingBehind", false);
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