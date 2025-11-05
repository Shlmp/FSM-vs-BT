using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "FleeState", menuName = "FSM/States/FleeState")]
public class FleeState : State
{
    public float moveSpeed = 4f;
    public float safeDistance = 15f;
    public float detectionRadius = 10f;
    public LayerMask unitMask;

    private NavMeshAgent agent;
    private Vector3 safeDestination;

    public override void EnterState(StateMachine stateMachine)
    {
        agent = stateMachine.GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = moveSpeed;
            safeDestination = GetSafePosition(stateMachine.transform.position);
            agent.SetDestination(safeDestination);
        }
    }

    private Vector3 GetSafePosition(Vector3 origin)
    {
        Collider[] units = Physics.OverlapSphere(origin, detectionRadius, unitMask);
        if (units.Length == 0) return origin;

        Vector3 center = Vector3.zero;
        foreach (var u in units)
            center += u.transform.position;
        center /= units.Length;

        Vector3 fleeDir = (origin - center).normalized;
        Vector3 target = origin + fleeDir * safeDistance;

        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            return hit.position;
        else
            return origin;
    }
}
