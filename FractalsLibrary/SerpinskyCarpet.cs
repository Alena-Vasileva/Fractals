using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;

namespace FractalsLibrary
{
    /// <summary>
    /// Фрактал "Ковёр Серпинского".
    /// </summary>
    public class SerpinskyCarpet : Fractal
    {
        /// <summary>
        /// Максимальная допустимая глубина рекурсии.
        /// </summary>
        public override int MaxAvaibleRecursionDepth { get; } = 6;

        /// <summary>
        /// Метод начала отрисовки фрактала.
        /// </summary>
        public override void StartRendering()
        {
            Sketch.Children.Clear();
            Point startPoint = new Point(Sketch.Width/2, Sketch.Height/2);
            Rendering(startPoint, Sketch.Width/3,0);
        }

        /// <summary>
        /// Метод для рекурсивной отрисовки фрактала.
        /// </summary>
        /// <param name="centre">Центр нового ковра.</param>
        /// <param name="size">Размер стороны нового ковра.</param>
        /// <param name="recursionDepth">Текущая глубина рекурсии.</param>
        private void Rendering(Point centre, double size, int recursionDepth)
        {
            if (recursionDepth >= MaxRecursionDepth)
            {
                return;
            }
            Polygon rectangle = new();
            size /= 2;
            rectangle.Points.Add(new Point(centre.X - size, centre.Y - size));
            rectangle.Points.Add(new Point(centre.X + size, centre.Y - size));
            rectangle.Points.Add(new Point(centre.X + size, centre.Y + size));
            rectangle.Points.Add(new Point(centre.X - size, centre.Y + size));
            rectangle.Fill = new SolidColorBrush(ColorPalette[recursionDepth]);
            Sketch.Children.Add(rectangle);
            size *= 2;
            Rendering(new Point(centre.X - size, centre.Y - size), size / 3, recursionDepth + 1);
            Rendering(new Point(centre.X, centre.Y - size), size / 3, recursionDepth + 1);
            Rendering(new Point(centre.X + size, centre.Y - size), size / 3, recursionDepth + 1);
            Rendering(new Point(centre.X - size, centre.Y), size / 3, recursionDepth + 1);
            Rendering(new Point(centre.X + size, centre.Y), size / 3, recursionDepth + 1);
            Rendering(new Point(centre.X - size, centre.Y + size), size / 3, recursionDepth + 1);
            Rendering(new Point(centre.X, centre.Y + size), size / 3, recursionDepth + 1);
            Rendering(new Point(centre.X + size, centre.Y + size), size / 3, recursionDepth + 1);
        }

        /// <summary>
        /// Метод, возвращающий название фрактала.
        /// </summary>
        /// <returns>Название фрактала.</returns>
        public override string ToString() => "Ковёр Серпинского";
    }

}
