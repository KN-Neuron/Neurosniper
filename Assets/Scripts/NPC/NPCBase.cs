using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public abstract class NPCBase : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Animator animator;
    protected State state;
    protected List<State> idleStates;

    public float movementRadius = 50f;

    protected virtual void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        state.Update();
    }

    public void SwitchToState(State state)
    {
        this.state?.QuitState();
        this.state = state;
        this.state?.EnterState();
    }

    public void getRandomStateAfterIdle()
    {
        int id = Random.Range(0, idleStates.Count);
        SwitchToState(idleStates[id]);
    }

    public void TriggerPanic()
    {
        if (state.GetType() != typeof(PanicState))
        {
            SwitchToState(new PanicState(this));
        }
    }

    public void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * movementRadius + transform.position;
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, movementRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    public abstract State getDefaultState();

    protected virtual void OnEnable()
    {
        DroneSpotlight.OnPlayerSpotted += TriggerPanic;
    }

    protected virtual void OnDisable()
    {
        DroneSpotlight.OnPlayerSpotted -= TriggerPanic;
    }
}


