using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ModularNetworkSimulator
{
    public interface INodes
    { // An interface describing the collection of INode objects
        Queue<INode> NodeQueue { get; }

        void AddNode(INode node);
        void InitializeNodes();
        INode GetNodeByID(int id);
        INode[] FindFurthestNodes();
        INodesIterator CreateIterator();

        void EndSimulation();
    }

    public interface INodesIterator
    { // Allows the INodes collection to be stepped through
        INode First();
        INode Next();
        bool IsDone();
        INode CurrentItem();
    }
}
