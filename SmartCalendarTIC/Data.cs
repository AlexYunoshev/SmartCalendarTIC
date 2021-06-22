using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCalendarTIC
{
    public static class Data
    {
        public static List<Task> tasks = new List<Task>();
        public static List<Task> tasks_my = new List<Task>();

        public static string Text = "";

        public static string GetStringMyTask()
        {
            string s = "";
            foreach (Task t in Data.tasks_my)
            {
                s += "Дисциплина: " + t.Subject + "\n";
                s += "Задание: " + t.TaskTitle + "\n";
                s += "Выполнить до: " + t.DeadLine.ToString() + "\n\n";
            }
            return s;
        }
    }
}
