using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace FractalsLibrary
{
    /// <summary>
    /// ������� ����� ��� ���������.
    /// </summary>
    public abstract class Fractal
    {
        /// <summary>
        /// ������������ ���������� ������� �������� ��� ���������� ��������.
        /// </summary>
        public abstract int MaxAvaibleRecursionDepth { get; }

        /// <summary>
        /// ������������ ������� �������� �� ��������� ��� ���� ���������.
        /// </summary>
        public const int DefaultMaxRecursionDepth = 5;

        /// <summary>
        /// ������������ ������� �������� ��� ���������� ��������.
        /// </summary>
        private int maxRecursionDepth = DefaultMaxRecursionDepth;

        /// <summary>
        /// ��������, ����������� ��������� ��� ������������ ������� ��������.
        /// </summary>
        public int MaxRecursionDepth
        {
            get => maxRecursionDepth;
            set
            {
                if (value < 1 || value > MaxAvaibleRecursionDepth)
                {
                    throw new FractalException("������������ ������� ��������");
                }
                maxRecursionDepth = value;
                StartRendering();
            }
        }

        /// <summary>
        /// ������� ��� ��������� ��������.
        /// </summary>
        public static Canvas Sketch { get; set; }

        /// <summary>
        /// ���� ��� ��������� ��������� ������ ��������.
        /// </summary>
        public Color StartColor { get; set; } = Color.FromRgb(255, 0, 51);

        /// <summary>
        /// ���� ��� ��������� ��������� ��������� ��������.
        /// </summary>
        public Color EndColor { get; set; } = Color.FromRgb(255, 205, 255);

        /// <summary>
        /// ������� ������ ��� ���� ��������.
        /// </summary>
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
                            (byte)(StartColor.B + (EndColor.B - StartColor.B) * i / MaxRecursionDepth));
                }
                return palette;
            }
        }

        /// <summary>
        /// ����� ������ ��������� ��������.
        /// </summary>
        public abstract void StartRendering();

        /// <summary>
        /// �����, ������������ �������� ��������.
        /// </summary>
        /// <returns>�������� ��������.</returns>
        public abstract override string ToString();
    }
}
