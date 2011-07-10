using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MNS_GraphicsLib;

namespace Location
{
    public class NormalLocation : ILocation
    {
        static List<PanelObj> setupPanel()
        {
            List<PanelObj> list = new List<PanelObj>();

            return list;
        }
        public List<PanelObj> SetupPanel
        { get { return setupPanel(); } }

        public List<PanelObj> panelObjs;
        public List<PanelObj> PanelObjs
        {
            get { return panelObjs; }
            set { PanelObjs = value; }
        }

        #region VARIABLES
        double x;
        public double X
        {
            get { return x; }
        }

        double y;
        public double Y
        {
            get { return y; }
        }

        public bool FieldSet
        { get { return true; } }    // The Field is always set ... =[(0,0),(1,1)]

        public double Aspect
        { get { return 1.0; } }     // The Aspect ratio is always 1.

        // Field is constant for NormalLocation & set in constructor
        ILocation[] field = new NormalLocation[2];
        public ILocation[] Field
        {
            get { return field; }
        }
        #endregion VARIABLES

        #region CONSTRUCTOR
        public NormalLocation(double x, double y)
        {
            this.x = x;
            this.y = y;
            field[0] = new NormalLocation(0, 0, true);  // To avoid loops, the field ILocations
            field[1] = new NormalLocation(1, 1, true);  // have an alternate constructor.
        }

        private NormalLocation(double x, double y, bool isField)
        { // Alternate constructor if isField = true
            this.x = x;
            this.y = y;
        }
        #endregion CONSTRUCTOR

        #region INTERFACE METHODS
        public NormalLocation Normalize() { return this; } // by definition, this object is already Normal.

        public bool SetField(ILocation[] field)
        { return false; }                                  // The Normal Field is always [(0,0),(1,1)]

        public bool SetField(ILocation point1, ILocation point2)
        { return false; }                                  // The Normal Field is always [(0,0),(1,1)]

        public bool InField()
        {
            if (!((x >= 0) && (x <= 1) && (y >= 0) && (y <= 1)))  // Point within Normal Field?
                return true;
            else return false;
        }

        public double FieldMultiplier(double field_x)
        {
            return 0; // not applicable to NormalLocation
        }

        public double Distance(ILocation toLocation)
        {
            if (toLocation is NormalLocation)
            {
                NormalLocation n = (NormalLocation)toLocation;
                return Math.Sqrt(Math.Pow(n.X - x,2) + Math.Pow(n.Y - y,2));
            }
            else return 0.0d;
        }
        #endregion INTERFACE METHODS
    }
}
