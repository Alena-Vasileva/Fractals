using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace FractalsLibrary
{
    public abstract class Fractal
    {
        public abstract int MaxAvaibleRecursionDepth { get; }

        public const int DefaultMaxRecursionDepth = 5;
        private int maxRecursionDepth = DefaultMaxRecursionDepth;
        public int MaxRecursionDepth
        {
            get => maxRecursionDepth;
            set
            {
                if (value < 1 || value > MaxAvaibleRecursionDepth)
                {
                    throw new FractalException("Недопустимая глубина рекурсии");
                }
                maxRecursionDepth = value;
                StartRendering();
            }
        }

        public static Canvas Sketch { get; set; }

        public Color StartColor { get; set; } = Color.FromRgb(255, 0, 51);

        public Color EndColor { get; set; } = Color.FromRgb(255, 205, 255);

        public Color[] ColorPalette
        {
            get
            {
                Color[] palette = new Color[MaxRecursionDepth];
                for (int i = 0; i < MaxRecursionDepth; i++)
                {
                    palette[i] = Color.FromRgb(
                            (byte)(StartColor.R + (EndColor.R - StartColor.R) * i / MaxRecursionDepth),
                            (byte)(StartColor.G + (EndColor.G - StartColor.G) * i / MaxRecursionDepth),
                            (byte)(StartColor.B + (EndColor.B - StartColor.B) * i / MaxRecursionDepth)
                        );
                }
                return palette;
            }
        }

        public Fractal()
        { }

        public abstract void StartRendering();

        public abstract override string ToString();
    }
}
