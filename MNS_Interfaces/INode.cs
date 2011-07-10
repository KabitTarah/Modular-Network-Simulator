using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using Location;
using MNS_Reporting;

namespace ModularNetworkSimulator
{
    public interface INode : IReferrer
    {
        double NodeSize { get; }
        bool Collided { get; }  // Determines whether a collision is occurring
        ILocation Location { get; set; }
        IRandomizer RandomValue { get; set; }
        bool IsSink { get; set; }
        int ID { get; set; }
        GraphicsPath GradientPath { get; set; } // Instantiate only, Visualizer will modify accordingly
        // Private:
        // IEventManager
        // IPhysicalProcessor

        void Initialize();
        void PreTransmit();
        void BeginTransmit();
        void EndTransmit();
        void BeginReceive();
        bool EndReceive();      // true if COLLISION OCCURRED, otherwise false.

        void ForceCollision();

        void EndSimulation();   // occurs when simulation is complete. Allows the node 
                                // to send any remaining reports.

        ReportLevel GetMessageLevel(IMessage msg);
    }
}
