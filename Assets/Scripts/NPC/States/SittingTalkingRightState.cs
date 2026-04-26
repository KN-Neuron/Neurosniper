public class SittingTalkingRightState : State
{
    public SittingTalkingRightState(NPCBase npc) : base(npc) { }

    public override void EnterState()
    {
        animator.SetBool("isSittingTalkingRight", true);
        agent.isStopped = true;
    }

    public override void QuitState()
    {
        animator.SetBool("isSittingTalkingRight", false);
    }

    public override void Update()
    {
        // Sitting and talking state does not have a duration; remains until changed
    }
}