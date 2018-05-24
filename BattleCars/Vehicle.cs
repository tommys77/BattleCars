using BattleCars.BaseClass;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Timers;

namespace BattleCars
{
    public class Vehicle : ViewModelBase
    {
        public bool IsMovingForward { get; set; }
        public bool IsMovingBackward { get; set; }
        public int RotationDirection { get; set; }

        public BitmapImage DefaultVehicleImage { get; set; }
        private BitmapImage _explosionImage = new BitmapImage(new Uri(@"pack://siteoforigin:,,,/resources/explosion.png"));


        public bool Exploded;

        public enum Speed { FORWARD = 5, BACKWARD = 2, ROTATION = 3 }
        public enum Size { WIDTH = 20, HEIGHT = 35 }

        private Timer timer;


        public Vehicle()
        {
            timer = new Timer(3000);
            timer.AutoReset = false;
            VehicleWidth = (int)Size.WIDTH;
            VehicleHeight = (int)Size.HEIGHT;
        }

        private int _angle;
        public int Angle
        {
            get
            {
                return _angle;
            }
            set { SetProperty(ref _angle, value); }
        }

        private int _defaultAngle;
        public int DefaultAngle
        {
            get { return _defaultAngle; }
            set { _defaultAngle = value; Angle = value; }
        }

        private Point _position;
        public Point Position
        {
            get
            {
                return _position;
            }
            set { SetProperty(ref _position, value); }
        }

        private Point _startPosition;
        public Point StartPosition
        {
            get
            { return _startPosition; }
            set
            {
                _startPosition = value;
                Position = value;
            }
        }

        private int _vehicleWidth;
        public int VehicleWidth
        {
            get { return _vehicleWidth; }
            set { SetProperty(ref _vehicleWidth, value); }
        }

        private int _vehicleHeight;
        public int VehicleHeight
        {
            get { return _vehicleHeight; }
            set { SetProperty(ref _vehicleHeight, value); }
        }

        private BitmapImage _vehicleImage;

        public BitmapImage VehicleImage
        {
            get
            {
                if (_vehicleImage != null) return _vehicleImage;
                else return DefaultVehicleImage;
            }
            set { SetProperty(ref _vehicleImage, value); }
        }

        public void Move()
        {

            if (RotationDirection < 0 && !Exploded)
            {
                Angle -= (int)Speed.ROTATION;
            }
            if (RotationDirection > 0 && !Exploded)
            {
                Angle += (int)Speed.ROTATION;
            }

            double radians = (Math.PI / 180) * Angle;
            var vector = new Vector() { X = Math.Sin(radians), Y = -Math.Cos(radians) };

            if (IsMovingForward && !Exploded)
            {
                Position += (vector * (int)Speed.FORWARD);
            }
            else if (IsMovingBackward && !Exploded)
            {
                Position -= (vector * (int)Speed.BACKWARD);
            }

            if (Exploded)
            {
                Explosion();
            }
        }

        private void Explosion()
        {
            VehicleImage = _explosionImage;
            VehicleWidth = (int)Size.WIDTH * 2;
            VehicleHeight = (int)Size.HEIGHT * 2;
            CountDownToReset();
        }

        private void CountDownToReset()
        {
            Exploded = false;
            timer.Elapsed += new ElapsedEventHandler(ResetVehicle);
            timer.Enabled = true;
            timer.Start();
        }

        private void ResetVehicle(object sender, ElapsedEventArgs e)
        {
            Position = StartPosition;
            Angle = DefaultAngle;
            VehicleImage = DefaultVehicleImage;
            VehicleHeight = (int)Size.HEIGHT;
            VehicleWidth = (int)Size.WIDTH;
            timer.Enabled = false;
            timer.Stop();
        }
    }

}
