using System;
using CrashKonijn.Goap.Behaviours;
using CrashKonijn.Goap.Interfaces;
using UnityEngine;
using UnityEngine.AI;

public class ActionMovebehaviour : MonoBehaviour
{
    [SerializeField] private float MinMoveDistance = 0.25f;

    private Vector3 LastPosition;
    private NavMeshAgent agent;
    private Animator anim;
    private AgentBehaviour behaviour;
    private ITarget CurrentTarget;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        behaviour = GetComponent<AgentBehaviour>();
    }

    private void OnEnable()
    {
        behaviour.Events.OnTargetChanged += EventOnTargetChanged;
        behaviour.Events.OnTargetOutOfRange += EventOntargetOutOfRange;
    }

    private void OnDisable()
    {
        behaviour.Events.OnTargetChanged -= EventOnTargetChanged;
        behaviour.Events.OnTargetOutOfRange -= EventOntargetOutOfRange;
    }

    private void EventOntargetOutOfRange(ITarget target)
    {
        anim.SetBool("WALK", false);
    }

    private void EventOnTargetChanged(ITarget target, bool inRange)
    {
        CurrentTarget = target;
        agent.SetDestination(target.Position);
        anim.SetBool("WALK", true);
    }

    private void Update(){
        if(CurrentTarget == null){
            return;
        }
        if (MinMoveDistance < Vector3.Distance(CurrentTarget.Position, LastPosition)){
            LastPosition = CurrentTarget.Position;
            agent.SetDestination(CurrentTarget.Position);
            anim.SetBool("WALK", agent.velocity.magnitude >0.1f);
        }
    }
}