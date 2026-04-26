using UnityEngine;
using UnityEngine.AI;

public abstract class State
{
    protected NPCBase npc;
    protected Animator animator;
    protected NavMeshAgent agent;
    public State(NPCBase npc)
    {
        this.npc = npc;
        animator = npc.transform.GetComponent<Animator>();
        agent = npc.transform.GetComponent<NavMeshAgent>();
    }

    public abstract void EnterState();
    public abstract void QuitState();
    public abstract void Update();

}