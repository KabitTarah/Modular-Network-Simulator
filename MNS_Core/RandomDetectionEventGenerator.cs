using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using MNS_GraphicsLib;
using MNS_Reporting;
using Location;

namespace ModularNetworkSimulator
{
    public class NoEvent : IApplicationEventGenerator, IReportSubject
    {
        static List<PanelObj> setupPanel()
        {
            List<PanelObj> list = new List<PanelObj>();
            PanelObj obj = new PanelObj();
            obj.name = "DescLabel";
            obj.type = FormType.label;
            obj.text = "No Application / Detection will occur.";
            obj.width = 106;
            obj.xSlot = 0;
            obj.ySlot = 0;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "LabelEndTime";
            obj.type = FormType.label;
            obj.text = "End Event Time (s)";
            obj.width = 106;
            obj.xSlot = 0;
            obj.ySlot = 1;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "EndTime";
            obj.type = FormType.doubleBox;
            obj.value = "300";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 1;
            list.Add(obj);

            return list;
        }
        public List<PanelObj> SetupPanel
        { get { return setupPanel(); } }

        public List<PanelObj> panelObjs;
        public List<PanelObj> PanelObjs
        {
            get { return panelObjs; }
            set { panelObjs = value; }
        }

        PanelObjHelper panelObjsHelper;

        bool isInitialized;
        public bool IsInitialized
        {
            get { return true; }
        }

        IEventManager eventMgr;
        public IEventManager EventMgr
        {
            set { eventMgr = value; }
        }

        INodes nodes;
        public INodes Nodes
        {
            set { nodes = value; }
        }

        IRandomizer randomizer;
        public IRandomizer Randomizer
        {
            set { randomizer = value; }
        }

        XYDoubleLocation[] field = new XYDoubleLocation[2];
        public ILocation[] Field
        {
            get { return field; }
            set
            {
                if (value is XYDoubleLocation[])
                    field = (XYDoubleLocation[])value;
                else field = new XYDoubleLocation[2];
            }
        }

        double endTime = 0;

        public NoEvent()
        { }

        public void Initialize(IEventManager eventMgr, INodes nodes,
            IRandomizer randomizer, ILocation[] field)
        {
            panelObjsHelper = new PanelObjHelper(panelObjs);
            this.eventMgr = eventMgr;
            this.nodes = nodes;
            this.randomizer = randomizer;
            this.field = (XYDoubleLocation[])field;

            this.endTime = panelObjsHelper.GetDoubleByName("EndTime");
        }

        public void GenerateEvent()
        {
            SimulationCompleteEvent simCompleteEvent = new SimulationCompleteEvent(nodes);
            simCompleteEvent.Time = endTime;
            eventMgr.AddEvent(simCompleteEvent);
        }

        public void GenerateEvent(ILocation location)
        {
            GenerateEvent();
        }

        #region IReportSubject Interface Methods
        ArrayList observers = new ArrayList();

        public void Attach(IReportObserver observer)
        {
            if (observers.Contains(observer))
                return;
            else observers.Add(observer);
        }

        public void Detach(IReportObserver observer)
        {
            if (observers.Contains(observer))
                observers.Remove(observer);
        }

        public void Notify(IReport report)
        { }

        #endregion
    }

    public class FQN_RandomDetectionEventGenerator : IApplicationEventGenerator, IReportSubject
    { // This module implements the Application Event Generator by generating a random event somewhere
        // in the field with the provided effective radius. This is then passed to each node that exists
        // within the affected area.
        static List<PanelObj> setupPanel()
        {
            List<PanelObj> list = new List<PanelObj>();
            PanelObj obj = new PanelObj();
            obj.name = "EventSizeLabel";
            obj.type = FormType.label;
            obj.text = "Event Size";
            obj.width = 58;
            obj.xSlot = 0;
            obj.ySlot = 0;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "EventSize";
            obj.type = FormType.doubleBox;
            obj.value = "750";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 0;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "EventTimeLabel";
            obj.type = FormType.label;
            obj.text = "Event μ Time (s)";
            obj.width = 58;
            obj.xSlot = 0;
            obj.ySlot = 1;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "EventMeanTime";
            obj.type = FormType.doubleBox;
            obj.value = "15";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 1;
            list.Add(obj);

            return list;
        }
        public List<PanelObj> SetupPanel
        { get { return setupPanel(); } }

        public List<PanelObj> panelObjs;
        public List<PanelObj> PanelObjs
        { 
            get { return panelObjs;} 
            set { panelObjs = value;}
        }
        PanelObjHelper panelObjsHelper;

        bool isInitialized;
        public bool IsInitialized
        {
            get { return isInitialized; }
        }

        IEventManager eventMgr;
        public IEventManager EventMgr
        {
            set { eventMgr = value; }
        }

        INodes nodes;
        public INodes Nodes
        {
            set { nodes = value; }
        }

        IRandomizer randomizer;
        public IRandomizer Randomizer
        {
            set { randomizer = value; }
        }

