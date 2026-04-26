using System.Collections.Generic;
using UnityEngine;

public class NPC_bored : NPCBase
{
    protected override void Start()
    {
        base.Start();
        idleStates = new List<State>
        {
            new BoredState(this),
            new LookingAtWatchState(this),
            new LookingBehindState(this)
        };
        SwitchToState(idleStates[0]);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override State getDefaultState()
    {
        return idleStates[0];
    }
}
