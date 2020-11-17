using GameOverlay.Windows;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace FullscreenOverlay
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IntPtr gameclient;
        private UtilityOverlay overlayinstance = new UtilityOverlay();
        public static bool ShowOverlayText = false;
        public static string myText = "Overlay is loaded!";
        private static bool StopTimer = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TimerCheckWindows()
        {
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };

            timer.Tick += (sender, EventArgs) =>
            {
                if (StopTimer)
                {
                    timer.Stop();
                }

                if (StopTimer == false && WindowHelper.GetForegroundWindow() != overlayinstance.Myoverlaywin.Handle)
                {
                    WindowHelper.EnableBlurBehind(gameclient);
                    overlayinstance.Myoverlaywin.Recreate();
                }
            };
            timer.Start();
        }

        public IntPtr GetProcessHandle(string client)
        {
            IntPtr handle = IntPtr.Zero;
            try
            {
                var processes = Process.GetProcessesByName(client);
                if (processes.Length > 0)
                {
                    handle = processes[0].MainWindowHandle;
                }
                else
                {
                }
            }
            catch
            {
            }
            return handle;
        }

        private void Button_On(object sender, RoutedEventArgs e)
        {
            gameclient = GetProcessHandle("League of Legends");

            overlayinstance.CreateOverlay();
            overlayinstance.Myoverlaywin.Create();

            ShowOverlayText = true;
            StopTimer = false;

            overlayinstance.Myoverlaywin.Show();

            TimerCheckWindows();
        }

        private void Button_Off(object sender, RoutedEventArgs e)
        {
            StopTimer = true;

            overlayinstance.Myoverlaywin.Dispose();
            overlayinstance.Mygfx.Dispose();
        }
    }
}
