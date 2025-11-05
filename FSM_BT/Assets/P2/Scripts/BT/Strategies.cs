using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Home.BehaviourTrees
{
    public interface IStrategy
    {
        Node.Status Process();
        void Reset();
    }

    // --------------------------------------------------------------------------------  PATROL STRATEGY
    public class PatrolStrategy : IStrategy
    {
        private readonly Transform entity;
        private readonly NavMeshAgent agent;
        private readonly List<Transform> patrolPoints;
        private readonly float patrolSpeed;
        private int currentIndex;

        public PatrolStrategy(Transform entity, NavMeshAgent agent, List<Transform> patrolPoints, float patrolSpeed = 2f)
        {
            this.entity = entity;
            this.agent = agent;
            this.patrolPoints = patrolPoints;
            this.patrolSpeed = patrolSpeed;
        }

        public Node.Status Process()
        {
            if (patrolPoints == null || patrolPoints.Count == 0) return Node.Status.Failure;
            if (agent == null) return Node.Status.Failure;

            agent.speed = patrolSpeed;

            if (!agent.hasPath && !agent.pathPending)
            {
                agent.SetDestination(patrolPoints[currentIndex].position);
            }

            float arrivalThreshold = Mathf.Max(agent.stoppingDistance, 0.4f);

            if (!agent.pathPending && agent.remainingDistance <= arrivalThreshold)
            {
                currentIndex = (currentIndex + 1) % patrolPoints.Count;
                agent.SetDestination(patrolPoints[currentIndex].position);
            }

            return Node.Status.Running;
        }


        public void Reset() => currentIndex = 0;
    }

    // --------------------------------------------------------------------------------  APPROACH GROUP STRATEGY
    public class ApproachGroupStrategy : IStrategy
    {
        private readonly Transform entity;
        private readonly NavMeshAgent agent;
        private readonly LayerMask unitMask;
        private readonly float detectionRadius;
        private readonly float moveSpeed;

        public ApproachGroupStrategy(Transform entity, NavMeshAgent agent, LayerMask unitMask, float detectionRadius, float moveSpeed = 3.5f)
        {
            this.entity = entity;
            this.agent = agent;
            this.unitMask = unitMask;
            this.detectionRadius = detectionRadius;
            this.moveSpeed = moveSpeed;
        }

        public Node.Status Process()
        {
            if (agent == null) return Node.Status.Failure;
            agent.speed = moveSpeed;

            Collider[] units = Physics.OverlapSphere(entity.position, detectionRadius, unitMask.value);
            Debug.Log($"[Approach] detected units: {units.Length}");

            if (units.Length == 0) return Node.Status.Failure;

            Vector3 center = Vector3.zero;
            foreach (var u in units)
                center += u.transform.position;
            center /= units.Length;

            if (NavMesh.SamplePosition(center, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                agent.SetDestination(hit.position);

            return Node.Status.Running;
        }


        public void Reset() { }
    }

    // --------------------------------------------------------------------------------  FLEE STRATEGY
    public class FleeStrategy : IStrategy
    {
        private readonly Transform entity;
        private readonly NavMeshAgent agent;
        private readonly LayerMask unitMask;
        private readonly float detectionRadius;
        private readonly float safeDistance;
        private readonly float moveSpeed;
        private bool destinationSet = false;

        public FleeStrategy(Transform entity, NavMeshAgent agent, LayerMask unitMask, float detectionRadius, float safeDistance = 15f, float moveSpeed = 4f)
        {
            this.entity = entity;
            this.agent = agent;
            this.unitMask = unitMask;
            this.detectionRadius = detectionRadius;
            this.safeDistance = safeDistance;
            this.moveSpeed = moveSpeed;
        }

        public Node.Status Process()
        {
            agent.speed = moveSpeed;

            if (!destinationSet)
            {
                Vector3 safePos = GetSafePosition();
                if (NavMesh.SamplePosition(safePos, out NavMeshHit hit, 5f, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                    destinationSet = true;
                }
            }

            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                destinationSet = false;
                return Node.Status.Success;
            }

            return Node.Status.Running;
        }

        private Vector3 GetSafePosition()
        {
            Collider[] units = Physics.OverlapSphere(entity.position, detectionRadius, unitMask);
            if (units.Length == 0) return entity.position;

            Vector3 center = Vector3.zero;
            foreach (var u in units)
                center += u.transform.position;
            center /= units.Length;

            Vector3 fleeDir = (entity.position - center).normalized;
            return entity.position + fleeDir * safeDistance;
        }

        public void Reset() => destinationSet = false;
    }

    // --------------------------------------------------------------------------------  DETECT FEW UNITS STRATEGY
    public class DetectFewUnitsStrategy : IStrategy
    {
        private readonly Transform entity;
        private readonly LayerMask unitMask;
        private readonly float detectionRadius;
        private readonly int threshold;

        public DetectFewUnitsStrategy(Transform entity, LayerMask unitMask, float detectionRadius, int threshold = 5)
        {
            this.entity = entity;
            this.unitMask = unitMask;
            this.detectionRadius = detectionRadius;
            this.threshold = threshold;
        }

        public Node.Status Process()
        {
            Collider[] units = Physics.OverlapSphere(entity.position, detectionRadius, unitMask.value);
            Debug.Log($"[DetectFew] count={units.Length} (threshold={threshold})");
            if (units.Length > 0 && units.Length <= threshold)
                return Node.Status.Success;
            return Node.Status.Failure;
        }


        public void Reset() { }
    }

    // --------------------------------------------------------------------------------  DETECT TOO MANY UNITS STRATEGY
    public class DetectTooManyUnitsStrategy : IStrategy
    {
        private readonly Transform entity;
        private readonly LayerMask unitMask;
        private readonly float detectionRadius;
        private readonly int threshold;

        public DetectTooManyUnitsStrategy(Transform entity, LayerMask unitMask, float detectionRadius, int threshold = 5)
        {
            this.entity = entity;
            this.unitMask = unitMask;
            this.detectionRadius = detectionRadius;
            this.threshold = threshold;
        }

        public Node.Status Process()
        {
            Collider[] units = Physics.OverlapSphere(entity.position, detectionRadius, unitMask.value);
            Debug.Log($"[DetectMany] count={units.Length} (threshold={threshold})");
            if (units.Length > threshold)
                return Node.Status.Success;
            return Node.Status.Failure;
        }

        public void Reset() { }
    }
}
