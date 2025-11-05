using System.Collections.Generic;

namespace Home.BehaviourTrees
{
    public class Leaf : Node
    {
        readonly IStrategy strategy;

        public Leaf(string name, IStrategy strategy) : base(name)
        {
            this.strategy = strategy;
        }

        public override Status Process() => strategy.Process();

        public override void Reset() => strategy.Reset();
    }

    public class BehaviourTree : Node
    {
        public BehaviourTree(string name) : base(name) { }


        public override Status Process()
        {
            while (currentChild < children.Count)
            {
                var status = children[currentChild].Process();

                if (status != Status.Success)
                {
                    return status;
                }
                currentChild++;
            }
            return Status.Success;
        }
    }

    public class Selector : Node
    {
        public Selector(string name) : base(name) { }

        public override Status Process()
        {
            while (currentChild < children.Count)
            {
                var status = children[currentChild].Process();

                if (status == Status.Running)
                    return Status.Running;

                if (status == Status.Success)
                    return Status.Success;

                currentChild++;
            }

            Reset();
            return Status.Failure;
        }
    }

    public class Sequence : Node
    {
        public Sequence(string name) : base(name) { }

        public override Status Process()
        {
            while (currentChild < children.Count)
            {
                var status = children[currentChild].Process();

                if (status == Status.Running)
                    return Status.Running;

                if (status == Status.Failure)
                {
                    Reset();
                    return Status.Failure;
                }

                currentChild++;
            }

            Reset();
            return Status.Success;
        }
    }



    public class Node
    {
        public enum Status
        {
            Success,
            Failure,
            Running
        }

        public readonly string name;

        public readonly List<Node> children = new List<Node>();
        protected int currentChild = 0;

        public Node(string name)
        {
            this.name = name;
        }

        public void AddChild(Node child)
        {
            children.Add(child);
        }

        public virtual Status Process()
        {
            var status = children[currentChild].Process();
            UnityEngine.Debug.Log($"Node: {name} returned {status}");
            return status;
        }


        public virtual void Reset()
        {
            currentChild = 0;
            foreach (Node child in children)
            {
                child.Reset();
            }
        }
    }
}