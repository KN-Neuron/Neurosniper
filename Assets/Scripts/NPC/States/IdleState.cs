using UnityEngine;
public class IdleState : State
{
    private float currentDuration;
    private const float idleTimeMin = 2f;
    private const float idleTimeMax = 5f;
    public IdleState(NPCBase npc) : base(npc) {}

    public override void EnterState()
    {
        currentDuration = Random.Range(idleTimeMin, idleTimeMax);
        animator.SetBool("isIdle", true);
        agent.isStopped = true;
    }
    public override void QuitState()
    {
        animator.SetBool("isIdle", false);
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