using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using Location;
using ModularNetworkSimulator;
using MNS_Reporting;
using MNS_GraphicsLib;

namespace PEQ
{
    public class PEQTestApplication : IApplicationEventGenerator, 
        IReportSubject
    {
        // Virtually identical to the FQN version of this class.

        #region IApplicationEventGenerator Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

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
            obj.value = "99";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 0;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "EventTimeLabel";
            obj.type = FormType.label;
            obj.text = "Event Start Time (s)";
            obj.width = 58;
            obj.xSlot = 0;
            obj.ySlot = 1;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "EventMeanTime";
            obj.type = FormType.doubleBox;
            obj.value = "30";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 1;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "EventFreqLabel";
            obj.type = FormType.label;
            obj.text = "Event Freq (Hz)";
            obj.width = 58;
            obj.xSlot = 0;
            obj.ySlot = 2;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "EventFreq";
            obj.type = FormType.doubleBox;
            obj.value = "0.1";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 2;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "NumOccurancesLabel";
            obj.type = FormType.label;
            obj.text = "Number of Events";
            obj.width = 58;
            obj.xSlot = 0;
            obj.ySlot = 3;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "NumOccurances";
            obj.type = FormType.intBox;
            obj.value = "10";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 3;
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
            set
            {
                if (value is XYDoubleLocation[])
                    field = (XYDoubleLocation[])value;
                else field = new XYDoubleLocation[2];
            }
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IApplicationEventGenerator Variables

        #region PEQRandomDetectionEventGenerator Variables
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

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

        double eventFreq;
        int numOccurances;

        double _dataID = 0;

        bool finalEvent = true;

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion PEQRandomDetectionEventGenerator Variables

        #region PEQRandomDetectionEventGenerator Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

        public PEQTestApplication()
        {
            isInitialized = false;
        }

        public PEQTestApplication(IEventManager eventMgr, INodes nodes,
            IRandomizer randomizer, ILocation[] field)
        {
            Initialize(eventMgr, nodes, randomizer, field);
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion PEQRandomDetectionEventGenerator Methods

        #region IApplicationEventGenerator Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

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
            this.eventFreq = panelObjsHelper.GetDoubleByName("EventFreq");
            this.numOccurances = panelObjsHelper.GetIntByName("NumOccurances");
            //this.finalEvent = panelObjsHelper.GetBoolByName("FinalEvent");
            isInitialized = true;
        }

        public void GenerateEvent()
        { // Rather than setting a random time, in this case the event is simply generated when specified.
            XYDoubleLocation initalCorner = (XYDoubleLocation)field[0];  // Casting to xyLocation objects. Other
            XYDoubleLocation finalCorner = (XYDoubleLocation)field[1];     // coordinate systems are not supported.

            XYDoubleLocation location;

            for (int i = 0; i < 5; i++)
            {
                location = new XYDoubleLocation(initalCorner.X,
                    i*(finalCorner.Y-initalCorner.Y)/4);

                GenerateEvent(location);
            }
        }

        public void GenerateEvent(ILocation location)
        {
            if (location is XYDoubleLocation)
                GenerateEvent(eventMeanTime, location);
            else
                GenerateEvent();
        }

        public void GenerateEvent(double time, ILocation location)
        { // Overloads GenerateEvent to allow the simulation to provide the time the event occurs.
            XYDoubleLocation initalCorner = (XYDoubleLocation)field[0];  // Casting to xyLocation objects. Other
            XYDoubleLocation finalCorner = (XYDoubleLocation)field[1];     // coordinate systems are not supported.

            List<PEQNode> nodesInRange = new List<PEQNode>();

            // Generate the random center location of the event.
            eventCenter = (XYDoubleLocation)location;

            eventCenter.SetField(field);

            Notify(generateStaticReport(time, eventCenter));

            foreach (PEQNode node in nodes.NodeQueue)
            { // Find which nodes are within the effective detection area.
                if (eventCenter.Distance(node.Location) <= eventSize) // if in the event area
                    nodesInRange.Add(node);
            }
            
            double eventTime = 0;
            for (int i = 0; i < this.numOccurances; i++)
            {
                eventTime = time + i / this.eventFreq;

                foreach (PEQNode node in nodesInRange)
                {
                    PEQMessageApplication appMsg = new PEQMessageApplication(); // Create a new app message
                    appMsg._Data._DataID = _dataID++;
                    MessageEvent msgEvent = new MessageEvent(appMsg); // create a new message event
                    msgEvent.Referrer = node; // set the referrer to the current node

                    PEQDataInfoReport rep = new PEQDataInfoReport(node.ID, eventTime);
                    rep._Sent = 1;
                    rep._DataID = appMsg._Data._DataID;
                    Notify(rep);

                    PEQTimerInternal messageTimer = new PEQTimerInternal(appMsg, eventTime, node);
                    eventMgr.AddEvent(messageTimer);   // add the event to the Event Queue.
                }
            }

            SimulationCompleteEvent simCompleteEvent = new SimulationCompleteEvent(nodes);
            simCompleteEvent.Time = eventTime + 60; // one minute after last event occurs
            eventMgr.AddEvent(simCompleteEvent);
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IApplicationEventGenerator Methods

        #region IReportSubject Methods
        /*       / \
         *     // | \\
         *    /   |   \
         *        |           */

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
            
            CircleReport circReport = new CircleReport(time, eventCenter, eventSize * 2,
                gradientPath, true, -1);
            circReport.Color = ColorEnum.FlashOverride;
            circReport.FinalColor = ColorEnum.Transparent;

            circReport.Layer = DrawLayer.Background;

            return circReport;
        }

        /*        |
         *    \   |   /
         *     \\ | //
         *       \ /          */
        #endregion IReportSubject Methods
    }
}
