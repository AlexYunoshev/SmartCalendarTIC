using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using mshtml;
using System.Runtime.Serialization.Json;
using System.IO;

namespace SmartCalendarTIC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                DataContractJsonSerializer jsFormatter = new DataContractJsonSerializer(typeof(List<Task>));
                using (FileStream fs = new FileStream("data.json", FileMode.Open))
                {
                    Data.tasks_my = (List<Task>)jsFormatter.ReadObject(fs);                
                }
            }
            catch
            {
                
            }

           

        }



        private void Calendar_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            GridCursorMenu.Margin = new Thickness(0 + index * 170, -5, 0, 0);
            Main.Content = new Calendar();
        }

        private void Elearn_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            GridCursorMenu.Margin = new Thickness(0 + index * 170, -5, 0, 0);
            Main.Content = new Elearn(Main.ActualHeight, Main.ActualWidth);
        }


        private void Developers_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            GridCursorMenu.Margin = new Thickness(0 + index * 170, -5, 0, 0);
            Main.Content = new PageDevelopers();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            int index = int.Parse(((Button)e.Source).Uid);
            GridCursorMenu.Margin = new Thickness(0 + index * 170, -5, 0, 0);
            Main.Content = new PageAbout();
        }


        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Main.Content = new Calendar();
        }

       
    }
}
