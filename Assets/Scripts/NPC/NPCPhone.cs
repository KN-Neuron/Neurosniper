using System.Collections.Generic;
using UnityEngine;

public class NPCPhone : NPCBase
{
    protected override void Start()
    {
        base.Start();
        idleStates = new List<State>
        {
            new IdleState(this),
            new TalkingOnThePhoneState(this),
            new TextingState(this)
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
