using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MNS_GraphicsLib;
using Location;

namespace ModularNetworkSimulator
{
    public class SpiralDeployer : IDeployer
    { // RandomDeployer sets a certain number of nodes randomly about the field (assuring no overlaps)

        static List<PanelObj> setupPanel()
        {
            List<PanelObj> list = new List<PanelObj>();
            PanelObj obj = new PanelObj();
            obj.name = "Archimedean Intro";
            obj.type = FormType.label;
            obj.text = "Archimedean Spiral of form r=a+bθ";
            obj.width = 200;
            obj.xSlot = 0;
            obj.ySlot = 0;
            list.Add(obj);
            
            obj = new PanelObj();
            obj.name = "aLabel";
            obj.type = FormType.label;
            obj.text = "a";
            obj.width = 58;
            obj.xSlot = 0;
            obj.ySlot = 1;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "a";
            obj.type = FormType.intBox;
            obj.value = "0";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 1;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "bLabel";
            obj.type = FormType.label;
            obj.text = "b";
            obj.width = 58;
            obj.xSlot = 0;
            obj.ySlot = 2;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "b";
            obj.type = FormType.doubleBox;
            obj.value = "80";
            obj.width = 106;
            obj.xSlot = 1;
            obj.ySlot = 2;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "NodeDistanceLabel";
            obj.type = FormType.label;
            obj.text = "Node Scroll Dist (m)";
            obj.width = 58;
            obj.xSlot = 0;
            obj.ySlot = 3;
            list.Add(obj);

            obj = new PanelObj();
            obj.name = "NodeDistance";
            obj.type = FormType.doubleBox;
            obj.value = "750";
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

        int numNodes = 25;
        double padding = 100;
        double nodeDistance = 100;
        double a=50; // Archimedean Spiral: r=a+b\theta
        double b=100; 

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
        IRandomizer rand;

        public SpiralDeployer()
        {
            isInitialized = false;
        }

        public SpiralDeployer(INodes nodes, INodeFactory nodeFactory, ILocation[] field,
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
                throw new InvalidCastException("RandomDeployer must take XYDoubleLocation field");
            panelObjsHelper = new PanelObjHelper(panelObjs);
            a = panelObjsHelper.GetIntByName("a");
            b = panelObjsHelper.GetDoubleByName("b");
            nodeDistance = panelObjsHelper.GetDoubleByName("NodeDistance");
            isInitialized = true;
        }

        public void Deploy()
        {
            if (!isInitialized)
                throw new InvalidOperationException("RandomDeployer not initialized!");

            XYDoubleLocation center, current;
            center = new XYDoubleLocation((field[0].X + field[1].X) / 2, (field[0].Y + field[1].Y) / 2);

            List<XYDoubleLocation> pointList = new List<XYDoubleLocation>();

            //rand = randFactory.CreateRandomizer();


            bool continueFlag = true;
            
            int i = 0;
            double s = 2 * Math.PI * b;  // For l=pi*s*n^2
            double n, theta, l, r, x, y;
            while (continueFlag)
            {
                l = i * nodeDistance;
                n = Math.Sqrt(l / (Math.PI * s));
                theta = 2 * Math.PI * n;

                r = a + b * theta;
                x = r * Math.Cos(theta) + center.X;
                y = r * Math.Sin(theta) + center.Y;

                current = new XYDoubleLocation(x, y);
                current.SetField(field);

                if (current.InField())
                    nodes.AddNode(nodeFactory.CreateNode(current));
                else
                    continueFlag = false;
                i++;
            }
        }
    }
}
