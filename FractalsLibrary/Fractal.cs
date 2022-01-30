using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace FractalsLibrary
{
    /// <summary>
    /// Базовый класс для фракталов.
    /// </summary>
    public abstract class Fractal
    {
        /// <summary>
        /// Максимальная допустимая глубина рекурсии для выбранного фрактала.
        /// </summary>
        public abstract int MaxAvaibleRecursionDepth { get; }

        /// <summary>
        /// Максимальная глубина рекурсии по умолчанию для всех фракталов.
        /// </summary>
        public const int DefaultMaxRecursionDepth = 5;

        /// <summary>
        /// Максимальная глубина рекурсии для выбранного фрактала.
        /// </summary>
        private int maxRecursionDepth = DefaultMaxRecursionDepth;

        /// <summary>
        /// Свойство, реализующее поведение для максимальной глубины рекурсии.
        /// </summary>
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

        /// <summary>
        /// Область для рисования фрактала.
        /// </summary>
        public static Canvas Sketch { get; set; }

        /// <summary>
        /// Цвет для отрисовки элементов первой итерации.
        /// </summary>
        public Color StartColor { get; set; } = Color.FromRgb(255, 0, 51);

        /// <summary>
        /// Цвет для отрисовки элементов последней итерации.
        /// </summary>
        public Color EndColor { get; set; } = Color.FromRgb(255, 205, 255);

        /// <summary>
        /// Палитра цветов для всех итераций.
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
        /// Метод начала отрисовки фрактала.
        /// </summary>
        public abstract void StartRendering();

        /// <summary>
        /// Метод, возвращающий название фрактала.
        /// </summary>
        /// <returns>Название фрактала.</returns>
        public abstract override string ToString();
    }
}
