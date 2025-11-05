using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "PatrolState", menuName = "FSM/States/PatrolState")]
public class PatrolState : State
{
    public float moveSpeed = 3f;

    private int currentIndex = 0;
    private NavMeshAgent agent;
    private Transform[] patrolPoints;

    public override void EnterState(StateMachine stateMachine)
    {
        agent = stateMachine.GetComponent<NavMeshAgent>();
        patrolPoints = stateMachine.patrolPoints;

        if (agent == null || patrolPoints == null || patrolPoints.Length == 0)
            return;

        agent.speed = moveSpeed;
        currentIndex = 0;
        agent.SetDestination(patrolPoints[currentIndex].position);
    }

    public override void UpdateState(StateMachine stateMachine)
    {
        if (patrolPoints == null || patrolPoints.Length == 0 || agent == null)
            return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            currentIndex = (currentIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentIndex].position);
        }
    }
}
