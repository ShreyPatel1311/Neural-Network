using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Enums;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;

public class PatrolAction : ActionBase<CommonData>
{
    public override void Created()
    {
        
    }

    public override void End(IMonoAgent agent, CommonData data)
    {
        
    }

    public override ActionRunState Perform(IMonoAgent agent, CommonData data, ActionContext context)
    {
        data.timer -= context.DeltaTime;

        if(data.timer > 0){
            return ActionRunState.Continue;
        }

        return ActionRunState.Stop;
    }

    public override void Start(IMonoAgent agent, CommonData data)
    {
        data.timer = Random.Range(1, 2);
    }
}
