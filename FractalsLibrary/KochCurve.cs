using System;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FractalsLibrary
{
    public class KochCurve : Fractal
    {
        public override int MaxAvaibleRecursionDepth { get; } = 7;

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

        private void Rendering(Line baseLine, double lineLenght, int recursionDepth)
        {
            if (recursionDepth >= MaxRecursionDepth)
            {
                return;
            }
            Debug.WriteLine("Rec");
            Sketch.Children.Remove(baseLine);
            double angle = Math.Atan((baseLine.Y2 - baseLine.Y1) / (baseLine.X2 - baseLine.X1));
            if ( baseLine.X2 < baseLine.X1)
            {
                angle += Math.PI;
            }
            Line[] lines = new Line[4];
            lines[0] = new Line
            {
                X1 = baseLine.X1,
                X2 = baseLine.X1 + (baseLine.X2 - baseLine.X1) / 3,
                Y1 = baseLine.Y1,
                Y2 = baseLine.Y1 + (baseLine.Y2 - baseLine.Y1) / 3,
                Stroke = baseLine.Stroke
            };
            lines[1] = new Line
            {
                X1 = lines[0].X2,
                X2 = lines[0].X2 + Math.Cos(angle - Math.PI / 3) * lineLenght,
                Y1 = lines[0].Y2,
                Y2 = lines[0].Y2 + Math.Sin(angle - Math.PI / 3) * lineLenght,
                Stroke = new SolidColorBrush(ColorPalette[recursionDepth])
            };
            lines[2] = new Line
            {
                X1 = lines[1].X2,
                X2 = lines[1].X2 + Math.Cos(angle + Math.PI / 3) * lineLenght,
                Y1 = lines[1].Y2,
                Y2 = lines[1].Y2 + Math.Sin(angle + Math.PI / 3) * lineLenght,
                Stroke = new SolidColorBrush(ColorPalette[recursionDepth])
            };
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

        public override string ToString() => "Кривая Коха";
    }
}
