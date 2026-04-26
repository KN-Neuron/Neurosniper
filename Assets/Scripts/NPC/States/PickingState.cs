using UnityEngine;

public class PickingState : State
{
    private const float duration = 5f;
    private float currentDuration;

    public PickingState(NPCBase npc) : base(npc) { }

    public override void EnterState()
    {
        animator.SetBool("isPicking", true);
        agent.isStopped = true;
        currentDuration = duration;
    }

    public override void QuitState()
    {
        animator.SetBool("isPicking", false);
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
