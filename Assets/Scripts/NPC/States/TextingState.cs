using UnityEngine;
using UnityEngine.Rendering;

public class TextingState : State
{
    public const float duration = 23f;
    private float currentDuration;

    public TextingState(NPCBase npc) : base(npc) { }

    public override void EnterState()
    {
        animator.SetBool("isTextingWhileStanding", true);
        agent.isStopped = true;
        currentDuration = duration;
    }

    public override void QuitState()
    {
        animator.SetBool("isTextingWhileStanding", false);
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