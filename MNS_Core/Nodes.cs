using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModularNetworkSimulator
{
    public class NodesIterator : INodesIterator
    { // A concrete iterator class over the concrete Nodes class
        #region VARIABLES
        Queue<INode> nodes;
        int index;
        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        #endregion VARIABLES

        #region CONSTRUCTORS
        public NodesIterator(INodes nodes)
        {
            index = 0;
            this.nodes = nodes.NodeQueue;
        }
        #endregion CONSTRUCTORS

        #region INodesIterator_METHODS
        public INode First()
        {
            if (nodes.Count > 0)
                return nodes.First();
            else return null;
        }

        public INode Next()
        {
            if (IsDone())
                return null;
            else
                return nodes.ElementAt<INode>(index++);
        }

        public bool IsDone()
        {
            if (nodes.Count <= index)
                return true;
            else return false;
        }

        public INode CurrentItem()
        {
            if (IsDone())
                return null;
            else
                return nodes.ElementAt<INode>(index);
        }
        #endregion INodesIterator_METHODS
    }

    public class Nodes : INodes
    { // Concrete Nodes class. (Note that the Nodes class does not care what type of INode class is being used).
        Queue<INode> nodes = new Queue<INode>();
        public Queue<INode> NodeQueue
        {
            get { return nodes; }
        }

        IRandomizer randomizer;

        public Nodes(IRandomizer randomizer)
        {
            this.randomizer = randomizer;
        }

        public void AddNode(INode node)
        {
            this.nodes.Enqueue(node);
        }

        public void InitializeNodes()
        {
            INodesIterator iterator = this.CreateIterator();
            while (!iterator.IsDone())
            {
                iterator.Next().Initialize();
            }
        }

        public INode GetNodeByID(int id)
        {
            INodesIterator iterator = this.CreateIterator();
            INode node = null;
            bool finished = false;
            while (!iterator.IsDone() && !finished)
            {
                node = iterator.Next();
                if (node.ID == id)
                    finished = true;
            }
            return node;
        }

        public INodesIterator CreateIterator()
        {
            return new NodesIterator(this);
        }

        public INode[] FindFurthestNodes()
        {
            INode[] pair = new INode[2];
            NodesIterator iterator = new NodesIterator(this);
            NodesIterator iterator2 = new NodesIterator(this);
            double distance = 0;
            double currentDist = 0;

            while (!iterator.IsDone())
            {
                INode node1 = iterator.Next();
                iterator2.Index = iterator.Index;
                while (!iterator2.IsDone())
                {
                    INode node2 = iterator2.Next();
                    currentDist = node1.Location.Distance(node2.Location);

                    if (currentDist > distance)
                    {
                        distance = currentDist;
                        pair[0] = node1;
                        pair[1] = node2;
                    }
                }
            }

            return pair;
        }

        public void SetRandomSinkNode()
        { // Chooses one node at random to be the Sink
            int sinkID = this.randomizer.Next(0, this.nodes.Count - 1);

            INode node = this.nodes.ElementAt<INode>(sinkID);
            node.IsSink = true;
        }

        public void EndSimulation()
        {
            INodesIterator iterator = this.CreateIterator();
            while (!iterator.IsDone())
            {
                iterator.Next().EndSimulation();
            }
        }
    }
}
