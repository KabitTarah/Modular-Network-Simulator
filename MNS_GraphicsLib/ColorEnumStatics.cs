using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MNS_GraphicsLib
{
    public enum ColorEnum
    {
        BackgroundColor, DarkBackgroundColor, FlashOverride, Flash,
        Immediate, Priority, Routine, Black, White, Node, Sink, Collision,
        Busy, Transparent
    };

    public class ColorEnumStatics
    {
        static int alpha = 96;
        public Color BackgroundColor = Color.White;
        public Color DarkBackgroundColor = Color.Gray;
        public Color FlashOverride = Color.FromArgb(alpha, Color.Red);
        public Color Flash = Color.FromArgb(alpha, Color.Orange);
        public Color Immediate = Color.FromArgb(alpha, Color.Yellow);
        public Color Priority = Color.FromArgb(alpha, Color.Green);
        public Color Routine = Color.FromArgb(alpha, Color.DarkGray);
        public Color Black = Color.Black;
        public Color White = Color.White;
        public Color Node = Color.Coral;
        public Color Sink = Color.Green;
        public Color Collision = Color.Red;
        public Color Busy = Color.Yellow;
        public Color Transparent = Color.Transparent;

        public ColorEnumStatics() { }

        public Color GetColorFromEnum(ColorEnum color)
        {
            switch (color)
            {
                case ColorEnum.BackgroundColor:
                    return BackgroundColor;
                case ColorEnum.DarkBackgroundColor:
                    return DarkBackgroundColor;
                case ColorEnum.FlashOverride:
                    return FlashOverride;
                case ColorEnum.Flash:
                    return Flash;
                case ColorEnum.Immediate:
                    return Immediate;
                case ColorEnum.Priority:
                    return Priority;
                case ColorEnum.Routine:
                    return Routine;
                case ColorEnum.Black:
                    return Black;
                case ColorEnum.White:
                    return White;
                case ColorEnum.Node:
                    return Node;
                case ColorEnum.Sink:
                    return Sink;
                case ColorEnum.Collision:
                    return Collision;
                case ColorEnum.Busy:
                    return Busy;
                case ColorEnum.Transparent:
                    return Color.Transparent;
                default:
                    return Color.Transparent;
            }

        }
    }
}
