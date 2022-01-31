using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FractalsLibrary;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace Fractals
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Экземпляр вспомогательного класса.
        /// </summary>
        private ViewModel viewModel = new ViewModel();

        /// <summary>
        /// Ссылка на текущий фрактал.
        /// </summary>
        private Fractal currFractal;

        /// <summary>
        /// Начальная ширина канваса.
        /// </summary>
        private double StartWidth;

        /// <summary>
        /// Начальная высота канваса.
        /// </summary>
        private double StartHeight;

        /// <summary>
        /// Конструктор, задающий основные параметры.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Fractal.Sketch = Sketch;
            DataContext = viewModel;
            MaxWidth = SystemParameters.PrimaryScreenWidth;
            MaxHeight = SystemParameters.PrimaryScreenHeight;
            MinWidth = SystemParameters.PrimaryScreenWidth / 2;
            MinHeight = SystemParameters.PrimaryScreenHeight / 2;
        }

        /// <summary>
        /// Вспомогательный метод для настройки размера канваса.
        /// </summary>
        private void SetCanvasSize()
        {
            try
            {
                Sketch.Width = ScrollViewerCanvas.ActualHeight;
                Sketch.Height = ScrollViewerCanvas.ActualHeight;
                StartWidth = Sketch.Width;
                StartHeight = Sketch.Height;
            }
            catch (FractalException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при отрисовке фрактала");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Непридвиденная ошибка");
            }
        }

        /// <summary>
        /// Вспомогательный метод для смены фрактала.
        /// </summary>
        /// <param name="sender">Ссылка на комбобокс.</param>
        /// <param name="e">Дополнительные сведения.</param>
        private void RenderNewFractal(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (StartHeight == 0)
                {
                    SetCanvasSize();
                }
                currFractal = viewModel.list[ComboBoxAvaibleFractals.SelectedIndex];
                SetDefaultSettings();
                currFractal.StartRendering();
                SetSpecificSettings();
            }
            catch (FractalException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при отрисовке фрактала");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Непридвиденная ошибка");
            }
        }

        /// <summary>
        /// Вспомогательный метод для выставления параметров, специфичных для каждого фрактала.
        /// </summary>
        private void SetSpecificSettings()
        {
            if (currFractal is FractalsLibrary.BlownFractalTree)
            {
                RenderFractalTreeSettings();
            }
            if (currFractal is FractalsLibrary.CantorSet)
            {
                RenderCantorSetSettings();
            }
        }

        /// <summary>
        /// Предпросмотр ввода цифр.
        /// </summary>
        /// <param name="sender">Ссылка на текстбокс.</param>
        /// <param name="e">Дополнительные сведения.</param>
        private void PreviewInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                if (!Char.IsDigit(e.Text, 0))
                {
                    e.Handled = true;
                }
            }
            catch { }
        }

        /// <summary>
        /// Вспомогательный метод для обработки изменения глубины рекурсии текущего фрактала.
        /// </summary>
        /// <param name="sender">Ссылка на комбобокс.</param>
        /// <param name="e">Дополнительные сведения.</param>
        private void OnMaxRecursionDepthChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                if (currFractal != null)
                {
                    currFractal.MaxRecursionDepth = (int)SliderMaxRecursionDepth.Value;
                    SetSpecificSettings();
                }
            }
            catch (FractalException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при отрисовке фрактала");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Непридвиденная ошибка");
            }
        }

        /// <summary>
        /// Вспомогательный метод для обработки изменения размера окна.
        /// </summary>
        /// <param name="sizeInfo">Информация о размере.</param>
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            try
            {
                if (sizeInfo.PreviousSize.Height == 0)
                {
                    return;
                }
                double sizeRatio = sizeInfo.NewSize.Height / sizeInfo.PreviousSize.Height;
                StartHeight = Sketch.Height *= sizeRatio;
                StartWidth = Sketch.Width *= sizeRatio;
                BlownFractalTree.LineLenght *= sizeRatio;
                currFractal.StartRendering();
                SetSpecificSettings();
            }
            catch (FractalException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при отрисовке фрактала");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Непридвиденная ошибка");
            }
        }

        /// <summary>
        /// Выставление настроек по умолчанию.
        /// </summary>
        private void SetDefaultSettings()
        {
            SliderStartColorR.Value = 255;
            SliderStartColorG.Value = 0;
            SliderStartColorB.Value = 51;
            SliderEndColorR.Value = 255;
            SliderEndColorG.Value = 205;
            SliderEndColorB.Value = 255;
            SliderMaxRecursionDepth.Maximum = currFractal.MaxAvaibleRecursionDepth;
            SliderMaxRecursionDepth.Value = Fractal.DefaultMaxRecursionDepth;
            ScrollViewerCanvas.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
            ScrollViewerCanvas.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
        }

        /// <summary>
        /// Вспомогательный метод для обработки зума фрактала.
        /// </summary>
        /// <param name="sender">Ссылка на комбобокс.</param>
        /// <param name="e">Дополнительные сведения.</param>
        private void OnZoomChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Sketch == null)
                {
                    return;
                }
                double scale = ComboBoxZoom.SelectedIndex switch
                {
                    0 => 1,
                    1 => 2,
                    2 => 3,
                    3 => 5,

                };
                if (scale == 1)
                {
                    ScrollViewerCanvas.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                    ScrollViewerCanvas.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                }
                else
                {
                    ScrollViewerCanvas.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                    ScrollViewerCanvas.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                }
                Sketch.Width = StartWidth * scale;
                Sketch.Height = StartHeight * scale;
                currFractal.StartRendering();
                SetSpecificSettings();
            }
            catch (FractalException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при отрисовке фрактала");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Непридвиденная ошибка");
            }
        }

        /// <summary>
        /// Сохранение изображения.
        /// </summary>
        /// <param name="sender">Ссылка на кнопку.</param>
        /// <param name="e">Дополнительные сведения.</param>
        private void ButtonSaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.DefaultExt = ".PNG";
                saveFileDialog.Filter = "Image (.PNG)|*.PNG";
                if (saveFileDialog.ShowDialog() != true)
                {
                    return;
                }
                RenderTargetBitmap bmp = new RenderTargetBitmap((int)Sketch.ActualWidth * 300 / 96, (int)Sketch.ActualHeight * 300 / 96, 300, 300, PixelFormats.Pbgra32);
                Size size = Sketch.RenderSize;
                Sketch.Measure(size);
                Sketch.Arrange(new Rect(size));
                bmp.Render(Sketch);
                PngBitmapEncoder encoder = new();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                using (FileStream file = File.Create(saveFileDialog.FileName))
                {
                    encoder.Save(file);
                }
            }
            catch (FractalException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при отрисовке фрактала");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Непридвиденная ошибка");
            }
        }

        /// <summary>
        /// Метод для отрисовки специфических настроек фрактала.
        /// </summary>
        private void RenderFractalTreeSettings()
        {
            try
            {
                StackPanel settings = new();
                TextBox textBox = new();
                textBox.Text = Math.Round((currFractal as BlownFractalTree).LeftAngle / Math.PI * 180).ToString();
                textBox.KeyUp += TextBoxLeftAngleKeyUp;
                CustomizeTextBox(ref textBox);
                Label label = new();
                label.Content = "- Угол наклона левого отрезка";
                settings.Children.Add(SettingsStack(ref textBox, ref label));
                textBox = new();
                textBox.Text = Math.Round((currFractal as BlownFractalTree).RightAngle / Math.PI * 180).ToString();
                textBox.KeyUp += TextBoxRightAngleKeyUp;
                CustomizeTextBox(ref textBox);
                label = new();
                label.Content = "- Угол наклона правого отрезка";
                settings.Children.Add(SettingsStack(ref textBox, ref label));
                textBox = new();
                textBox.Text = (currFractal as BlownFractalTree).LenghtRatio.ToString();
                textBox.KeyUp += TextBoxLenghtRatioKeyUp;
                CustomizeTextBox(ref textBox);
                label = new();
                label.Content = "- Отношение длин отрезков";
                settings.Children.Add(SettingsStack(ref textBox, ref label));
                Sketch.Children.Add(settings);
            }
            catch (FractalException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при отрисовке фрактала");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Непридвиденная ошибка");
            }
        }

        /// <summary>
        /// Выставление настроек элемента.
        /// </summary>
        /// <param name="textBox">Ссылка на элемент.</param>
        private void CustomizeTextBox(ref TextBox textBox)
        {
            textBox.Width = 50;
            textBox.VerticalAlignment = VerticalAlignment.Center;
            textBox.Margin = new Thickness(5, 5, 0, 5);
            textBox.PreviewTextInput += PreviewInput;
        }

        /// <summary>
        /// Создание комбинации из поля ввода и подписи.
        /// </summary>
        /// <param name="textBox">Поле ввода.</param>
        /// <param name="label">Подпись.</param>
        /// <returns>Стэк из поля ввода и подписи.</returns>
        private StackPanel SettingsStack(ref TextBox textBox, ref Label label)
        {
            StackPanel settings = new();
            settings.Orientation = Orientation.Horizontal;
            settings.Children.Add(textBox);
            settings.Children.Add(label);
            return settings;
        }

        /// <summary>
        /// Привенение настроек по нажатию клавиши Enter.
        /// </summary>
        /// <param name="sender">Ссылка на поле ввода.</param>
        /// <param name="e">Дополнительные сведения.</param>
        private void TextBoxLeftAngleKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    (currFractal as BlownFractalTree).LeftAngle = double.Parse(((TextBox)sender).Text);
                    RenderFractalTreeSettings();
                }
            }
            catch (FractalException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при отрисовке фрактала");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Непридвиденная ошибка");
            }
        }

        /// <summary>
        /// Привенение настроек по нажатию клавиши Enter.
        /// </summary>
        /// <param name="sender">Ссылка на поле ввода.</param>
        /// <param name="e">Дополнительные сведения.</param>
        private void TextBoxRightAngleKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    (currFractal as BlownFractalTree).RightAngle = double.Parse(((TextBox)sender).Text);
                    RenderFractalTreeSettings();
                }
            }
            catch (FractalException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при отрисовке фрактала");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Непридвиденная ошибка");
            }
        }

        /// <summary>
        /// Привенение настроек по нажатию клавиши Enter.
        /// </summary>
        /// <param name="sender">Ссылка на поле ввода.</param>
        /// <param name="e">Дополнительные сведения.</param>
        private void TextBoxLenghtRatioKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    (currFractal as BlownFractalTree).LenghtRatio = double.Parse(((TextBox)sender).Text);
                    RenderFractalTreeSettings();
                }
            }
            catch (FractalException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при отрисовке фрактала");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Непридвиденная ошибка");
            }
        }

        /// <summary>
        /// Метод для отрисовки специфических настроек фрактала.
        /// </summary>
        private void RenderCantorSetSettings()
        {
            TextBox textBoxDistance = new();
            textBoxDistance.Text = (currFractal as CantorSet).Distance.ToString();
            textBoxDistance.KeyUp += TextBoxDistanceKeyUp;
            textBoxDistance.Width = 50;
            textBoxDistance.VerticalAlignment = VerticalAlignment.Center;
            textBoxDistance.Margin = new Thickness(5, 5, 0, 5);
            Label labelDistance = new();
            labelDistance.Content = "- Растояние между отрезками";
            StackPanel settings = new();
            settings.Orientation = Orientation.Horizontal;
            settings.Children.Add(textBoxDistance);
            settings.Children.Add(labelDistance);
            Sketch.Children.Add(settings);
        }

        /// <summary>
        /// Выставление настроек элемента.
        /// </summary>
        /// <param name="textBox">Ссылка на элемент.</param>
        private void TextBoxDistanceKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    (currFractal as CantorSet).Distance = int.Parse(((TextBox)sender).Text);
                    RenderCantorSetSettings();
                }
            }
            catch (FractalException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при отрисовке фрактала");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Непридвиденная ошибка");
            }
        }

        /// <summary>
        /// Изменение цвета первой итерации.
        /// </summary>
        /// <param name="sender">Ссылка на изменённый слайдер.</param>
        /// <param name="e">Дополнительные сведения.</param>
        private void OnStartColorChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                if (currFractal != null)
                {
                    currFractal.StartColor = Color.FromRgb((byte)SliderStartColorR.Value,
                        (byte)SliderStartColorG.Value, (byte)SliderStartColorB.Value);
                    currFractal.StartRendering();
                    SetSpecificSettings();
                }
            }
            catch (FractalException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при отрисовке фрактала");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Непридвиденная ошибка");
            }
        }

        /// <summary>
        /// Изменение цвета последней итерации.
        /// </summary>
        /// <param name="sender">Ссылка на изменённый слайдер.</param>
        /// <param name="e">Дополнительные сведения.</param>
        private void OnEndColorChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                if (currFractal != null)
                {
                    currFractal.EndColor = Color.FromRgb((byte)SliderEndColorR.Value,
                        (byte)SliderEndColorG.Value, (byte)SliderEndColorB.Value);
                    currFractal.StartRendering();
                    SetSpecificSettings();
                }
            }
            catch (FractalException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при отрисовке фрактала");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Непридвиденная ошибка");
            }
        }
    }
}
