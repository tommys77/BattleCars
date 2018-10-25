using BattleCars.BaseClass;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media.Imaging;

namespace BattleCars
{
    public class Projectile : ViewModelBase
    {
        public int Player;
        public bool HitSomething = false;
        private readonly Timer timer;

        private int Speed = 3;
        public enum Size {WIDTH = 5, HEIGHT = 10}

        public Projectile()
        {
        }

        private int _projectileAngle;
        public int ProjectileAngle
        {
            get { return _projectileAngle; }
            set { SetProperty(ref _projectileAngle, value); }
        }

        private BitmapImage _projectileImage;
        public BitmapImage ProjectileImage
        {
            get { return _projectileImage; }
            set { SetProperty(ref _projectileImage, value); }
        }

        private Point _position;
        public Point Position
        {
            get { return _position; }
            set { SetProperty(ref _position, value); }
        }

        public void Move()
        {
                double radians = (Math.PI / 180) * ProjectileAngle;
                var vector = new Vector() { X = Math.Sin(radians), Y = -Math.Cos(radians) };

                Position += (vector * Speed);

        }

    }
}
