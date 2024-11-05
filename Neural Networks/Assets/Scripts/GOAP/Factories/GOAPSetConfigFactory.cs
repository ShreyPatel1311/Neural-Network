using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Classes.Builders;
using CrashKonijn.Goap.Configs.Interfaces;
using CrashKonijn.Goap.Enums;

public class GOAPSetConfigFactory : GoapSetFactoryBase
{
    public override IGoapSetConfig Create()
    {
        GoapSetBuilder builder = new("ENDEAVOURED");
        BuildGoals(builder);
        BuildActions(builder);
        BuildSensors(builder);
        return builder.Build();
    }

    private void BuildActions(GoapSetBuilder builder)
    {
        builder.AddAction<PatrolAction>().SetTarget<PatrolTargets>().AddEffect<IsWandering>(EffectType.Increase).SetBaseCost(5).SetInRange(10);
    }

    private void BuildSensors(GoapSetBuilder builder)
    {
        builder.AddTargetSensor<PatrolTargetSensor>().SetTarget<PatrolTargets>();
    }

    private void BuildGoals(GoapSetBuilder builder)
    {
        builder.AddGoal<PatrolGoal>().AddCondition<IsWandering>(CrashKonijn.Goap.Resolver.Comparison.GreaterThanOrEqual, 1);
    }
}