public class SittingState : State
{
    public SittingState(NPCBase npc) : base(npc) { }

    public override void EnterState()
    {
        animator.SetBool("isSitting", true);
        agent.isStopped = true;
    }

    public override void QuitState()
    {
        animator.SetBool("isSitting", false);
    }

    public override void Update()
    {
        // Sitting state does not have a duration; remains until changed
    }
}