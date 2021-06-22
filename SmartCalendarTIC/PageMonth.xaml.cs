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
    /// Interaction logic for PageMonth.xaml
    /// </summary>
    public partial class PageMonth : Page
    {
        private DateTime dateFromMain;
        private DateTime dateCopy;

        private int columnStart = 0;
        private int rowStart = 1;
        private int rowNumbers;
        private int currentMonth;
       


        public PageMonth(DateTime date)
        {
            InitializeComponent();
            dateFromMain = date;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            dateCopy = new DateTime(dateFromMain.Year, dateFromMain.Month, 1);
            currentMonth = dateCopy.Month;
            int j = ColumnStart(dateCopy);
            rowNumbers = 0;

            while (currentMonth == dateCopy.Month)
            {
                var rowDefenition = new RowDefinition();
                MainGrid.RowDefinitions.Add(rowDefenition);
                rowNumbers++;

                while (j < 7)
                {
                    dateCopy = dateCopy.AddDays(1);
                    j++;
                }
                j = 0;
            }

            dateCopy = new DateTime(dateFromMain.Year, dateFromMain.Month, 1);

            columnStart = ColumnStart(dateCopy);
            j = columnStart;
            currentMonth = dateCopy.Month;


            for (int i = rowStart; i <= rowNumbers; i++)
            {
                while (j < 7)
                {
                    if (currentMonth == dateCopy.Month) // тоді це цей місяць
                    {
                        if (dateCopy.Date == DateTime.Today)
                            AddItem(dateCopy.Day.ToString(), i, j, (SolidColorBrush)new BrushConverter().ConvertFrom("#c9efff"), Brushes.Black, 1, Data.tasks, dateCopy, Data.tasks_my);
                        else
                            AddItem(dateCopy.Day.ToString(), i, j, Brushes.White, Brushes.Black, 1, Data.tasks, dateCopy, Data.tasks_my);
                    }
                    else // тоді це наст місяць
                    {
                        AddItem(dateCopy.Day.ToString(), i, j, (SolidColorBrush)new BrushConverter().ConvertFrom("#ced4d6"), Brushes.Black, 1, Data.tasks, dateCopy, Data.tasks_my);
                    }
                    dateCopy = dateCopy.AddDays(1);
                    j++;
                }
                j = 0;
            }

            if (columnStart != 0)
            {
                dateCopy = new DateTime(dateFromMain.Year, dateFromMain.Month, 1);
                dateCopy = dateCopy.AddDays(-columnStart); // -3 дні
                for (int i = 0; i < columnStart; i++) // + 3 дні
                {
                    AddItem(dateCopy.Day.ToString(), 1, i, (SolidColorBrush)new BrushConverter().ConvertFrom("#ced4d6"), Brushes.Black, 1, Data.tasks, dateCopy, Data.tasks_my);
                    dateCopy = dateCopy.AddDays(1);
                }
            }

        }


        private void AddItem(string Day, int Row, int Column, SolidColorBrush background,
            SolidColorBrush border, int width, List<Task> tasks, DateTime date, List<Task> tasksmy)
        {
            string contentText = "";

            var q = tasks.Where(p => p.DeadLine.Date == date.Date);
            var m = tasksmy.Where(p => p.DeadLine.Date == date.Date);


            MonthItem n = new MonthItem();

            if (q != null)
            {
                foreach (Task t in q) {
                    contentText = "Дисциплина: " + t.Subject + "\n";
                    contentText += "Задание: " + t.TaskTitle + "\n";
                    contentText += "Сдать до: " + t.DeadLine.ToShortTimeString() + "\n";
                    TextBlock tb = new TextBlock();
                    tb.Margin = new Thickness(5, 5, 5, 5);
                    tb.Text = contentText;
                    n.Main.Children.Add(tb);     
                }
            }

            if (m != null)
            {
                foreach (Task t in m)
                {
                    contentText = "Дисциплина: " + t.Subject + "\n";
                    contentText += "Задание: " + t.TaskTitle + "\n";
                    contentText += "Сдать до: " + t.DeadLine.ToShortTimeString() + "\n";
                    TextBlock tb = new TextBlock();
                    tb.Margin = new Thickness(5, 5, 5, 5);
                    tb.Text = contentText;
                    n.Main.Children.Add(tb);
                }
            }



            n.DayOfMonth = date.Day.ToString();
           
            n.BacgroundColor = background;
            n.BorderColor = border;
            n.BorderWidth = width;

            MainGrid.Children.Add(n);
            Grid.SetRow(n, Row);
            Grid.SetColumn(n, Column);
        }

        private int ColumnStart(DateTime dateFromMain)
        {
            switch (dateFromMain.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return 0;
                case DayOfWeek.Tuesday:
                    return 1;
                case DayOfWeek.Wednesday:
                    return 2;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Friday:
                    return 4;
                case DayOfWeek.Saturday:
                    return 5;
                case DayOfWeek.Sunday:
                    return 6;
            }
            return -1;
        }


    }
}
