using System.Collections.Generic;

public class NPCSitting : NPCBase
{
    protected override void Start()
    {
        base.Start();
        idleStates = new List<State>
        {
            new SittingState(this)
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