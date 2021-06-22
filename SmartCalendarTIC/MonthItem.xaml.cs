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

namespace SmartCalendarTIC
{
    /// <summary>
    /// Interaction logic for MonthItem.xaml
    /// </summary>
    public partial class MonthItem : UserControl
    {
        public MonthItem()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string DayOfMonth { get; set; }

        public SolidColorBrush BacgroundColor { get; set; }
        public SolidColorBrush BorderColor { get; set; }
        public int BorderWidth { get; set; }
    }
}
