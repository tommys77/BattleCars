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

        private PlayerControl _p1;
        private PlayerControl _p2;

        private Point p1StartPos;
        private Point p2StartPos;

        private Vehicle player1;
        private Vehicle player2;
        //private string blue = "p1";
        //private string red = "p2";

        private ObservableCollection<Image> players = new ObservableCollection<Image>();

        DispatcherTimer timer = new DispatcherTimer();

        // double p1_X, p1_Y, p2_X, p2_Y;

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

            SetupGame();

        }

        public void SetupGame()
        {
            _p1 = new PlayerControl();
            _p2 = new PlayerControl();

            arena.Children.Add(_p1);
            arena.Children.Add(_p2);

            p1StartPos = new Point(Canvas.GetLeft(p1grid) + (p1grid.Width / 2) - (_p1.vehicle.Width / 2), Canvas.GetTop(p1grid) + (p1grid.Height / 3));
            p2StartPos = new Point(Canvas.GetLeft(p2grid) + (p2grid.Width / 2) - (_p2.vehicle.Width / 2), Canvas.GetTop(p2grid) + (p2grid.Height / 3));

            player1 = new Vehicle()
            {
                Position = p1StartPos,
                VehicleImage = blue_car,
                Angle = 180
            };


            player2 = new Vehicle()
            {
                Position = p2StartPos,
                VehicleImage = red_car,
            };

            _p1.DataContext = player1;
            _p2.DataContext = player2;

            //p1_X = player1.Position.X;
            //p1_Y = player1.Position.Y;
            //p2_X = player2.Position.X;
            //p2_Y = player2.Position.Y;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            SetPositions();

            Canvas.SetLeft(_p1, player1.Position.X);
            Canvas.SetTop(_p1, player1.Position.Y);
            Canvas.SetLeft(_p2, player2.Position.X);
            Canvas.SetTop(_p2, player2.Position.Y);
        }


        //There may be a better way to do this without repeating so much code.
        private void SetPositions()
        {
            //Player 1
            switch (KeyIsPressed.P1_DOWN)
            {
                case true: player1.IsMovingBackward = true; break;
                case false: player1.IsMovingBackward = false; break;
            }

            switch (KeyIsPressed.P1_UP)
            {
                case true: player1.IsMovingForward = true; break;
                case false: player1.IsMovingForward = false; break;
            }

            if (KeyIsPressed.P1_LEFT)
            {
                player1.RotationDirection = -1;
            }
            if (KeyIsPressed.P1_RIGHT)
            {
                player1.RotationDirection = 1;
            }

            if (!KeyIsPressed.P1_LEFT && !KeyIsPressed.P1_RIGHT)
            {
                player1.RotationDirection = 0;
            }

            //Player 2
            switch (KeyIsPressed.P2_DOWN)
            {
                case true: player2.IsMovingBackward = true; break;
                case false: player2.IsMovingBackward = false; break;
            }

            switch (KeyIsPressed.P2_UP)
            {
                case true: player2.IsMovingForward = true; break;
                case false: player2.IsMovingForward = false; break;
            }

            if (KeyIsPressed.P2_LEFT)
            {
                player2.RotationDirection = -1;
            }
            if (KeyIsPressed.P2_RIGHT)
            {
                player2.RotationDirection = 1;
            }
            if (!KeyIsPressed.P2_LEFT && !KeyIsPressed.P2_RIGHT)
            {
                player2.RotationDirection = 0;
            }

            WhenCrossingTheEdge();
        }


        //This method checks the positions further and accordingly creates the illusion of 
        // making the player teleport to the other side if the player goes past the edge
        // of the canvas.

        private void WhenCrossingTheEdge()
        {
            foreach (var player in arena.Children.OfType<PlayerControl>())
            {
                var vehicle = player.DataContext as Vehicle;

                var pos = new Point(vehicle.Position.X, vehicle.Position.Y);

                if (pos.X + _p1.ActualWidth < 0)
                {
                    pos.X = arena.ActualWidth - player.ActualWidth;
                }
                if (pos.X > arena.ActualWidth)
                {
                    pos.X = 0;
                }
                if (pos.Y + _p1.ActualHeight < 0)
                {
                    pos.Y = arena.ActualHeight;
                }
                if (pos.Y > arena.ActualHeight)
                {
                    pos.Y = 0 - player.ActualHeight;
                }

                vehicle.Position = pos;
                vehicle.Move();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            KeyIsPressed.SetTrue(e.Key);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            KeyIsPressed.SetFalse(e.Key);
        }


    }
}
