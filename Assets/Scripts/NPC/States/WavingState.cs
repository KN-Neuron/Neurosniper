using UnityEngine;

public class WavingState : State
{
    private const float duration = 5f;
    private float currentDuration;

    public WavingState(NPCBase npc) : base(npc) { }

    public override void EnterState()
    {
        animator.SetBool("isWaving", true);
        agent.isStopped = true;
        currentDuration = duration;
    }

    public override void QuitState()
    {
        animator.SetBool("isWaving", false);
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
