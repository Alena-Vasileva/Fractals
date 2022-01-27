using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FractalsLibrary
{
    public class BlownFractalTree : Fractal
    {
        public override int MaxAvaibleRecursionDepth { get; } = 13;

        private double leftAngle = Math.PI / 3.5;
        public double LeftAngle
        {
            get => leftAngle;
            set
            {
                if (value < 30 || value > 60)
                {
                    throw new FractalException("Недопустимый угол наклона левого отрезка.\n" +
                        "Введите число от 30 до 60.");
                }
                leftAngle = value * Math.PI / 180;
                StartRendering();
            }
        }

        private double rightAngle = Math.PI / 5;
        public double RightAngle
        {
            get => rightAngle;
            set
            {
                if (value < 30 || value > 60)
                {
                    throw new FractalException("Недопустимый угол наклона правого отрезка.\n" +
                        "Введите число от 30 до 60.");
                }
                rightAngle = value * Math.PI / 180;
                StartRendering();
            }
        }

        private double lenghtRatio = 0.7;
        public double LenghtRatio
        {
            get => lenghtRatio;
            set
            {
                if (value < 0.1 || value > 0.9)
                {
                    throw new FractalException("Недопустимое отношение длин отрезков.\n" +
                        "Введите число от 0.1 до 0.9.");
                }
                lenghtRatio = value;
                StartRendering();
            }
        }

        public static double LineLenght { get; set; } = 100;

        public override void StartRendering()
        {
            Sketch.Children.Clear();
            RenderSettings();
            Line line = new Line
            {
                X1 = Sketch.Width / 2,
                X2 = Sketch.Width / 2,
                Y1 = Sketch.Height,
                Y2 = Sketch.Height - LineLenght,
                Stroke = new SolidColorBrush(StartColor)
            };
            Sketch.Children.Add(line);
            Rendering(new Point(line.X2, line.Y2), Math.PI / 2, LineLenght * LenghtRatio, 1);
        }

        private void Rendering(Point startPoint, double startAngle, double lineLenght, int recursionDepth)
        {
            if (recursionDepth >= MaxRecursionDepth)
            {
                return;
            }
            RenderBranch(startPoint, startAngle + LeftAngle, lineLenght, recursionDepth);
            RenderBranch(startPoint, startAngle - RightAngle, lineLenght, recursionDepth);
        }

        private void RenderBranch(Point startPoint, double nextAngle, double lineLenght, int recursionDepth)
        {
            Line line = new Line
            {
                X1 = startPoint.X,
                X2 = startPoint.X + Math.Cos(nextAngle) * lineLenght,
                Y1 = startPoint.Y,
                Y2 = startPoint.Y - Math.Sin(nextAngle) * lineLenght,
                Stroke = new SolidColorBrush(ColorPalette[recursionDepth])
            };
            Sketch.Children.Add(line);
            Rendering(new Point(line.X2, line.Y2), nextAngle, lineLenght * LenghtRatio, recursionDepth + 1);
        }

        public override string ToString() => "Обдуваемое фрактальное дерево";

        private void RenderSettings()
        {
            System.Windows.Controls.StackPanel settings = new();
            System.Windows.Controls.TextBox textBox = new();
            textBox.Text = Math.Round(LeftAngle / Math.PI * 180).ToString();
            textBox.KeyUp += TextBoxLeftAngleKeyUp;
            CustomizeTextBox(ref textBox);
            System.Windows.Controls.Label label = new();
            label.Content = "- Угол наклона левого отрезка";
            settings.Children.Add(SettingsStack(ref textBox, ref label));
            textBox = new();
            textBox.Text = Math.Round(RightAngle / Math.PI * 180).ToString();
            textBox.KeyUp += TextBoxRightAngleKeyUp;
            CustomizeTextBox(ref textBox);
            label = new();
            label.Content = "- Угол наклона правого отрезка";
            settings.Children.Add(SettingsStack(ref textBox, ref label));
            textBox = new();
            textBox.Text = LenghtRatio.ToString();
            textBox.KeyUp += TextBoxLenghtRatioKeyUp;
            CustomizeTextBox(ref textBox);
            label = new();
            label.Content = "- Отношение длин отрезков";
            settings.Children.Add(SettingsStack(ref textBox, ref label));
            Sketch.Children.Add(settings);
        }

        private void CustomizeTextBox(ref System.Windows.Controls.TextBox textBox)
        {
            textBox.Width = 50;
            textBox.VerticalAlignment = VerticalAlignment.Center;
            textBox.Margin = new Thickness(5, 5, 0, 5);
        }

        private System.Windows.Controls.StackPanel SettingsStack(ref System.Windows.Controls.TextBox textBox,
            ref System.Windows.Controls.Label label)
        {
            System.Windows.Controls.StackPanel settings = new();
            settings.Orientation = System.Windows.Controls.Orientation.Horizontal;
            settings.Children.Add(textBox);
            settings.Children.Add(label);
            return settings;
        }

        private void TextBoxLeftAngleKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                LeftAngle = double.Parse(((System.Windows.Controls.TextBox)sender).Text);
        }

        private void TextBoxRightAngleKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                RightAngle = double.Parse(((System.Windows.Controls.TextBox)sender).Text);
        }

        private void TextBoxLenghtRatioKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                LenghtRatio = double.Parse(((System.Windows.Controls.TextBox)sender).Text);
        }
    }
}