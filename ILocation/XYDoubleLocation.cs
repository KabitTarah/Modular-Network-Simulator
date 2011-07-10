using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MNS_GraphicsLib;

namespace Location
{
    public class XYDoubleLocation : ILocation
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

        bool fieldSet = false;
        public bool FieldSet
        { get { return fieldSet; } }

        public double Aspect
        {
            get
            {
                if (fieldSet)
                {
                    XYDoubleLocation[] f = (XYDoubleLocation[])field;
                    return (Math.Abs((f[1].X - f[0].X) / (f[1].Y - f[0].Y)));
                }
                else return 0;
            }
        }

        // Initially set this to ((0,0),(0,0)), an empty field.
        ILocation[] field = new XYDoubleLocation[2];
        public ILocation[] Field
        {
            get { return field; }
        }
        #endregion VARIABLES

        #region CONSTRUCTOR
        public XYDoubleLocation()
        { // A parameterless constructor is required for "Activator.CreateInstance" method.
            this.x = 0;
            this.y = 0;
        }

        public XYDoubleLocation(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        // Any non-Normal Location can be constructed using a Normal and a Field.
        public XYDoubleLocation(NormalLocation normLoc, XYDoubleLocation[] field)
        { // (Note that this constructor would be much more complex for something like Lat/Long)
            if (!SetField(field))  // First, set the field and verify it.
            {
                throw (new Exception("Invalid Field in XYDoubleLocation"));
            }
            else
            {
                // Next, measure the size of the field in the X and Y locations
                XYDoubleLocation[] tempField = (XYDoubleLocation[])this.Field;
                double dX = tempField[1].X - tempField[0].X;
                double dY = tempField[1].Y - tempField[0].Y;

                // Transform the magnitude of the Normal
                double X = dX * normLoc.X;
                double Y = dY * normLoc.Y;

                // Transform the translation of the Normal
                this.x = X + tempField[0].X;
                this.y = Y + tempField[0].Y;
            }
        }
        #endregion CONSTRUCTOR

        #region INTERFACE METHODS
        public NormalLocation Normalize()  // Creates the normalization on [(0,0),(1,1)]
        {
            // First, find the field magnitudes
            XYDoubleLocation[] field = (XYDoubleLocation[])this.Field;
            double dX = field[1].X - field[0].X;
            double dY = field[1].Y - field[0].Y;

            // If the field has not been set (or is invalid), return null
            if ((dX == 0) || (dY == 0)) return null;
            else
            { // otherwise, normalize.
                return new NormalLocation((x - field[0].X) / dX, (y - field[0].Y) / dY);
            }
        }

        public bool SetField(ILocation[] field)     // Applies a field variable to this object
        {
            if (field.Length != 2)                  // All rectangular fields have length 2.
                return false;
            else
            {
                return SetField(field[0], field[1]);    // Just defer the function... (makes it easier to call)
            }
        }

        public bool SetField(ILocation point1, ILocation point2)
        {
            XYDoubleLocation p1, p2;
            if (!((point1 is XYDoubleLocation) && (point2 is XYDoubleLocation)))
                return false;   // A field must have the same type as the ILocation object containing it!
            else
            {
                p1 = (XYDoubleLocation)point1;          // Casting
                p2 = (XYDoubleLocation)point2;          // to the interface realization

                if ((p1.X == p2.X) || (p1.Y == p2.Y))
                    return false;           // Field must have some area!

                if (p1.X > p2.X)            // p1 must be the smaller entry.
                {
                    double temp = p1.X;
                    p1.x = p2.X;
                    p2.x = temp;
                }

                if (p1.Y > p2.Y)            // p1 must be the smaller entry.
                {
                    double temp = p1.Y;
                    p1.y = p2.Y;
                    p2.y = temp;
                }

                this.field[0] = p1;
                this.field[1] = p2;
                fieldSet = true;
                return true;
            }
        }

        public bool InField()
        {
            XYDoubleLocation[] f = (XYDoubleLocation[])this.field;

            if ((f[0].X <= this.X) && (f[0].Y <= this.Y) &&
                (f[1].X >= this.X) && (f[1].Y >= this.Y))
                return true;
            else return false;
        }

        public double FieldMultiplier(double field_x)
        {
            if (!fieldSet) return 0;
            else
            {
                XYDoubleLocation p0 = (XYDoubleLocation)field[0];
                XYDoubleLocation p1 = (XYDoubleLocation)field[1];
                return (field_x / (p1.X - p0.X));
            }
        }

        public double Distance(ILocation toLocation)
        {
            if (toLocation is XYDoubleLocation)
            {
                XYDoubleLocation n = (XYDoubleLocation)toLocation;
                return Math.Sqrt(Math.Pow(n.X - x, 2) + Math.Pow(n.Y - y, 2));
            }
            else return 0.0d;
        }
        #endregion INTERFACE METHODS
    }
}
