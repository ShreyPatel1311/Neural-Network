using CrashKonijn.Goap.Classes;
using CrashKonijn.Goap.Interfaces;
using CrashKonijn.Goap.Sensors;
using UnityEngine;
using UnityEngine.AI;

public class PatrolTargetSensor : LocalTargetSensorBase
{
    public override void Created()
    {
        
    }

    public override ITarget Sense(IMonoAgent agent, IComponentReference references)
    {
        return new PositionTarget(GetPosition(agent));
    }

    public override void Update()
    {
        
    }

    private Vector3 GetPosition(IMonoAgent agent)
    {
        int count = 0;
        while(count < 10)
        {
            Vector2 random = Random.insideUnitCircle * 5;
            Vector3 position = agent.transform.position + new Vector3(random.x, 0, random.y);
            if(NavMesh.SamplePosition(position, out NavMeshHit hit, 5, NavMesh.AllAreas))
            {
                return position;
            }
            count++;
        }

        return agent.transform.position;
    }
}
