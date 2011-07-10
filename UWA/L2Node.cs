using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriorityQueue;

namespace UWA
{
    class L2Node
    {
        public int ID;
        int currentID;
        double currentEndTime;
        double currentBaseReceiveLevel;
        bool isReceiving = false;

        // if NoiseLevel > ReceiveLevel, collision occurs.
        public SPL ReceiveLevel = 0;
        public SPL NoiseLevel = 0;
        public SPL MinReceiveLevel;

        PriorityQueue<L2NodeEntry, double> SPL_Events = new PriorityQueue<L2NodeEntry, double>();

        public L2Node(int nodeID, double minReceiveLevel) : this(nodeID, minReceiveLevel, 0) 
        { }

        public L2Node(int nodeID, double minReceiveLevel, double baseNoiseLevel)
        {
            ID = nodeID;
            MinReceiveLevel = minReceiveLevel;
            NoiseLevel = baseNoiseLevel;
        }

        public void StartEvent(int sendingNodeID, double BaseReceiveLevel, double time, double duration)
        {
            bool thisCollided = false;
            bool prevCollided = false;
            L2NodeEntry l2Entry;

            if (BaseReceiveLevel > NoiseLevel)
            {
                ReceiveLevel = (SPL)BaseReceiveLevel - NoiseLevel;
                NoiseLevel = NoiseLevel + (SPL)BaseReceiveLevel;
                if (isReceiving)
                {
                    prevCollided = true;
                    l2Entry = new L2NodeEntry(currentID, double.NegativeInfinity, prevCollided);
                    SPL_Events.Enqueue(l2Entry, -currentEndTime);
                }

                currentID = sendingNodeID;
                currentEndTime = time + duration;
            }
            else
            {
                ReceiveLevel = ReceiveLevel - (SPL)BaseReceiveLevel;
                NoiseLevel = NoiseLevel + (SPL)BaseReceiveLevel;
            }
            thisCollided = (ReceiveLevel > MinReceiveLevel);
            l2Entry = new L2NodeEntry(sendingNodeID, BaseReceiveLevel, thisCollided);
            SPL_Events.Enqueue(l2Entry, -(time + duration));
        }

        public bool EndEvent(int sendingNodeID, double time)
        { // Returns true if collision occurred

            PriorityQueue<L2NodeEntry, double> tempEvents = new PriorityQueue<L2NodeEntry, double>();
            L2NodeEntry currentEntry;
            double currentTime;
            PriorityQueueItem<L2NodeEntry, double> item;
            bool collided = false;

            while ((SPL_Events.Count > 0) && (SPL_Events.Peek().Priority < -time))
            {
                item = SPL_Events.Dequeue();
                currentTime = -item.Priority;
                currentEntry = item.Value;

                if ((currentEntry.id == sendingNodeID) & (currentEntry.Collision))
                    collided = true;

                ReceiveLevel = ReceiveLevel + currentEntry.BaseReceiveLevel;
                NoiseLevel = NoiseLevel - currentEntry.BaseReceiveLevel;
            }

            while ((SPL_Events.Count > 0) && (SPL_Events.Peek().Priority == time))
            {
                item = SPL_Events.Dequeue();
                if (item.Value.id != sendingNodeID)
                    tempEvents.Enqueue(item);
                else
                {
                    currentEntry = item.Value;

                    if (currentEntry.Collision)
                        collided = true;

                    if (currentID == sendingNodeID)
                    {
                        ReceiveLevel = 0;
                    }
                    else if (ReceiveLevel != 0)
                    {
                        ReceiveLevel = ReceiveLevel + currentEntry.BaseReceiveLevel;
                    }
                    NoiseLevel = NoiseLevel - currentEntry.BaseReceiveLevel;
                }
            }

            while (tempEvents.Count > 0)
            {
                SPL_Events.Enqueue((PriorityQueueItem<L2NodeEntry, double>)tempEvents.Dequeue());
            }

            return collided;
        }
    }

    class L2NodeEntry
    {
        public int id;
        public SPL BaseReceiveLevel = 0;
        public bool Collision = false;

        public L2NodeEntry(int id, SPL bRL, bool collided)
        {
            this.id = id;
            this.BaseReceiveLevel = bRL;
            Collision = collided;
        }
    }

    class L2NodesIterator
    { // A concrete iterator class over the concrete Nodes class
        #region VARIABLES
        Queue<L2Node> l2nodes;
        int index;
        #endregion VARIABLES

        #region CONSTRUCTORS
        public L2NodesIterator(L2Nodes l2nodes)
        {
            index = 0;
            this.l2nodes = l2nodes.NodeQueue;
        }
        #endregion CONSTRUCTORS

        public L2Node First()
        {
            if (l2nodes.Count > 0)
                return l2nodes.First();
            else return null;
        }

        public L2Node Next()
        {
            if (IsDone())
                return null;
            else
                return l2nodes.ElementAt<L2Node>(index++);
        }

        public bool IsDone()
        {
            if (l2nodes.Count <= index)
                return true;
            else return false;
        }

        public L2Node CurrentItem()
        {
            if (IsDone())
                return null;
            else
                return l2nodes.ElementAt<L2Node>(index);
        }
    }

    class L2Nodes
    {
        Queue<L2Node> l2nodes = new Queue<L2Node>();
        public Queue<L2Node> NodeQueue
        {
            get { return l2nodes; }
        }

        public L2Nodes()
        { }

        public void AddL2Node(L2Node l2node)
        {
            this.l2nodes.Enqueue(l2node);
        }

        public L2Node GetNodeByID(int id)
        {
            L2NodesIterator iterator = this.CreateIterator();
            L2Node l2node = null;
            bool finished = false;
            while (!iterator.IsDone() && !finished)
            {
                l2node = iterator.Next();
                if (l2node.ID == id)
                    finished = true;
            }
            return l2node;
        }

        public L2NodesIterator CreateIterator()
        {
            return new L2NodesIterator(this);
        }
    }
}
