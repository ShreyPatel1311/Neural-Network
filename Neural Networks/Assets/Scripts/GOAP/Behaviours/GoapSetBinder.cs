using CrashKonijn.Goap.Behaviours;
using UnityEngine;

public class GoapSetBinder : MonoBehaviour
{
    [SerializeField] private GoapRunnerBehaviour runner;
    void Awake()
    {
        AgentBehaviour agent = GetComponent<AgentBehaviour>();
        agent.GoapSet = runner.GetGoapSet("ENDEAVOURED");
    }
}