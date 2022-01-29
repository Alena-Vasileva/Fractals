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
        private ViewModel viewModel = new ViewModel();
        private Fractal currFractal;
        private double StartWidth;
        private double StartHeight;

        public MainWindow()
        {
            InitializeComponent();
            Fractal.Sketch = Sketch;
            StartWidth = Sketch.Width;
            StartHeight = Sketch.Height;
            DataContext = viewModel;
            MaxWidth = SystemParameters.PrimaryScreenWidth;
            MaxHeight = SystemParameters.PrimaryScreenHeight;
            MinWidth = SystemParameters.PrimaryScreenHeight / 2;
            MinHeight = SystemParameters.PrimaryScreenHeight / 2;
        }

        private void RenderNewFractal(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                currFractal = viewModel.list[ComboBoxAvaibleFractals.SelectedIndex];
                SetDefaultSettings();
                currFractal.StartRendering();
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

        private void PreviewInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }

        private void OnMaxRecursionDepthChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (int.TryParse(TextBoxMaxRecursionDepth.Text, out int newMaxRecursionDepth))
                {
                    currFractal.MaxRecursionDepth = newMaxRecursionDepth;
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

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            try
            {
                if (sizeInfo.PreviousSize.Height == 0)
                {
                    return;
                }
                double sizeRatio = sizeInfo.NewSize.Height / sizeInfo.PreviousSize.Height;
                Sketch.Height *= sizeRatio;
                Sketch.Width *= sizeRatio;
                BlownFractalTree.LineLenght *= sizeRatio;
                currFractal.StartRendering();
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

        private void SetDefaultSettings()
        {
            TextBoxMaxRecursionDepth.Text = Fractal.DefaultMaxRecursionDepth.ToString();
            TextBoxStartColorR.Text = "255";
            TextBoxStartColorG.Text = "0";
            TextBoxStartColorB.Text = "51";
            TextBoxEndColorR.Text = "255";
            TextBoxEndColorG.Text = "205";
            TextBoxEndColorB.Text = "255";
            LabelMaxRecursionDepth.Content = "<=" + currFractal.MaxAvaibleRecursionDepth.ToString();
        }

        private void OnColorChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (byte.TryParse(TextBoxStartColorR.Text, out byte startColorR) &&
                    byte.TryParse(TextBoxStartColorG.Text, out byte startColorG) &&
                    byte.TryParse(TextBoxStartColorB.Text, out byte startColorB))
                {
                    currFractal.StartColor = Color.FromRgb(startColorR, startColorG, startColorB);
                    currFractal.StartRendering();
                }

                if (byte.TryParse(TextBoxEndColorR.Text, out byte endColorR) &&
                    byte.TryParse(TextBoxEndColorG.Text, out byte endColorG) &&
                    byte.TryParse(TextBoxEndColorB.Text, out byte endColorB))
                {
                    currFractal.EndColor = Color.FromRgb(endColorR, endColorG, endColorB);
                    currFractal.StartRendering();
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

        private void OnZoomChanged(object sender, SelectionChangedEventArgs e)
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
        }

        private void ButtonSaveClick(object sender, RoutedEventArgs e)
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
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            using (FileStream file = File.Create(saveFileDialog.FileName))
            {
                encoder.Save(file);
            }
        }
    }
}
