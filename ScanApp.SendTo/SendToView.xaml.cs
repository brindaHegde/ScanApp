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
using System.Windows.Shapes;

namespace ScanApp.SendTo
{
    /// <summary>
    /// Interaction logic for SendToView.xaml
    /// </summary>
    public partial class SendToView : Window
    {
        public SendToView()
        {
            this.DataContext = new SendToViewModel(this);
            InitializeComponent();
        }
    }
}
