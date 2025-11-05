using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "ApproachGroupState", menuName = "FSM/States/ApproachGroupState")]
public class ApproachGroupState : State
{
    public float moveSpeed = 3.5f;
    public float detectionRadius = 10f;
    public LayerMask unitMask;

    private NavMeshAgent agent;

    public override void EnterState(StateMachine stateMachine)
    {
        agent = stateMachine.GetComponent<NavMeshAgent>();
        if (agent != null) agent.speed = moveSpeed;
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        if (agent == null) return;

        Collider[] units = Physics.OverlapSphere(stateMachine.transform.position, detectionRadius, unitMask);
        if (units.Length == 0) return;

        Vector3 center = Vector3.zero;
        foreach (var u in units)
            center += u.transform.position;
        center /= units.Length;

        if (NavMesh.SamplePosition(center, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
    }
}
