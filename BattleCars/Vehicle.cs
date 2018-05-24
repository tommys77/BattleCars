using BattleCars.BaseClass;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Threading;

namespace BattleCars
{
    public class Vehicle : ViewModelBase
    {
        public bool IsMovingForward { get; set; }
        public bool IsMovingBackward { get; set; }
        public int RotationDirection { get; set; }
        public Point StartPosition { get; set; }

        public enum Speed { FORWARD = 5, BACKWARD = 2, ROTATION = 3 }

        public enum Size { WIDTH = 20, HEIGHT = 35 }

        public Vehicle()
        {
            VehicleWidth = (int)Size.WIDTH;
            VehicleHeight = (int)Size.HEIGHT;
        }

        private int _angle;
        public int Angle
        {
            get { return _angle; }
            set { SetProperty(ref _angle, value); }
        }


        private Point _location;
        public Point Position
        {
            get { return _location; }
            set { SetProperty(ref _location, value); }
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
            get { return _vehicleImage; }
            set { SetProperty(ref _vehicleImage, value); }
        }

        public void Move()
        {
            if (RotationDirection < 0)
            {
                Angle -= (int)Speed.ROTATION;
            }
            if (RotationDirection > 0)
            {
                Angle += (int)Speed.ROTATION;
            }

            double radians = (Math.PI / 180) * Angle;
            var vector = new Vector() { X = Math.Sin(radians), Y = -Math.Cos(radians) };

            if (IsMovingForward)
            {
                Position += (vector * (int)Speed.FORWARD);
            }
            else if (IsMovingBackward)
            {
                Position -= (vector * (int)Speed.BACKWARD);
            }
        }
    }
}
