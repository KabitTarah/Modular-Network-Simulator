using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriorityQueue;

namespace ModularNetworkSimulator
{
    public class SimulationCompleteEvent : IEvent
    { // Used by the EventManager to determine when the simulation should end.   
      // This event can be enqueued by a process (e.g. Sink Node receives an event)
      // or initially based on a certain run-time.
        #region VARIABLES (SET/GET)
        double time;
        public double Time
        {
            get { return time; }
            set { time = value; }
        }

        double duration = 0;
        public double Duration
        {
            get { return duration; }
            set { duration = 0; }
        }

        public bool SuppressReport
        {
            get { return false; }
            set { }
        }

        INodes nodes;
        public INodes Nodes
        {
            get { return nodes; }
            set { nodes = value; }
        }

        #endregion VARIABLES

        #region METHODS
        public SimulationCompleteEvent(INodes Nodes)
        {
            nodes = Nodes;
        }

        public void Execute()
        {
            nodes.EndSimulation();
        }

        public object Clone()
        {
            return null;        // Never clone this IEvent!
        }
        #endregion METHODS
    }

    public class EventManager : IEventManager
    { // The base implementation of the Event Manager. This simply enqueues and dequeues IEvent objects
      // (see: Command Pattern) and invokes them.
        #region VARIABLES (GET/SET)
        PriorityQueue<IEvent, double> EventQueue = new PriorityQueue<IEvent, double>();

        double clock = 0;
        public double CurrentClock
        {
            get { return clock; }
        }
        #endregion VARIABLES

        #region METHODS
        public void AddEvent(IEvent e)
        {
            // Note that time is negated because the priority queue takes the highest value as preferred.
            EventQueue.Enqueue(e, -e.Time);
        }

        public void StartSimulation()
        {
            bool SimulationComplete = false;    // flag for loop to trigger on
            PriorityQueueItem<IEvent, double> EventQueueItem; // Encapsulation of a single item in the queue
            IEvent currentEvent;

            while (!SimulationComplete)
            {
                if (this.EventQueue.Count > 0) // If not... there's a problem!
                {
                    EventQueueItem = this.EventQueue.Dequeue(); // Pull the next item from the Queue

                    currentEvent = EventQueueItem.Value; // Remove the PriorityQueueItem encapsulation
                    if (currentEvent is SimulationCompleteEvent)  // trigger on event type
                    { // Simulation has finished. End. (Even if events remain in the Queue as this will be common).
                        SimulationComplete = true;
                        currentEvent.Execute();    // allow for wrap-up work in this event
                    }
                    else
                    { // Otherwise... continue
                        clock = -EventQueueItem.Priority;   // set the current clock to the dequeued item.
                        currentEvent.Execute();             // Invoke the IEvent object (Command Pattern)
                    }
                    // Model invokation was here. Now listening for status will be used instead.
                }
                else 
                {
                    SimulationComplete = true;
                    throw new Exception("Simulation Queue Empty!!");
                }
            }
        }
        #endregion METHODS
    }
}
