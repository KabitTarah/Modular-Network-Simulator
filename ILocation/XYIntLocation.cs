using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MNS_GraphicsLib;

namespace Location
{
    public class XYIntLocation : ILocation
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
        int x;
        public int X
        {
            get { return x; }
        }

        int y;
        public int Y
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

        ILocation[] field = new XYIntLocation[2];
        public ILocation[] Field
        {
            get { return field; }
        }
        #endregion VARIABLES

        #region CONSTRUCTOR
        public XYIntLocation(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public XYIntLocation(NormalLocation normLoc, XYIntLocation[] field)
        {
            if (!SetField(field))
            {
                throw (new Exception("Invalid Field in XYIntLocation"));
            }
            else
            {
                XYIntLocation[] tempField = (XYIntLocation[])this.Field;
                int dX = tempField[1].X - tempField[0].X;
                int dY = tempField[1].Y - tempField[0].Y;

                double X = dX * normLoc.X;
                double Y = dY * normLoc.Y;

                this.x = (int)(X + tempField[0].X);
                this.y = (int)(Y + tempField[0].Y);
            }
        }
        #endregion CONSTRUCTOR

        #region INTERFACE METHODS
        public NormalLocation Normalize()
        {
            XYIntLocation[] field = (XYIntLocation[])this.Field;
            double dX = field[1].X - field[0].X;
            double dY = field[1].Y - field[0].Y;

            if ((dX == 0) || (dY == 0)) return null;
            else
            {
                return new NormalLocation((x - field[0].X) / dX, (y - field[0].Y) / dY);
            }
        }

        public bool SetField(ILocation[] field)
        {
            if (field.Length != 2)
                return false;
            else
            {
                return SetField(field[0], field[1]);
            }
        }

        public bool SetField(ILocation point1, ILocation point2)
        {
            XYIntLocation p1, p2;
            if (!((point1 is XYIntLocation) && (point2 is XYIntLocation)))
                return false;
            else
            {
                p1 = (XYIntLocation)point1;
                p2 = (XYIntLocation)point2;

                if ((p1.X == p2.X) || (p1.Y == p2.Y))
                    return false;           // Field must have some area!

                if (p1.X > p2.X)            // p1 must be the smaller entry.
                {
                    int temp = p1.X;
                    p1.x = p2.X;
                    p2.x = temp;
                }

                if (p1.Y > p2.Y)            // p1 must be the smaller entry.
                {
                    int temp = p1.Y;
                    p1.y = p2.Y;
                    p2.y = temp;
                }

                this.field[0] = p1;
                this.field[1] = p2;
                this.fieldSet = true;
                return true;
            }
        }

        public bool InField()
        {
            XYIntLocation[] f = (XYIntLocation[])this.field;

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
                XYIntLocation p0 = (XYIntLocation)field[0];
                XYIntLocation p1 = (XYIntLocation)field[1];
                double width = (double)(p1.X - p0.X);
                return (field_x / width);
            }
        }

        public double Distance(ILocation toLocation)
        {
            if (toLocation is XYIntLocation)
            {
                XYIntLocation n = (XYIntLocation)toLocation;
                return Math.Sqrt(Math.Pow((double)n.X - (double)x, 2)
                    + Math.Pow((double)n.Y - (double)y, 2));
            }
            else return 0.0d;
        }
        #endregion INTERFACE METHODS
    }
}
