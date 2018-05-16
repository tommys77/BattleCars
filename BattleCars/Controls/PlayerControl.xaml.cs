using System;
using System.Collections.Generic;
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

namespace BattleCars.Controls
{
    /// <summary>
    /// Interaction logic for Player1Control.xaml
    /// </summary>
    public partial class PlayerControl : UserControl
    {
        public enum Speed
        {
            FORWARD = 3,
            BACKWARD = 1,
            LEFT = 1,
            RIGHT = 1
        }

        public PlayerControl()
        {
            InitializeComponent();
        }
    }
}
