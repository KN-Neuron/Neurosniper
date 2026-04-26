using System.Collections.Generic;
using UnityEngine.UI;

public class NPCWalking : NPCBase
{
    protected override void Start()
    {
        base.Start();
        idleStates = new List<State>
        {
            new IdleState(this),
            new WalkingState(this),
            //new BoredState(this),
            //new TextingState(this),
            //new LookingBehindState(this)
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