using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;
using BattleCars.Controls;

namespace BattleCars
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private BitmapImage blue_car = new BitmapImage(new Uri(@"pack://siteoforigin:,,,/resources/blue_car.png"));
        private BitmapImage red_car = new BitmapImage(new Uri(@"pack://siteoforigin:,,,/resources/red_car.png"));

        private Player1 P1;
        private Player2 P2;

        //private string blue = "p1";
        //private string red = "p2";

        private ObservableCollection<Image> players = new ObservableCollection<Image>();

        DispatcherTimer timer = new DispatcherTimer();

        double p1_X, p1_Y, p2_X, p2_Y;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        public void InitializeGame()
        {
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += new EventHandler(TimerTick);
            timer.Start();

            //Set start positions
            p1_X = Canvas.GetLeft(p1_grid);
            p1_Y = Canvas.GetTop(p1_grid);
            p2_X = Canvas.GetLeft(p2_grid);
            p2_Y = Canvas.GetTop(p2_grid);

            //CreatePlayers();
            SetupGame();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            RedrawCars();
        }

        private void RedrawCars()
        {
            //Player 1
            if (KeyIsPressed.P1_DOWN)
            {
                p1_Y += (double)PlayerControl.Speed.BACKWARD;
            }
            if (KeyIsPressed.P1_UP)
            {
                p1_Y -= (double)PlayerControl.Speed.FORWARD;
            }
            if (KeyIsPressed.P1_LEFT)
            {
                p1_X -= (double)PlayerControl.Speed.LEFT;
            }
            if (KeyIsPressed.P1_RIGHT)
            {
                p1_X += (double)PlayerControl.Speed.RIGHT;
            }


            //Player 2
            if (KeyIsPressed.P2_DOWN)
            {
                p2_Y += (double)PlayerControl.Speed.BACKWARD;
            }
            if (KeyIsPressed.P2_UP)
            {
                p2_Y -= (double)PlayerControl.Speed.FORWARD;
            }
            if (KeyIsPressed.P2_LEFT)
            {
                p2_X -= (double)PlayerControl.Speed.LEFT;
            }
            if (KeyIsPressed.P2_RIGHT)
            {
                p2_X += (double)PlayerControl.Speed.RIGHT;
            }

            FurtherAdjustPostions();
        }

        private void FurtherAdjustPostions()
        {
            //Player 1
            if (p1_X + P1.ActualWidth < 0)
            {
                p1_X = arena.Width - P1.ActualWidth;
            }
            if (p1_X > arena.Width)
            {
                p1_X = 0;
            }
            if (p1_Y + P1.ActualHeight < 0)
            {
                p1_Y = arena.Height;
            }
            if (p1_Y > arena.Height)
            {
                p1_Y = 0 - P1.ActualHeight;
            }

            //Player 2
            if (p2_X + P2.ActualWidth < 0)
            {
                p2_X = arena.Width - P2.ActualWidth;
            }
            if (p2_X > arena.Width)
            {
                p2_X = 0;
            }
            if (p2_Y + P2.ActualHeight < 0)
            {
                p2_Y = arena.Height;
            }
            if (p2_Y > arena.Height)
            {
                p2_Y = 0 - P2.ActualHeight;
            }

            Canvas.SetLeft(P1, p1_X);
            Canvas.SetTop(P1, p1_Y);
            Canvas.SetLeft(P2, p2_X);
            Canvas.SetTop(P2, p2_Y);
        }

        public void SetupGame()
        {
            P1 = new Player1();
            P2 = new Player2();
            arena.Children.Add(P1);
            arena.Children.Add(P2);
        }

        private void ControlTheCar(object sender, KeyEventArgs e)
        {
            KeyIsPressed.SetTrue(e.Key);
        }

        private void KeyReleased(object sender, KeyEventArgs e)
        {
            KeyIsPressed.SetFalse(e.Key);
        }
    }
}
