using System.Collections.Generic;
using UnityEngine;

public class TalkingSmokingNPC : NPCBase
{
    protected override void Start()
    {
        base.Start();
        idleStates = new List<State>
        {
            new TalkingState(this),
            new SmokingState(this),
            new LookingAtWatchState(this)
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
