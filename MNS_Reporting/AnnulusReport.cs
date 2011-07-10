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
    public class AnnulusReport : IGraphicalReport
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

        DrawLayer layer = DrawLayer.Background;
        public DrawLayer Layer
        {
            get { return layer; }
            set { layer = value; }
        }

        double size;
        public double Size
        { get { return size; } }

        double width;
        public double Width
        { get { return width; } }

        double maximumMessageSize = 0;
        public double MaximumMessageSize
        {
            get { return maximumMessageSize; }
            set { maximumMessageSize = value; }
        }

        double fieldMultiplied = 1;
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
        ColorEnum finalColor = ColorEnum.Transparent;
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

        GraphicsPath path1, path2;
        Region region;

        public AnnulusReport(double time, ILocation loc, double size, double width, GraphicsPath gradientPath)
        {
            this.time = time;
            loc1 = loc;
            this.size = size;
            this.width = width;
            this.gradientPath = gradientPath;
        }

        public AnnulusReport(double time, ILocation loc, double size, double width, GraphicsPath gradientPath,
            bool isStatic, int id)
        {
            this.time = time;
            loc1 = loc;
            this.size = size;
            this.width = width;
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
            if (isPointConverted)
            {
                XYIntLocation pt1 = (XYIntLocation)loc1;

                // Undo any previous field multiplier, then redo with new field multiplier.
                size = (double)(size / fieldMultiplied);
                size = (double)(size * FieldMultiplier);
                width = (double)(width / fieldMultiplied);
                width = (double)(width * FieldMultiplier);
                maximumMessageSize = (double)(maximumMessageSize / fieldMultiplied);
                maximumMessageSize = (double)(maximumMessageSize * FieldMultiplier);

                // Quicker if this is all precached!!
                path1 = new GraphicsPath();
                path2 = new GraphicsPath();
                path1.AddEllipse((float)(pt1.X - size / 2),
                    (float)(pt1.Y - size / 2), (float)size, (float)size);
                path2.AddEllipse((float)(pt1.X - (size - 2 * width) / 2),
                    (float)(pt1.Y - (size - 2 * width) / 2),
                    (float)(size - 2 * width), (float)(size - 2 * width));

                region = new Region(path1);
                region.Exclude(path2);

                fieldMultiplied = FieldMultiplier;
                isDrawable = true;
                return true;
            }
            else return false;  // Location must be converted first!
        }

        public bool Draw(Graphics g, ColorEnumStatics colorDef)
        {
            if (gradientPath.PointCount == 0) return false;
            Color[] colors = { colorDef.GetColorFromEnum(finalColor) };
            PathGradientBrush b = new PathGradientBrush(gradientPath);
            b.CenterColor = colorDef.GetColorFromEnum(color);
            b.SurroundColors = colors;

            //SolidBrush b = new SolidBrush(colorDef.GetColorFromEnum(color));

            if ((loc1 is XYIntLocation) && isDrawable)
            {
                g.FillRegion(b, region);

                return true;
            }
            else return false;
        }
    }
}
