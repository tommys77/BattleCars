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

        private Vehicle player1;
        private Vehicle player2;
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

            //CreatePlayers();
            SetupGame();

        }

        public void SetupGame()
        {
            _p1 = new PlayerControl();
            _p2 = new PlayerControl();

            arena.Children.Add(_p1);
            arena.Children.Add(_p2);

            player1 = new Vehicle()
            {
                VehicleImage = blue_car,
            };

            player2 = new Vehicle()
            {
                VehicleImage = red_car,
            };

            _p1.DataContext = player1;
            _p2.DataContext = player2;

            p1_X = player1.Location.X;
            p1_Y = player1.Location.Y;
            p2_X = player2.Location.X;
            p2_Y = player2.Location.Y;



        }

        private void TimerTick(object sender, EventArgs e)
        {
            RedrawCars();
        }

        private void RedrawCars()
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
                player1.Angle -= (int)Vehicle.Speed.LEFT;
            }
            if (KeyIsPressed.P1_RIGHT)
            {
                player1.Angle += (int)Vehicle.Speed.RIGHT;
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
                //p2_X -= (double)Vehicle.Speed.LEFT;
                player2.Angle -= (int)Vehicle.Speed.LEFT;
            }
            if (KeyIsPressed.P2_RIGHT)
            {
                //p2_X += (double)Vehicle.Speed.RIGHT;
                player2.Angle += (int)Vehicle.Speed.RIGHT;
            }

            Move(player1, _p1);
            Move(player2, _p2);
        }

        private void FurtherAdjustPostions()
        {
            //Player 1
            var _1pos = new Point(player1.Location.X, player1.Location.Y);

            //Point _1pos = new Point(Canvas.GetLeft(_p1.Car), Canvas.GetTop(_p1.Car));

            if (_1pos.X + _p1.ActualWidth < 0)
            {
                _1pos.X = arena.ActualWidth - _p1.ActualWidth;
            }
            if (_1pos.X > arena.ActualWidth)
            {
                _1pos.X = 0;
            }
            if (_1pos.Y + _p1.ActualHeight < 0)
            {
                _1pos.Y = arena.ActualHeight;
            }
            if (_1pos.Y > arena.Height)
            {
                _1pos.Y = 0 - _p1.ActualHeight;
            }

            player1.Location = _1pos;

            //Player 2

            var _2pos = new Point(player2.Location.X, player2.Location.Y);

            if (_2pos.X + _p2.ActualWidth < 0)
            {
                _2pos.X = arena.ActualWidth - _p2.ActualWidth;
            }
            if (_2pos.X > arena.ActualWidth)
            {
                _2pos.X = 0;
            }
            if (_2pos.Y + _p2.ActualHeight < 0)
            {
                _2pos.Y = arena.ActualHeight;
            }
            if (_2pos.Y > arena.ActualHeight)
            {
                _2pos.Y = 0 - _p2.ActualHeight;
            }

            player2.Location = _2pos;


        }

        private void Move(Vehicle vehicle, PlayerControl playerControl)
        {

            double radians = (Math.PI / 180) * vehicle.Angle;
            var vector = new Vector() { X = Math.Sin(radians), Y = -Math.Cos(radians) };

            //vehicle.Location = new Point(Canvas.GetLeft(playerControl), Canvas.GetTop(playerControl));

            if (vehicle.IsMovingForward)
            {
                vehicle.Location += (vector * (int)Vehicle.Speed.FORWARD);
            }
            else if (vehicle.IsMovingBackward)
            {
                vehicle.Location -= (vector * (int)Vehicle.Speed.BACKWARD);
            }

            FurtherAdjustPostions();

            Canvas.SetLeft(playerControl, vehicle.Location.X);
            Canvas.SetTop(playerControl, vehicle.Location.Y);

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
