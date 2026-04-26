using UnityEngine;

public class TalkingOnThePhoneState : State
{
    private const float duration = 23f;
    private float currentDuration;
    public TalkingOnThePhoneState(NPCBase npc) : base(npc) { }

    public override void EnterState()
    {
        animator.SetBool("isTalkingOnThePhone", true);
        agent.isStopped = true;
        currentDuration = duration;
    }

    public override void QuitState()
    {
        animator.SetBool("isTalkingOnThePhone", false);
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