using OcrTranslator.Helpers;
using OcrTranslator.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OcrTranslator
{

    /// <summary>
    /// Interaction logic for TextOverlay.xaml
    /// </summary>
    public partial class TextOverlay : Window
    {
        private static HttpClient httpClient = new HttpClient();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);
        private System.Drawing.Rectangle areaRectangle;
        private System.Drawing.Rectangle screenRectangle;
        private string ocrValue;
        int margin = 5;
        //private DpiScale dpiScale;
        public TextOverlay(System.Drawing.Rectangle areaRectangleParam, System.Drawing.Rectangle screenRectangleParam, string ocrValue)
        {
            
            areaRectangle = areaRectangleParam;
            screenRectangle = screenRectangleParam;
            //dpiScale = dpiScaleParam;

            //Left = screenRectangle.Left;
            //Top = screenRectangle.Top;
            //Width = screenRectangle.Width / dpiScale.DpiScaleX;
            //Height = screenRectangle.Height / dpiScale.DpiScaleY;

            InitializeComponent();
            StartRotationAnimation();
            OcrValueTextBlock.MaxWidth = areaRectangle.Width;
            this.ocrValue = TitleText.Text = ocrValue;
            Top = areaRectangle.Y - 130;
            Left = screenRectangle.X <= 0 ? screenRectangle.X + areaRectangle.X : areaRectangle.X;
            //Left = screenRectangle.Left;
            //Top = screenRectangle.Top;
            this.SizeChanged += Window_SizeChanged;
        }
        private void StartRotationAnimation()
        {
            // Configura a animação de rotação
            DoubleAnimation rotateAnimation = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = new Duration(TimeSpan.FromSeconds(2)),
                RepeatBehavior = RepeatBehavior.Forever // Roda indefinidamente
            };

            // Aplica a animação ao RotateTransform
            iconRotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            WindowUtilities.CloseAllTextOverlays();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            WindowUtilities.OcrOverlayKeyDown(e.Key);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //Lets Get The Translation 
                using StringContent jsonContent = new(JsonSerializer.Serialize(
                    new
                    {
                        text = ocrValue,
                    }
                    ), Encoding.UTF8, "application/json"
                    );

                using HttpResponseMessage response = await httpClient.PostAsync("https://localhost:7347/AI", jsonContent);


                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonFormatted = JsonSerializer.Deserialize<CreateAIResponse>(jsonResponse);
                    FormatText(jsonFormatted.response);
                    _ = Dispatcher.BeginInvoke(() =>
                    {
                        ThisWindow.Visibility = Visibility.Visible;
                    });

                }


                //this.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
                //this.Arrange(new Rect(0, 0, this.DesiredSize.Width, this.DesiredSize.Height));

                //IntPtr hwnd = new WindowInteropHelper(this).Handle;
                //Top = areaRectangle.Y - Height <= 0 ? areaRectangle.Y + this.ActualHeight + margin : areaRectangle.Y - this.ActualHeight - margin;
                //Left = screenRectangle.X <= 0 ? screenRectangle.X + areaRectangle.X : areaRectangle.X;

                //Top = areaRectangle.Y - screenRectangle.Y
                // The first move puts it on the correct monitor, which triggers WM_DPICHANGED
                // The +1/-1 coerces WPF to update Window.Top/Left/Width/Height in the second move
                //MoveWindow(hwnd, (int)(screenRectangle.Left + 1), (int)screenRectangle.Top, (int)(screenRectangle.Width - 1), (int)screenRectangle.Height, false);
                //MoveWindow(hwnd, (int)screenRectangle.Left, (int)screenRectangle.Top, (int)screenRectangle.Width, (int)screenRectangle.Height, true);
            }
            catch(Exception ex)
            {

            }
            finally
            {
                SIconRefresh.Visibility = Visibility.Collapsed;
                OcrValueTextBlock.Visibility = Visibility.Visible;
            }

        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateWindowPosition();
        }
        private void UpdateWindowPosition()
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
          
            Top = areaRectangle.Y - Height <= 0
                ? areaRectangle.Y + ActualHeight + margin
                : areaRectangle.Y - ActualHeight - margin;
            Left = screenRectangle.X <= 0
                ? screenRectangle.X + areaRectangle.X
                : areaRectangle.X;

            // Atualiza a posição da janela
            MoveWindow(hwnd, (int)Left, (int)Top, (int)Width, (int)Height, true);
        }
        private void FormatText(string input)
        {
            // Limpa o conteúdo existente
            OcrValueTextBlock.Document.Blocks.Clear();

            // Quebra as linhas para identificar parágrafos
            string[] lines = input.Split("\n\n");

            foreach (var line in lines)
            {
                Paragraph paragraph = new Paragraph();

                // Substitui estilos e cria partes formatadas
                string[] boldParts = line.Split("**");
                for (int i = 0; i < boldParts.Length; i++)
                {
                    if (i % 2 == 1) // Partes dentro de ** são negrito
                    {
                        Run boldRun = new Run(boldParts[i])
                        {
                            FontWeight = FontWeights.Bold
                        };
                        paragraph.Inlines.Add(boldRun);
                    }
                    else
                    {
                        string[] italicParts = boldParts[i].Split("*");
                        for (int j = 0; j < italicParts.Length; j++)
                        {
                            if (j % 2 == 1) // Partes dentro de * são itálico
                            {
                                Run italicRun = new Run(italicParts[j])
                                {
                                    FontStyle = FontStyles.Italic
                                };
                                paragraph.Inlines.Add(italicRun);
                            }
                            else
                            {
                                paragraph.Inlines.Add(new Run(italicParts[j]));
                            }
                        }
                    }
                }

                OcrValueTextBlock.Document.Blocks.Add(paragraph);
            }
        }


        public void ShowOnSpecificScreen(uint screenIndex)
        {
            // Validate screen index
            if (screenIndex < 0 || screenIndex >= Screen.AllScreens.Length)
            {
                System.Windows.MessageBox.Show("Invalid screen index.");
                return;
            }

            // Get the target screen
            Screen targetScreen = Screen.AllScreens[screenIndex];
            System.Drawing.Rectangle workingArea = targetScreen.WorkingArea;

            // Set the position of the window
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = workingArea.Left; // Position the window at the left edge of the screen
            this.Top = workingArea.Top; // Position the window at the top edge of the screen
            this.Width = workingArea.Width; // Optional: Adjust width to match the screen width
            this.Height = workingArea.Height; // Optional: Adjust height to match the screen height

            // Show the window
            this.Show();
        }
    }
}
