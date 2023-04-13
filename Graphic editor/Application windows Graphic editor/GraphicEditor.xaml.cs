using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Controls;
using System.Linq;
using System.Collections.ObjectModel;
namespace Graphic_editor
{
    namespace editor
    {
        public partial class Editor : Window
        {
            public ToolType currentTool => _currentTool.type;
            public Brush currentBrush = Brushes.Black;
            public Brush currentBrushFilling = Brushes.White;
            public int currentBrushThickness = 1;
            public System.Windows.Shapes.Path currentPath = null;
            UndoRedo undo_redo = new UndoRedo();

            public Tool _currentTool = new Pencil();

            public Editor()
            {
                InitializeComponent();
                cnvPaint.Background = Brushes.White;
            }

            private void NewBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
            {
                if (MessageBox.Show(this, "Вы хотите сохранить этот файл?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    SaveBinding_OnExecuted(sender, e);
                Clear();
                cnvPaint.Background = Brushes.White;
            }
            private void OpenBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
            {
                ImageBrush brush = new ImageBrush();
                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.AddExtension = true;
                openDialog.CheckFileExists = true;
                openDialog.DefaultExt = "png";
                openDialog.Filter = "Image files|*.png;*.jpeg;*.ico|All files (*.*)|*.*";
                double imageWidth = 0, imageHeight = 0;
                if (openDialog.ShowDialog() == true && openDialog.SafeFileName != "")
                {
                    Clear();
                    brush.ImageSource = new BitmapImage(new Uri(openDialog.FileName));
                    imageWidth = brush.ImageSource.Width;
                    imageHeight = brush.ImageSource.Height;
                    cnvPaint.Width = imageWidth;
                    cnvPaint.Height = imageHeight;
                    brush.Stretch = Stretch.Uniform;
                    cnvPaint.Background = brush;
                }
            }
            private void SaveBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog()
                {
                    Filter = "PNG (*.png)|*.png|JPEG (*.jpg)|*.jpg|All(*.*)|*",
                    FileName = "Nameless",
                    DefaultExt = "png",
                };
                int cw = (int)cnvPaint.Width;
                int ch = (int)cnvPaint.Height;
                RenderTargetBitmap renderBitmap = new RenderTargetBitmap(cw, ch, 96d, 96d, PixelFormats.Pbgra32);
                cnvPaint.Measure(new Size(cw, ch));
                cnvPaint.Arrange(new Rect(new Size(cw, ch)));
                renderBitmap.Render(cnvPaint);
                InvalidateVisual();
                if (saveFileDialog.ShowDialog() == true)
                {
                    var extension = System.IO.Path.GetExtension(saveFileDialog.FileName);
                    using (FileStream file = File.Create(saveFileDialog.FileName))
                    {
                        BitmapEncoder encoder = null;
                        switch (extension.ToLower())
                        {
                            case ".jpg":
                                encoder = new JpegBitmapEncoder();
                                break;
                            case ".png":
                                encoder = new PngBitmapEncoder();
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(extension);
                        }
                        encoder.Frames.Add(BitmapFrame.Create(renderBitmap));
                        encoder.Save(file);
                    }
                }
            }
            private void CloseBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
            {
                Close();
            }
            private void Clear()
            {
                cnvPaint.Children.Clear();
                undo_redo.undoCommands.Clear();
                undo_redo.redoCommands.Clear();
                btnRedo.IsEnabled = false;
                btnUndo.IsEnabled = false;
            }

            private void BtnPencil_Click(object sender, RoutedEventArgs e)
            {
                _currentTool = new Pencil();
                spThickness.IsEnabled = false;
            }
            private void BtnBrush_Click(object sender, RoutedEventArgs e)
            {
                _currentTool = new _Brush();
                spThickness.IsEnabled = true;
            }
            private void BtnEraser_Click(object sender, RoutedEventArgs e)
            {
                _currentTool = new Eraser();
                spThickness.IsEnabled = true;
            }
            private void CnvPaint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                _currentTool.OnPressed(this, e.GetPosition(cnvPaint));
            }

            private void CnvPaint_MouseMove(object sender, MouseEventArgs e)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    _currentTool.OnHold(e.GetPosition(cnvPaint));
                }
            }
            private void CnvPaint_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                ICommand command = _currentTool.OnReleased(cnvPaint);
                undo_redo.AddComand(command);
                btnUndo.IsEnabled = true;
            }
            private void BtnUndo_Click(object sender, RoutedEventArgs e)
            {
                undo_redo.Undo(1);
                btnRedo.IsEnabled = undo_redo.undoCommands.Count != 0;
            }
            private void BtnRedo_Click(object sender, RoutedEventArgs e)
            {
                undo_redo.Redo(1);
                btnUndo.IsEnabled = undo_redo.redoCommands.Count != 0;
            }

            private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
            {
                if (colorPicker.SelectedColor != null)
                {
                    currentBrush = new SolidColorBrush((Color)colorPicker.SelectedColor);
                }
            }

            private void ColorPickerFilling_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
            {
                if (colorPicker.SelectedColor != null)
                {
                    currentBrushFilling = new SolidColorBrush((Color)colorPickerFilling.SelectedColor);
                }
            }

            private void SlBrushThickness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {
                currentBrushThickness = (int)e.NewValue;
            }

            [DllImport("user32.dll")]
            static extern IntPtr GetDC(IntPtr hwnd);
            [DllImport("user32.dll")]
            static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);
            [DllImport("gdi32.dll")]
            static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
            public Color GetPixelColor(IntPtr hwnd, int x, int y)
            {
                IntPtr hdc = GetDC(hwnd);
                uint pixel = GetPixel(hdc, x, y);
                ReleaseDC(hwnd, hdc);
                Color color = Color.FromRgb((byte)(pixel & 0x000000FF), (byte)((pixel & 0x0000FF00) >> 8), (byte)((pixel & 0x00FF0000) >> 16));
                return color;
            }
            public void GetColor(int x, int y)
            {
                IntPtr hwnd = new WindowInteropHelper(Application.Current.MainWindow).Handle;
                var pixel = GetPixelColor(hwnd, x, y);
                currentBrush = new SolidColorBrush(pixel);
                colorPicker.SelectedColor = pixel;
            }

            private void MProperties_Click(object sender, RoutedEventArgs e)
            {
                ImageProperty propertyWindow = new ImageProperty(cnvPaint.Width, cnvPaint.Height);
                propertyWindow.Owner = this;
                propertyWindow.Show();
            }
            private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
                => e.Cancel = MessageBox.Show(this, "Вы уверены?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No;

            private void Square_Click(object sender, RoutedEventArgs e)
            {
                _currentTool = new Square();
                spThickness.IsEnabled = true;
            }

            private void Ellipse_Click(object sender, RoutedEventArgs e)
            {
                _currentTool = new Ellipse(false);
                spThickness.IsEnabled = true;
            }

            private void Rectangle_Click(object sender, RoutedEventArgs e)
            {
                _currentTool = new Rectangle();
                spThickness.IsEnabled = true;
            }

            private void Rhombus_Click(object sender, RoutedEventArgs e)
            {
                _currentTool = new Rhombus();
                spThickness.IsEnabled = true;
            }
        }
    }
}