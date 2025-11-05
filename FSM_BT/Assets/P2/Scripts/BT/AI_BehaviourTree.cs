using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Home.BehaviourTrees;

public class AI_BehaviourTree : MonoBehaviour
{
    public List<Transform> patrolPoints = new List<Transform>();
    public LayerMask unitMask;
    public float detectionRadius = 10f;

    private NavMeshAgent agent;
    private BehaviourTree rootTree;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        BuildTree();
    }

    private void Update()
    {
        if (rootTree == null) return;

        var status = rootTree.Process();

        if (status != Home.BehaviourTrees.Node.Status.Running)
            rootTree.Reset();
        else
            rootTree.Reset(); // always reset to re-check conditions every frame
    }



    private void BuildTree()
    {
        rootTree = new BehaviourTree("AI Root");

        // Create Nodes
        var patrolNode = new Leaf("Patrol", new PatrolStrategy(transform, agent, patrolPoints, 2f));
        var fewUnitsCheck = new Leaf("Detect Few Units", new DetectFewUnitsStrategy(transform, unitMask, detectionRadius));
        var manyUnitsCheck = new Leaf("Detect Too Many Units", new DetectTooManyUnitsStrategy(transform, unitMask, detectionRadius));
        var approachNode = new Leaf("Approach Group", new ApproachGroupStrategy(transform, agent, unitMask, detectionRadius, 3.5f));
        var fleeNode = new Leaf("Flee Area", new FleeStrategy(transform, agent, unitMask, detectionRadius, 15f, 4f));

        // Sequences
        var fleeSequence = new Sequence("Flee Sequence");
        fleeSequence.AddChild(manyUnitsCheck);
        fleeSequence.AddChild(fleeNode);

        var approachSequence = new Sequence("Approach Sequence");
        approachSequence.AddChild(fewUnitsCheck);
        approachSequence.AddChild(approachNode);

        // Priority = flee > approach > patrol
        var rootSelector = new Selector("Decision Selector");
        rootSelector.AddChild(fleeSequence);
        rootSelector.AddChild(approachSequence);
        rootSelector.AddChild(patrolNode);

        rootTree.AddChild(rootSelector);
    }



}
