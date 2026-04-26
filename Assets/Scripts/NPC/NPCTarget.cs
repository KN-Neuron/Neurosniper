using System.Collections.Generic;

public class NPCTarget : NPCBase
{
    protected override void Start()
    {
        base.Start();
        idleStates = new List<State>
        {
            new IdleState(this),
            new WalkingState(this),
            new LookingNervouslyState(this)
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