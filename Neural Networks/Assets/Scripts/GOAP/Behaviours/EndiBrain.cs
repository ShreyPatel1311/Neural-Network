using CrashKonijn.Goap.Behaviours;
using UnityEngine;

[RequireComponent(typeof(AgentBehaviour))]
public class EndiBrain : MonoBehaviour
{
    private AgentBehaviour behaviour;   

    void Awake()
    {
        behaviour = GetComponent<AgentBehaviour>();
    }
    void Start()
    {
        behaviour.SetGoal<PatrolGoal>(false);
    }
}