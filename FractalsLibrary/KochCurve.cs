using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FractalsLibrary
{
    /// <summary>
    /// Фрактал "Кривая Коха".
    /// </summary>
    public class KochCurve : Fractal
    {
        /// <summary>
        /// Максимальная допустимая глубина рекурсии.
        /// </summary>
        public override int MaxAvaibleRecursionDepth { get; } = 7;

        /// <summary>
        /// Метод начала отрисовки фрактала.
        /// </summary>
        public override void StartRendering()
        {
            Sketch.Children.Clear();
            Line line = new Line
            {
                X1 = 0,
                X2 = Sketch.Width,
                Y1 = Sketch.Height / 2,
                Y2 = Sketch.Height / 2,
                Stroke = new SolidColorBrush(StartColor)
            };
            Sketch.Children.Add(line);

            Rendering(line, (line.X2 - line.X1) / 3, 1);
        }

        /// <summary>
        /// Метод для рекурсивной отрисовки фрактала.
        /// </summary>
        /// <param name="baseLine">Базовый отрезок.</param>
        /// <param name="lineLenght">Длина отрезков на текщений итерации.</param>
        /// <param name="recursionDepth">Текущая глубина рекурсии.</param>
        private void Rendering(Line baseLine, double lineLenght, int recursionDepth)
        {
            if (recursionDepth >= MaxRecursionDepth)
            {
                return;
            }
            Sketch.Children.Remove(baseLine);
            double angle = Math.Atan((baseLine.Y2 - baseLine.Y1) / (baseLine.X2 - baseLine.X1));
            angle += (baseLine.X2 < baseLine.X1) ? Math.PI : 0;
            Line[] lines = new Line[4];
            lines[0] = new Line
            {
                X1 = baseLine.X1,
                X2 = baseLine.X1 + (baseLine.X2 - baseLine.X1) / 3,
                Y1 = baseLine.Y1,
                Y2 = baseLine.Y1 + (baseLine.Y2 - baseLine.Y1) / 3,
                Stroke = baseLine.Stroke
            };
            lines[1] = RenderLine(lines[0], -1, angle, lineLenght, recursionDepth);
            lines[2] = RenderLine(lines[1], 1, angle, lineLenght, recursionDepth);
            lines[3] = new Line
            {
                X1 = baseLine.X1 + 2 * (baseLine.X2 - baseLine.X1) / 3,
                X2 = baseLine.X2,
                Y1 = baseLine.Y1 + 2 * (baseLine.Y2 - baseLine.Y1) / 3,
                Y2 = baseLine.Y2,
                Stroke = baseLine.Stroke
            };
            foreach (var line in lines)
            {
                Sketch.Children.Add(line);
                Rendering(line, lineLenght / 3, recursionDepth + 1);
            }
        }

        /// <summary>
        /// Метод для рисования нового элемента.
        /// </summary>
        /// <param name="baseLine">Базовый отрезок.</param>
        /// <param name="ratio">Коэфициент для отпределения угла.</param>
        /// <param name="angle">Угол на предыдущей итерации.</param>
        /// <param name="lineLenght">Длина отрезков на текщений итерации.</param>
        /// <param name="recursionDepth">Текущая глубина рекурсии.</param>
        /// <returns>Нарисованная линия.</returns>
        private Line RenderLine(Line baseLine, int ratio, double angle, double lineLenght, int recursionDepth)
        {
            Line line = new Line
            {
                X1 = baseLine.X2,
                X2 = baseLine.X2 + Math.Cos(angle + ratio * Math.PI / 3) * lineLenght,
                Y1 = baseLine.Y2,
                Y2 = baseLine.Y2 + Math.Sin(angle + ratio * Math.PI / 3) * lineLenght,
                Stroke = new SolidColorBrush(ColorPalette[recursionDepth])
            };
            return line;
        }

        /// <summary>
        /// Метод, возвращающий название фрактала.
        /// </summary>
        /// <returns>Название фрактала.</returns>
        public override string ToString() => "Кривая Коха";
    }
}
