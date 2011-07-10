using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using Location;
using MNS_GraphicsLib;

namespace MNS_Reporting
{
    public class CircleReport : IGraphicalReport
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
        public ILocation Loc
        {
            get { return loc1; }
            set { loc1 = value; }
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

        double size;
        public double Size
        { get { return size; } }

        double minimumSize = 10;
        public double MinimumSize
        {
            get { return minimumSize; }
            set { minimumSize = value; }
        }

        double maximumMessageSize = 0;
        public double MaximumMessageSize
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
        public GraphicsPath GradientPath
        {
            get { return gradientPath; }
            set { gradientPath = value; }
        }

        public CircleReport(double time, ILocation loc, double size, GraphicsPath gradientPath)
        {
            this.time = time;
            loc1 = loc;
            this.size = size;
            this.gradientPath = gradientPath;
        }

        public CircleReport(double time, ILocation loc, double size, GraphicsPath gradientPath,
            bool isStatic, int id)
        {
            this.time = time;
            loc1 = loc;
            this.size = size;
            this.gradientPath = gradientPath;

            this.isStatic = isStatic;
            this.id = id;
            if (isStatic) layer = DrawLayer.Static;
        }

        public void ConvertTo(XYIntLocation[] field)  // Assume Aspect Ratio fixed  
        {
            XYIntLocation pt1 = new XYIntLocation(loc1.Normalize(), field);
            loc1 = pt1;

            isPointConverted = true;
        }

        public bool ConvertTo(double FieldMultiplier)
        {
            if (isPointConverted && !isDrawable)
            {
                size = (float)(size * FieldMultiplier);
                maximumMessageSize = (float)(maximumMessageSize * FieldMultiplier);
                isDrawable = true;
                return true;
            }
            else return false;  // Location must be converted first!
        }

        public bool Draw(Graphics g, ColorEnumStatics colorDef)
        {
            SolidBrush b = new SolidBrush(colorDef.GetColorFromEnum(color));

            double size;  // Force circle size to meet minimum requirement
            if (this.size < this.minimumSize)
                size = this.minimumSize;
            else
                size = this.size;

            XYIntLocation pt1;
            if ((loc1 is XYIntLocation) && isDrawable)
            {
                pt1 = (XYIntLocation)loc1;
                g.FillEllipse(b, (float)(pt1.X - size / 2), 
                    (float)(pt1.Y - size / 2), (float)size, (float)size);
                return true;
            }
            else return false;
        }
    }
}