        XYDoubleLocation[] field = new XYDoubleLocation[2];
        public ILocation[] Field
        {
            get { return field; }
            set {
                if (value is XYDoubleLocation[])
                    field = (XYDoubleLocation[])value;
                else field = new XYDoubleLocation[2];
            }
        }

        double eventSize;
        public double EventSize
        {
            get { return eventSize; }
            set { eventSize = value; }
        }

        XYDoubleLocation eventCenter;
        public XYDoubleLocation EventCenter
        {
            get { return eventCenter; }
        }

        double eventMeanTime;

        public FQN_RandomDetectionEventGenerator()
        {
            isInitialized = false;
        }

        public FQN_RandomDetectionEventGenerator(IEventManager eventMgr, INodes nodes,
            IRandomizer randomizer, ILocation[] field)
        {
            Initialize(eventMgr, nodes, randomizer, field);
        }

        public void Initialize(IEventManager eventMgr, INodes nodes,
            IRandomizer randomizer, ILocation[] field)
        {
            this.eventMgr = eventMgr;
            this.nodes = nodes;
            this.randomizer = randomizer;
            if (field is XYDoubleLocation[])
                this.field = (XYDoubleLocation[])field;
            else
            {
                isInitialized = false;
                return;
            }
            panelObjsHelper = new PanelObjHelper(panelObjs);
            this.eventSize = panelObjsHelper.GetDoubleByName("EventSize");
            this.eventMeanTime = panelObjsHelper.GetDoubleByName("EventMeanTime");
            isInitialized = true;
        }

        public void GenerateEvent()
        { // Rather than setting a random time, in this case the event is simply generated at 15 seconds.
            XYDoubleLocation initalCorner = (XYDoubleLocation)field[0];  // Casting to xyLocation objects. Other
            XYDoubleLocation finalCorner = (XYDoubleLocation)field[1];     // coordinate systems are not supported.


            XYDoubleLocation location = new XYDoubleLocation((float)(randomizer.NextDouble() * (finalCorner.X - initalCorner.X) + initalCorner.X),
                (float)(randomizer.NextDouble() * (finalCorner.Y - initalCorner.Y) + initalCorner.Y));
            GenerateEvent(location);

            SimulationCompleteEvent simCompleteEvent = new SimulationCompleteEvent(nodes);
            simCompleteEvent.Time = eventMeanTime + 60; // one minute after event occurs
            eventMgr.AddEvent(simCompleteEvent);
        }

        public void GenerateEvent(double time, ILocation location)
        { // Overloads GenerateEvent to allow the simulation to provide the time the event occurs.
            XYDoubleLocation initalCorner = (XYDoubleLocation)field[0];  // Casting to xyLocation objects. Other
            XYDoubleLocation finalCorner = (XYDoubleLocation)field[1];     // coordinate systems are not supported.

            // Generate the random center location of the event.
            eventCenter = (XYDoubleLocation)location;

            eventCenter.SetField(field);

            Notify(generateStaticReport(time, eventCenter));

            foreach (FloodingQueryNode node in nodes.NodeQueue)
            { // Find which nodes are within the effective detection area.
                if (eventCenter.Distance(node.Location) <= eventSize) // if in the event area
                {
                    FloodingQueryApplicationMessage appMsg = new FloodingQueryApplicationMessage(); // Create a new app message
                    MessageEvent msgEvent = new MessageEvent(appMsg); // create a new message event
                    msgEvent.Referrer = node; // set the referrer to the current node

                    NodeTimerEvent timerEvent = new NodeTimerEvent();   // create a timer event
                    timerEvent.Time = time;
                    timerEvent.Event = msgEvent;                        // apply the message event
                    timerEvent.node = node;
                    eventMgr.AddEvent(timerEvent);                      // add the event to the Event Queue.
                }
            }
        }

        public void GenerateEvent(ILocation location)
        {
            if (location is XYDoubleLocation)
                GenerateEvent(eventMeanTime, location);
            else GenerateEvent();
        }

        #region IReportSubject Interface Methods
        ArrayList observers = new ArrayList();

        public void Attach(IReportObserver observer)
        {
            if (observers.Contains(observer))
                return;
            else observers.Add(observer);
        }

        public void Detach(IReportObserver observer)
        {
            if (observers.Contains(observer))
                observers.Remove(observer);
        }

        public void Notify(IReport report)
        {
            IReportObserver observer;
            foreach (Object obj in observers)
            {
                observer = (IReportObserver)obj;
                observer.Update(report);
            }
        }

        CircleReport generateStaticReport(double time, XYDoubleLocation eventCenter)
        {
            GraphicsPath gradientPath = new GraphicsPath();
            gradientPath.AddEllipse((float)eventCenter.X, (float)eventCenter.Y, 
                (float)(eventSize), (float)(eventSize));

            CircleReport circReport = new CircleReport(time, eventCenter, eventSize*2, 
                gradientPath, true, -1);
            circReport.Color = ColorEnum.FlashOverride;
            circReport.FinalColor = ColorEnum.Transparent;

            circReport.Layer = DrawLayer.Background;

            return circReport;
        }
        #endregion
    }
}
