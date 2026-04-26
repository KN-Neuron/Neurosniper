using UnityEngine;

public class BoredState : State
{
    private const float duration = 10f;
    private float currentDuration;

    public BoredState(NPCBase npc) : base(npc) { }

    public override void EnterState()
    {
        animator.SetBool("isBored", true);
        agent.isStopped = true;
        currentDuration = duration;
    }

    public override void QuitState()
    {
        animator.SetBool("isBored", false);
    }

    public override void Update()
    {
        currentDuration -= Time.deltaTime;
        if (currentDuration <= 0f)
        {
            npc.getRandomStateAfterIdle();
        }
    }
}