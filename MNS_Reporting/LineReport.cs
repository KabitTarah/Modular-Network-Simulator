using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using MNS_GraphicsLib;
using Location;

namespace MNS_Reporting
{
    public class LineReport : IGraphicalReport
    {
        bool isStatic = false;
        public bool IsStatic
        {
            get { return isStatic; }
        }

        IReport previousStatic = null;
        public IReport PreviousStatic 
        {
            get { return previousStatic; }
            set { previousStatic = value; }
        }

        Action reportAction = Action.Start;
        public Action ReportAction
        {
            get { return reportAction; }
            set { reportAction = value; }
        }

        ILocation loc1;
        ILocation loc2;
        public ILocation Loc
        {
            get { return loc1; }
            set { }
        }

        int id = -1;
        public int ID
        {
            get { return id; }
        }

        double time = 0;
        public Double Time { get { return time; } }

        DrawLayer layer = DrawLayer.Foreground;
        public DrawLayer Layer
        {
            get { return layer; }
            set { layer = value; }
        }

        double maximumMessageSize = 0;
        public double MaximumMessageSize // Required by interface (unused)  
        {
            get { return maximumMessageSize; }
            set { maximumMessageSize = value; }
        }

        bool isPointConverted = false;
        bool isDrawable = false;
        public bool IsDrawable
        {
            get { return isDrawable; }
        }

        public bool IsArrow = false;

        ColorEnum color = ColorEnum.BackgroundColor;
        public ColorEnum Color
        {
            get { return color; }
            set { color = value; }
        }
        ColorEnum finalColor = ColorEnum.BackgroundColor;
        public ColorEnum FinalColor
        {
            get { return finalColor; }
            set { finalColor = value; }
        }
        GraphicsPath gradientPath;
        public GraphicsPath GradientPath  // Not used for lines!  
        {
            get { return gradientPath; }
            set { gradientPath = value; }
        }

        public int PenWidth = 3;

        public LineReport(double time, ILocation loc1, ILocation loc2)
        {
            this.time = time;
            this.loc1 = loc1;
            this.loc2 = loc2;
        }
        public LineReport(double time, ILocation loc1, ILocation loc2, bool isStatic, int id) :
            this(time, loc1, loc2)
        {
            this.isStatic = isStatic;
            this.id = id;
            if (isStatic) layer = DrawLayer.Static;
        }

        public LineReport(double time, XYDoubleLocation nearLoc, XYDoubleLocation farLoc,
            double distance, double length)
        { // Closure must still be checked before creating the LineReport!
            double k1 = distance - length;
            double k2 = distance;
            double dx = farLoc.X - nearLoc.X;
            double dy = farLoc.Y - nearLoc.Y;

            double theta = Math.Atan2(dy, dx);

            double inner = k1 * Math.Cos(theta);
            double innerX = nearLoc.X;
            double innerY;
            double outer = k2 * Math.Cos(theta);
            double outerX = farLoc.X;
            double outerY;

            if (!double.IsInfinity(dy / dx))
            {
                double m = dy / dx;
                innerX = inner + nearLoc.X;
                innerY = m * inner + nearLoc.Y;
                outerX = outer + nearLoc.X;
                outerY = m * outer + nearLoc.Y;
            }
            else
            {
                innerY = Math.Sign(dy) * k1 + nearLoc.Y;
                outerY = Math.Sign(dy) * k2 + nearLoc.Y;
            }

            XYDoubleLocation outerLoc;
            XYDoubleLocation innerLoc = new XYDoubleLocation(innerX, innerY);
            if ((Math.Abs(outerX - nearLoc.X) > Math.Abs(dx))
                || (Math.Abs(outerY - nearLoc.Y) > Math.Abs(dy)))
                outerLoc = farLoc;
            else outerLoc = new XYDoubleLocation(outerX, outerY);
            innerLoc.SetField(nearLoc.Field);
            outerLoc.SetField(nearLoc.Field);

            if ((Math.Abs(innerX - nearLoc.X) > Math.Abs(dx))
                || (Math.Abs(innerY - nearLoc.Y) > Math.Abs(dy)))
                innerLoc = outerLoc; // This occurs if the line has already intersected the neighbor

            this.loc1 = innerLoc;
            this.loc2 = outerLoc;
            this.time = time;
        }

        public LineReport(double time, XYDoubleLocation nearLoc, XYDoubleLocation farLoc,
            double distance, double length, bool isStatic, int id) :
            this(time, nearLoc, farLoc, distance, length)
        {
            this.isStatic = isStatic;
            this.id = id;
            if (isStatic) layer = DrawLayer.Static;
        }

        public void ConvertTo(XYIntLocation[] field)  // Assume Aspect Ratio fixed  
        {
            XYIntLocation pt1 = new XYIntLocation(loc1.Normalize(), field);
            XYIntLocation pt2 = new XYIntLocation(loc2.Normalize(), field);
            loc1 = pt1;
            loc2 = pt2;

            isPointConverted = true;
            isDrawable = true;   // Normally this would occur in ConvertTo(double FieldMultiplier)
        }
        public bool ConvertTo(double FieldMultiplier) // no size values will be given in this report type  
        { return true; }

        public bool Draw(Graphics g, ColorEnumStatics colorDef)
        {
            XYIntLocation pt1, pt2;
            Pen p = new Pen(colorDef.GetColorFromEnum(color), PenWidth);
            if (IsArrow)
            {
                p.StartCap = LineCap.ArrowAnchor;
            }

            if ((loc1 is XYIntLocation) && (loc2 is XYIntLocation) && isDrawable)
            {
                pt1 = (XYIntLocation)loc1; pt2 = (XYIntLocation)loc2;
                g.DrawLine(p, pt1.X, pt1.Y, pt2.X, pt2.Y);
                return true;
            }
            else return false;
        }
    }
}
