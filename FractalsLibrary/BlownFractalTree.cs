using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FractalsLibrary
{
    /// <summary>
    /// Фрактал "Обдуваемое ветром фрактальное дерево".
    /// </summary>
    public class BlownFractalTree : Fractal
    {
        /// <summary>
        /// Максимальная допустимая глубина рекурсии.
        /// </summary>
        public override int MaxAvaibleRecursionDepth { get; } = 13;

        /// <summary>
        /// Угол наклона левого отрезка.
        /// </summary>
        private double leftAngle = Math.PI / 3.5;

        /// <summary>
        /// Свойство, задающее поведение угла наклона левого отрезка.
        /// </summary>
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

        /// <summary>
        /// Угол наклона правого отрезка.
        /// </summary>
        private double rightAngle = Math.PI / 5;

        /// <summary>
        /// Свойство, задающее поведение угла наклона правого отрезка.
        /// </summary>
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

        /// <summary>
        /// Отношение длин отрезков на текущей и последующей итерации.
        /// </summary>
        private double lenghtRatio = 0.7;

        /// <summary>
        /// Свойство, задающее поведение отношения длин отрезков на текущей и последующей итерации.
        /// </summary>
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

        /// <summary>
        /// Длина начального отрезка.
        /// </summary>
        public static double LineLenght { get; set; }

        /// <summary>
        /// Метод начала отрисовки фрактала.
        /// </summary>
        public override void StartRendering()
        {
            Sketch.Children.Clear();
            RenderSettings();
            LineLenght = Sketch.Height / 4;
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

        /// <summary>
        /// Метод для рекурсивной отрисовки фрактала.
        /// </summary>
        /// <param name="startPoint">Начальная точка.</param>
        /// <param name="startAngle">Угол наклона предыдущего отрезка.</param>
        /// <param name="lineLenght">Длина отрезков на текщений итерации.</param>
        /// <param name="recursionDepth">Текущая глубина рекурсии.</param>
        private void Rendering(Point startPoint, double startAngle, double lineLenght, int recursionDepth)
        {
            if (recursionDepth >= MaxRecursionDepth)
            {
                return;
            }
            RenderBranch(startPoint, startAngle + LeftAngle, lineLenght, recursionDepth);
            RenderBranch(startPoint, startAngle - RightAngle, lineLenght, recursionDepth);
        }

        /// <summary>
        /// Метод для рисования одной ветви фрактала.
        /// </summary>
        /// <param name="startPoint">Начальная точка.</param>
        /// <param name="nextAngle">Угол наклона ветви.</param>
        /// <param name="lineLenght">Длина ветви.</param>
        /// <param name="recursionDepth">Текущая глубина рекурсии.</param>
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

        /// <summary>
        /// Метод, возвращающий название фрактала.
        /// </summary>
        /// <returns>Название фрактала.</returns>
        public override string ToString() => "Обдуваемое фрактальное дерево";

        /// <summary>
        /// Метод для отрисовки специфических настроек фрактала.
        /// </summary>
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

        /// <summary>
        /// Выставление настроек элемента.
        /// </summary>
        /// <param name="textBox">Ссылка на элемент.</param>
        private void CustomizeTextBox(ref System.Windows.Controls.TextBox textBox)
        {
            textBox.Width = 50;
            textBox.VerticalAlignment = VerticalAlignment.Center;
            textBox.Margin = new Thickness(5, 5, 0, 5);
        }

        /// <summary>
        /// Создание комбинации из поля ввода и подписи.
        /// </summary>
        /// <param name="textBox">Поле ввода.</param>
        /// <param name="label">Подпись.</param>
        /// <returns>Стэк из поля ввода и подписи.</returns>
        private System.Windows.Controls.StackPanel SettingsStack(ref System.Windows.Controls.TextBox textBox,
            ref System.Windows.Controls.Label label)
        {
            System.Windows.Controls.StackPanel settings = new();
            settings.Orientation = System.Windows.Controls.Orientation.Horizontal;
            settings.Children.Add(textBox);
            settings.Children.Add(label);
            return settings;
        }

        /// <summary>
        /// Привенение настроек по нажатию клавиши Enter.
        /// </summary>
        /// <param name="sender">Ссылка на поле ввода.</param>
        /// <param name="e">Дополнительные сведения.</param>
        private void TextBoxLeftAngleKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                LeftAngle = double.Parse(((System.Windows.Controls.TextBox)sender).Text);
        }

        /// <summary>
        /// Привенение настроек по нажатию клавиши Enter.
        /// </summary>
        /// <param name="sender">Ссылка на поле ввода.</param>
        /// <param name="e">Дополнительные сведения.</param>
        private void TextBoxRightAngleKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                RightAngle = double.Parse(((System.Windows.Controls.TextBox)sender).Text);
        }

        /// <summary>
        /// Привенение настроек по нажатию клавиши Enter.
        /// </summary>
        /// <param name="sender">Ссылка на поле ввода.</param>
        /// <param name="e">Дополнительные сведения.</param>
        private void TextBoxLenghtRatioKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                LenghtRatio = double.Parse(((System.Windows.Controls.TextBox)sender).Text);
        }
    }
}