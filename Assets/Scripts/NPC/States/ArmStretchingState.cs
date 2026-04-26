using UnityEngine;

public class ArmStretchingState : State
{
    public const float duration = 20f;
    private float currentDuration;

    public ArmStretchingState(NPCBase npc) : base(npc) { }

    public override void EnterState()
    {
        animator.SetBool("isArmStretching", true);
        agent.isStopped = true;
        currentDuration = duration;
    }

    public override void QuitState()
    {
        animator.SetBool("isArmStretching", false);
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
