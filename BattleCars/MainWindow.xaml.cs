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

namespace BattleCars
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private BitmapImage blue_car = new BitmapImage(new Uri(@"pack://siteoforigin:,,,/resources/blue_car.png"));
        private BitmapImage red_car = new BitmapImage(new Uri(@"pack://siteoforigin:,,,/resources/red_car.png"));

        private string blue = "p1";
        private string red = "p2";

        private ObservableCollection<Image> players = new ObservableCollection<Image>();

        DispatcherTimer timer = new DispatcherTimer();

        double p1_X, p1_Y, p2_X, p2_Y;

        private enum Direction
        {
            FORWARD = 3,
            BACKWARD = 1,
            LEFT = 1,
            RIGHT = 1
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        public void InitializeGame()
        {
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += new EventHandler(TimerTick);

            //Set start positions
            p1_X = Canvas.GetLeft(p1_grid);
            p1_Y = Canvas.GetTop(p1_grid);
            p2_X = Canvas.GetLeft(p2_grid);
            p2_Y = Canvas.GetTop(p2_grid);

            CreatePlayers();
            SetupGame();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            var cars = stadium.Children.OfType<Image>();

            foreach (var car in cars)
            {
                var xPos = Canvas.GetLeft(car);
                var yPos = Canvas.GetTop(car);

                switch (car.Name)
                {
                    case "p1":
                        if (xPos + car.Width < 0)
                        {
                            p1_X = stadium.Width - car.Width;
                        }
                        if (xPos > stadium.Width)
                        {
                            p1_X = 0;
                        }
                        if(yPos + car.Height < 0)
                        {
                            p1_Y = stadium.Height;
                        }
                        if(yPos > stadium.Height)
                        {
                            p1_Y = 0;
                        }
                        Canvas.SetLeft(car, p1_X);
                        Canvas.SetTop(car, p1_Y);
                        break;
                    case "p2":
                        Canvas.SetLeft(car, p2_X);
                        Canvas.SetTop(car, p2_Y);
                        break;
                }
            }
        }

        private void CreatePlayers()
        {
            //width and height
            var w = 40;
            var h = 60;

            var p1 = new Image() { Width = w, Height = h };
            var p2 = new Image() { Width = w, Height = h };
            p1.Source = blue_car;
            p1.Name = blue;
            p2.Source = red_car;
            p2.Name = red;

            stadium.Children.Add(p1);
            stadium.Children.Add(p2);
        }

        public void SetupGame()
        {
            timer.Start();
        }



        private void ControlTheCar(object sender, KeyEventArgs e)
        {
            var key = e.Key;

            switch (key)
            {
                case Key.W:
                    p1_Y -= (double)Direction.FORWARD;
                    break;
                case Key.A:
                    p1_X -= (double)Direction.LEFT;
                    break;
                case Key.S:
                    p1_Y += (double)Direction.BACKWARD;
                    break;
                case Key.D:
                    p1_X += (double)Direction.RIGHT;
                    break;
            }
        }
    }
}
