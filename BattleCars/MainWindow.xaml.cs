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

        private BitmapImage red_rocket = new BitmapImage(new Uri(@"pack://siteoforigin:,,,/resources/red_projectile.png"));
        private BitmapImage blue_rocket = new BitmapImage(new Uri(@"pack://siteoforigin:,,,/resources/blue_projectile.png"));

        private PlayerControl _p1;
        private PlayerControl _p2;

        private Point p1StartPos;
        private Point p2StartPos;

        private Vehicle player1;
        private Vehicle player2;

        //private ObservableCollection<Image> players = new ObservableCollection<Image>();
        private ObservableCollection<Projectile> projectiles = new ObservableCollection<Projectile>();
        private Collection<ProjectileController> rocketsGoBoom = new Collection<ProjectileController>();

        DispatcherTimer timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }


        public void InitializeGame()
        {
            _p1 = new PlayerControl();
            _p2 = new PlayerControl();

            arena.Children.Add(_p1);
            arena.Children.Add(_p2);

            p1StartPos = new Point(Canvas.GetLeft(p1grid) + (p1grid.Width / 2) - ((int)Vehicle.Size.WIDTH / 2), Canvas.GetTop(p1grid) + (p1grid.Height / 3));
            p2StartPos = new Point(Canvas.GetLeft(p2grid) + (p2grid.Width / 2) - ((int)Vehicle.Size.WIDTH / 2), Canvas.GetTop(p2grid) + (p2grid.Height / 3));

            player1 = new Vehicle()
            {
                StartPosition = p1StartPos,
                DefaultVehicleImage = blue_car,
                DefaultAngle = 180,
            };


            player2 = new Vehicle()
            {
                StartPosition = p2StartPos,
                DefaultVehicleImage = red_car,
            };

            _p1.DataContext = player1;
            _p2.DataContext = player2;

            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += new EventHandler(TimerTick);
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            SetPositions();

            Canvas.SetLeft(_p1, player1.Position.X);
            Canvas.SetTop(_p1, player1.Position.Y);
            Canvas.SetLeft(_p2, player2.Position.X);
            Canvas.SetTop(_p2, player2.Position.Y);



            foreach (var rocket in rocketsGoBoom)
            {
                arena.Children.Remove(rocket);
            }

            foreach (var rocket in arena.Children.OfType<ProjectileController>())
            {

                var bullet = rocket.DataContext as Projectile;

                Canvas.SetLeft(rocket, bullet.Position.X);
                Canvas.SetTop(rocket, bullet.Position.Y);
            }

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
            foreach (var r in arena.Children.OfType<ProjectileController>())
            {
                var rocket = r.DataContext as Projectile;

                var rPos = new Point(rocket.Position.X, rocket.Position.Y);

                if (rPos.X < 0 || rPos.X > arena.ActualWidth || rPos.Y < 0 || rPos.Y > arena.ActualHeight)
                {
                    rocketsGoBoom.Add(r);
                }
                else
                {
                    rocket.Move();
                }
            }

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
                CollisionDetection(vehicle);

            }

        }



        // Quick and dirty collision detection.

        private void CollisionDetection(Vehicle vehicle)
        {
            var activePlayers = arena.Children.OfType<PlayerControl>();
            var arsenal = arena.Children.OfType<ProjectileController>();

            Rect rect1 = new Rect { Width = vehicle.VehicleWidth, Height = vehicle.VehicleHeight, Location = vehicle.Position };

            bool crashed = false;
            //Check if colliding with other player
            foreach (var other in activePlayers)
            {
                Vehicle rival = null;

                if (other.DataContext != vehicle)
                {
                    rival = other.DataContext as Vehicle;

                    Rect rect2 = new Rect { Width = rival.VehicleWidth, Height = rival.VehicleHeight, Location = rival.Position };

                    if (rect1.IntersectsWith(rect2))
                    {
                        vehicle.Exploded = true;
                    }
                }
            }
            //Check if hit by rocket
            if (!crashed)
            {
                foreach (var rocket in arsenal)
                {
                    var rock = rocket.DataContext as Projectile;

                    if (vehicle == player1 && rock.Player == 2)
                    {
                        Rect rect2 = new Rect { Width = rocket.ActualWidth, Height = rocket.ActualHeight, Location = rock.Position };

                        if (rect1.IntersectsWith(rect2))
                        {
                            rocketsGoBoom.Add(rocket);
                            vehicle.Exploded = true;
                        }
                    }
                    else if (vehicle == player2 && rock.Player == 1)
                    {
                        Rect rect2 = new Rect { Width = rocket.ActualWidth, Height = rocket.ActualHeight, Location = rock.Position };

                        if (rect1.IntersectsWith(rect2))
                        {
                            rocketsGoBoom.Add(rocket);
                            vehicle.Exploded = true;
                        }
                    }
                }
            }
            vehicle.Move();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {



            KeyIsPressed.SetTrue(e.Key);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            KeyIsPressed.SetFalse(e.Key);

            var key = e.Key;

            if (key == Key.Space)
            {
                FireWeapon("P1");
            }

            if ((key == Key.Enter))
            {
                FireWeapon("P2");
            }
        }

        private void FireWeapon(string _p)
        {
            Vehicle player = null;
            PlayerControl _player = null;
            int shooter = 0;
            BitmapImage projectileImage = null;

            switch (_p)
            {
                case "P1":
                    player = player1;
                    _player = _p1;
                    shooter = 1;
                    projectileImage = red_rocket;

                    break;
                case "P2":
                    player = player2;
                    _player = _p2;
                    shooter = 2;
                    projectileImage = blue_rocket;
                    break;
            }

            var _bullet = new ProjectileController();

            var posX = Canvas.GetLeft(_player) - (-1 * (_player.ActualWidth / 2));
            var posY = Canvas.GetTop(_player) - (-1 * (_player.ActualHeight / 2));

            //Projectile from P1
            var bullet = new Projectile()
            {
                Player = shooter,
                Position = new Point(posX, posY),
                ProjectileAngle = player.Angle,
                ProjectileImage = projectileImage
            };



            _bullet.DataContext = bullet;
            arena.Children.Add(_bullet);


            Canvas.SetLeft(_bullet, bullet.Position.X);
            Canvas.SetTop(_bullet, bullet.Position.Y);


        }



    }
}
