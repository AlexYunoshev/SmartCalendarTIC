using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
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
    /// Interaction logic for Calendar.xaml
    /// </summary>
    public partial class Calendar : Page
    {

        public Calendar()
        {
            InitializeComponent();
        }
        DateTime date;
        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            date = DateTime.Now;
            TextBlockDateNow.Text = date.ToLongDateString();
            TextBlockDate.Text = date.Month.ToString() + "." + date.Year.ToString();
            Main.Content = new PageMonth(date);
            HTMLCode.Text = Data.Text;
            MyTaskList.Text = Data.GetStringMyTask();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            date = date.AddMonths(-1);
            Main.Content = new PageMonth(date);
            TextBlockDate.Text = date.Month.ToString() + "." + date.Year.ToString();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            date = date.AddMonths(1);
            Main.Content = new PageMonth(date);
            TextBlockDate.Text = date.Month.ToString() + "." + date.Year.ToString();
        }

        private void btnToday_Click(object sender, RoutedEventArgs e)
        {
            date = DateTime.Now;
            Main.Content = new PageMonth(date);
            TextBlockDate.Text = date.Month.ToString() + "." + date.Year.ToString();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime date = new DateTime();
                date = newDeadLine.SelectedDate.Value;
                DateTime date1 = new DateTime(date.Year, date.Month, date.Day, Convert.ToInt32(newTaskHour.Text), Convert.ToInt32(newTaskMinute.Text), 00); // год - месяц - день - час - минута - секунда
                Task t = new Task(newSubject.Text, newTaskTitle.Text, date1);
                Data.tasks_my.Add(t);

                DataContractJsonSerializer jsFormatter = new DataContractJsonSerializer(typeof(List<Task>));
                using (FileStream fs = new FileStream("data.json", FileMode.OpenOrCreate))
                {

                    jsFormatter.WriteObject(fs, Data.tasks_my);
                }
                Main.Content = new PageMonth(date);
                MyTaskList.Text = Data.GetStringMyTask();
                newSubject.Text = "";
                newTaskTitle.Text = "";
            }
            catch
            {

            }
           

        }

        private void SelectData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ComboBoxTasks.Items.Clear();
                var m = Data.tasks_my.Where(p => p.DeadLine.Date == DeadLine.SelectedDate);
                foreach (Task i in m)
                {
                    ComboBoxTasks.Items.Add(i.Subject.ToString() + ": " + i.TaskTitle.ToString() + " сдать до " + i.DeadLine.ToShortTimeString());
                }
                ComboBoxTasks.SelectedIndex = 0;
            }
            catch
            {

            }
           
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var m = Data.tasks_my.Where(p => p.DeadLine.Date == DeadLine.SelectedDate).ToList();
                Data.tasks_my.Remove(m[ComboBoxTasks.SelectedIndex]);
                DataContractJsonSerializer jsFormatter = new DataContractJsonSerializer(typeof(List<Task>));
                using (FileStream fs = new FileStream("data.json", FileMode.Create))
                {
                    
                    jsFormatter.WriteObject(fs, Data.tasks_my);
                }
                Main.Content = new PageMonth(date);
                MyTaskList.Text = Data.GetStringMyTask();
                ComboBoxTasks.Items.Clear();
            }
            catch
            {

            }
            
        }
    }
}
