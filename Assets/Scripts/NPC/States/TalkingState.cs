using UnityEngine;

public class TalkingState : State
{
    private const float minDuration = 3f;
    private const float maxDuration = 10f;
    private float currentDuration;

    public TalkingState(NPCBase npc) : base(npc) { }

    public override void EnterState()
    {
        animator.SetBool("isTalking", true);
        agent.isStopped = true;
        currentDuration = Random.Range(minDuration, maxDuration);
    }

    public override void QuitState()
    {
        animator.SetBool("isTalking", false);
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
