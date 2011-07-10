using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MNS_GraphicsLib;
using Location;

namespace ModularNetworkSimulator
{
    public class GridDeployer : IDeployer
    { // GridDeployer sets each node 1 km from a neighboring node in either the x or y direction.

        static List<PanelObj> setupPanel()
        {
            List<PanelObj> list = new List<PanelObj>();
            PanelObj obj = new PanelObj();
            obj.name = "NodeDistanceLabel";
            obj.type = FormType.label;
            obj.text = "Node Distance (m)";
            obj.width = 58;
            obj.xSlot = 0;
            obj.ySlot = 0;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "NodeDistance";
            obj.type = FormType.doubleBox;
            obj.value = "1000";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 0;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "PaddingLabel";
            obj.type = FormType.label;
            obj.text = "Padding (m)";
            obj.width = 58;
            obj.xSlot = 0;
            obj.ySlot = 1;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "Padding";
            obj.type = FormType.doubleBox;
            obj.value = "100";
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

        double nodeDistance = 1000;
        double padding = 100;

        bool isInitialized;
        public bool IsInitialized
        {
            get { return isInitialized; }
        }

        INodes nodes;
        public INodes Nodes
        {
            get { return nodes; }
        }

        INodeFactory nodeFactory;
        public INodeFactory NodeFactory
        {
            get { return nodeFactory; }
        }

        XYDoubleLocation[] field = new XYDoubleLocation[2];
        public ILocation[] Field
        {
            get { return field; }
        }

        IRandomizerFactory randFactory;
        public IRandomizerFactory RandFactory { set { randFactory = value; } }

        public GridDeployer()
        {
            isInitialized = false;
        }

        public GridDeployer(INodes nodes, INodeFactory nodeFactory, ILocation[] field,
            IRandomizerFactory randFactory)
        {
            Initialize(nodes, nodeFactory, field, randFactory);
        }

        public void Initialize(INodes nodes, INodeFactory nodeFactory, ILocation[] field,
            IRandomizerFactory randFactory)
        {
            this.nodes = nodes;
            this.nodeFactory = nodeFactory;
            this.randFactory = randFactory;
            if (field is XYDoubleLocation[])
                this.field = (XYDoubleLocation[])field;
            else
                throw new InvalidCastException("GridDeployer must take XYDoubleLocation field");
            panelObjsHelper = new PanelObjHelper(panelObjs);
            nodeDistance = panelObjsHelper.GetDoubleByName("NodeDistance");
            padding = panelObjsHelper.GetDoubleByName("Padding");
            isInitialized = true;
        }

        public void Deploy()
        {
            if (!isInitialized)
                throw new InvalidOperationException("GridDeployer not initialized!");

            double x, y;
            XYDoubleLocation initial, final, current;
            initial = (XYDoubleLocation)field[0];
            final = (XYDoubleLocation)field[1];

            for (x = initial.X + padding; x <= final.X - padding; x += nodeDistance)
                for (y = initial.Y + padding; y <= final.Y - padding; y += nodeDistance)
                {
                    current = new XYDoubleLocation(x, y);
                    current.SetField(field);
                    nodes.AddNode(nodeFactory.CreateNode(current));  // Creates a node using the factory and adds it to the collection.
                }
        }
    }
}
