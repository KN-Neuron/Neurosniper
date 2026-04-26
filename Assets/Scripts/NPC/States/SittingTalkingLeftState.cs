public class SittingTalkingLeftState : State
{
    public SittingTalkingLeftState(NPCBase npc) : base(npc) { }

    public override void EnterState()
    {
        animator.SetBool("isSittingTalkingLeft", true);
        agent.isStopped = true;
    }

    public override void QuitState()
    {
        animator.SetBool("isSittingTalkingLeft", false);
    }

    public override void Update()
    {
        // Sitting and talking state does not have a duration; remains until changed
    }
}