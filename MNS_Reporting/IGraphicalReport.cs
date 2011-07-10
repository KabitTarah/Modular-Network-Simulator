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
    public enum DrawLayer { Static = 0, Background = 1, Foreground = 2 };

    public enum Action { Start, Modify, Stop };

    public interface IGraphicalReport : IReport
    {
        bool IsStatic { get; }
        bool IsDrawable { get; }
        ColorEnum Color { get; set; }           // Color Category enumerated type (intent is for user to
        // be able to configure color match-ups in GUI).
        ColorEnum FinalColor { get; set; }      // Used with gradient brush
        GraphicsPath GradientPath { get; }      // To define gradient brush path (gen. provided by the Node)
        double MaximumMessageSize { get; set; }  // To create gradient path

        Action ReportAction { get; set; }   // Ignored for IsStatic == false

        DrawLayer Layer { get; set; } // highest value indicates top layer

        IReport PreviousStatic { get; set; }   // Non-null for static reports that
        // have end/modify records.
        // This forms a 1-way linked list of
        // related records.

        void ConvertTo(XYIntLocation[] field);  // Assume Aspect Ratio pre-fixed
        // ConvertTo can also be pre-done.
        bool ConvertTo(double FieldMultiplier); // Convert any size values.
        // FieldMultiplier = Aspect_view / Aspect_sim
        bool Draw(Graphics g, ColorEnumStatics colorDef); // Draw element to graphics object g
    }
}
