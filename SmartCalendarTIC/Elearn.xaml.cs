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

namespace SmartCalendarTIC
{
    /// <summary>
    /// Interaction logic for Elearn.xaml
    /// </summary>
    public partial class Elearn : Page
    {

        private List<string> subj_links = new List<string>();
        private Dictionary<string, string> subj_names = new Dictionary<string, string>();


        DispatcherTimer dispTimer = new DispatcherTimer();

        int taskNumber = 0;
        int subj_number = 0;
        int iframecount = 0;
        int links_number = 0;

        public Elearn(double height, double width)
        {
            
            InitializeComponent();
            WBElearn.Height = height;
            WBElearn.Width = width;

            dispTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            PBar.Value = 0;

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            switch (taskNumber)
            {
                case 0: NextSubj(); break;
                case 1: GetAssign(); break;
                case 2: GetInner(); break;
                case 3: GetTasks(); break;
                default: GoToURL("https://elearn.csn.khai.edu/xsl-portal"); taskNumber = -1; iframecount = 0; links_number--; break;
            }
            if (links_number == 0)
            {
                dispTimer.Stop();
                foreach (Task t in Data.tasks)
                {
                    Data.Text += "Дисциплина: " + t.Subject + "\n";
                    Data.Text += "Задание: " + t.TaskTitle + "\n";
                    Data.Text += "Выполнить до: " + t.DeadLine.ToString() + "\n\n";
                }
            }
            taskNumber++;
            // Forcing the CommandManager to raise the RequerySuggested event
            CommandManager.InvalidateRequerySuggested();
            PBar.Value++;
            
        }

        private void GetLink_Click(object sender, RoutedEventArgs e)
        {
            taskNumber = 0;
            iframecount = 0;
            subj_number = 0;
            GetSubj();
            links_number = subj_links.Count;
            PBar.Maximum = 5 * links_number;
            PBar.Value = 0;
            Data.tasks.Clear();
            dispTimer.Start();

        }

        //Получить ссылки на предметы
        private void GetSubj()
        {
            try
            {
                HTMLDocument a = (HTMLDocument)WBElearn.Document;
                IHTMLElement c = a.documentElement;
                subj_links.Clear();
                subj_names.Clear();
                //Формирование ссылок на предметы
                IHTMLElement ul = SearchID(c, "quickLinks");
                //Выделение ссылок на предметы

                foreach (IHTMLElement li_el in ul.children)
                {
                    foreach (IHTMLElement a_el in li_el.children)
                    {
                        if (a_el.tagName.ToUpper() == "A")
                        {
                            subj_links.Add(a_el.getAttribute("href"));
                            subj_names.Add(a_el.getAttribute("href"), a_el.innerText);
                        }
                    }
                }
                subj_links.Remove(subj_links.Last());
            }
            catch (Exception) { }
        }


        //Переход на новый предмет
        private void NextSubj()
        {
            try
            {
                string url = subj_links[subj_number];
                GoToURL(url);
                subj_number++;
                subj_number = (subj_number >= subj_links.Count) ? 0 : subj_number;
            }
            catch (Exception) { }
        }


        //Перейти на вкладку Assignements
        private void GetAssign()
        {
            try
            {
                //Получение ссылки на список с Assignment
                IHTMLElement ul = SearchID(((HTMLDocument)WBElearn.Document).documentElement, "toolSubMenuRS");
                //Поиск ссылки на Assignement
                bool gotcha = false;
                string link = "";
                foreach (IHTMLElement li in ul.children)
                {
                    foreach (IHTMLElement a in li.children)
                    {
                        if (a.tagName.ToUpper() == "A")
                        {
                            foreach (IHTMLElement span in a.children)
                            {
                                if (span.innerText == "Assignments")
                                {
                                    link = a.getAttribute("href");
                                    gotcha = true;
                                    break;
                                }
                            }
                        }
                        else if (a.tagName.ToUpper() == "SPAN")
                        {
                            if (a.innerText == "Assignments")
                            {
                                link = WBElearn.Source.ToString();
                                gotcha = true;
                                break;
                            }
                        }
                        if (gotcha) { break; }
                    }
                    if (gotcha) { break; }
                }
                //Переход на assignments
                GoToURL(link);
            }
            catch (Exception) { }
        }


        //Перейти внутрь вкладки Assignements
        private void GetInner()
        {
            try
            {
                string url = FindFrame(((HTMLDocument)WBElearn.Document).documentElement).getAttribute("src");
                GoToURL(url);
            }
            catch (Exception) { }
        }
        //Получить задания
        private void GetTasks()
        {
            try
            {
                IHTMLElement table = FindTable(((HTMLDocument)WBElearn.Document).documentElement);
                Task newT = new Task();
                int count = 0;
                bool add = false;
                //Проход по строкам таблицы
                foreach (IHTMLElement tr in table.children)
                {
                    foreach (IHTMLElement tbody in tr.children)
                    {
                        add = false;
                        count = 0;
                        newT = new Task();
                        newT.Subject = subj_names[subj_links[(subj_number == 0)? subj_links.Count - 1 : subj_number - 1]];
                        foreach (IHTMLElement td in tbody.children)
                        {
                            if (td.tagName == "TD")
                            {
                                add = true;
                                switch (count)
                                {
                                    case 0: break;
                                    case 1: newT.TaskTitle = td.innerText; break;
                                    case 2: break;
                                    case 3: newT.Status = td.innerText; break;
                                    case 4: break;
                                    case 5: newT.DeadLine = FormDateTime(td.innerText); break;
                                    default: break;
                                }
                                count++;
                            }
                        }
                        if (add) { 
                            if(newT.Status.ToLower() == "not started ")
                            {
                                Data.tasks.Add(newT);
                            }
                            
                        }
                    }
                }
            }
            catch (Exception) { }
        }



        //===========================================================================================
        //Поиск фрейма с заданиями
        private IHTMLElement FindFrame(IHTMLElement parent)
        {
            IHTMLElement rez = null;
            foreach (IHTMLElement ch_el in parent.children)
            {
                if ((ch_el.tagName).ToUpper() == "IFRAME")
                {
                    if (iframecount != 0) { rez = ch_el; }
                    iframecount++;

                }
                else
                {
                    rez = FindFrame(ch_el);
                }
                if (rez != null) { break; }
            }
            return rez;
        }



        /// <summary>
        /// Поиск элемента с заданым id среди дочерних
        /// </summary>
        /// <param name="parent">родительский элемент</param>
        /// <param name="cls">class искомого элемента</param>
        /// <returns>Элемент с заданым class</returns>
        private IHTMLElement SearchClass(IHTMLElement parent, string cls)
        {
            IHTMLElement rez = null;
            foreach (IHTMLElement ch_el in parent.children)
            {
                if ((ch_el.getAttribute("class") ?? "unknown") == cls)
                {
                    rez = ch_el;
                }
                else
                {
                    rez = SearchClass(ch_el, cls);
                }
                if (rez != null) { break; }
            }
            return rez;
        }

        /// <summary>
        /// Поиск элемента с заданым id среди дочерних
        /// </summary>
        /// <param name="parent">родительский элемент</param>
        /// <param name="id">id искомого элемента</param>
        /// <returns>Элемент с заданым id</returns>
        private IHTMLElement SearchID(IHTMLElement parent, string id)
        {
            IHTMLElement rez = null;
            foreach (IHTMLElement ch_el in parent.children)
            {
                if (ch_el.id == id)
                {
                    rez = ch_el;
                }
                else
                {
                    rez = SearchID(ch_el, id);
                }
                if (rez != null) { break; }
            }
            return rez;
        }

        /// <summary>
        /// Формирования даты
        /// </summary>
        /// <param name="str">строчное представление даты</param>
        /// <returns>Дата</returns>
        private DateTime FormDateTime(string str)
        {
            DateTime dt = new DateTime();
            int year = 0;
            int day = 0;
            int month = 1;
            int hours = 0;
            int minutes = 0;
            int seconds = 0;
            string pattern = @"^(?<month>\S*?)\s(?<day>\d+),\s(?<year>\d+)\s(?<hours>\d+):(?<minutes>\d+)\s(?<timeChange>pm|am)\s$";
            Match m = Regex.Match(str, pattern);
            year = Convert.ToInt32(m.Groups["year"].Value);
            day = Convert.ToInt32(m.Groups["day"].Value);
            switch (m.Groups["month"].Value)
            {
                case "Jan": month = 1; break;
                case "Feb": month = 2; break;
                case "Mar": month = 3; break;
                case "Apr": month = 4; break;
                case "May": month = 5; break;
                case "Jun": month = 6; break;
                case "Jul": month = 7; break;
                case "Aug": month = 8; break;
                case "Sep": month = 9; break;
                case "Oct": month = 10; break;
                case "Nov": month = 11; break;
                case "Dec": month = 12; break;
                default: break;
            }
            hours = Convert.ToInt32(m.Groups["hours"].Value) + ((m.Groups["timeChange"].Value == "am") ? 12 : 0);
            minutes = Convert.ToInt32(m.Groups["minutes"].Value);
            dt = new DateTime(year, month, day, hours, minutes, seconds);
            return dt;
        }

        /// <summary>
        /// Поиск таблицы с заданиями
        /// </summary>
        /// <param name="parent">стартовый эелемент поиска</param>
        /// <returns>указатель на таблицу</returns>
        private IHTMLElement FindTable(IHTMLElement parent)
        {
            IHTMLElement rez = null;
            foreach (IHTMLElement ch_el in parent.children)
            {
                if (ch_el.tagName.ToUpper() == "TABLE")
                {
                    rez = ch_el;
                }
                else
                {
                    rez = FindTable(ch_el);
                }
                if (rez != null) { break; }
            }
            return rez;
        }

        /// <summary>
        /// Перейти на URL
        /// </summary>
        /// <param name="url">URL--ссылка</param>
        private void GoToURL(string url)
        {
            WBElearn.Navigate(url);
        }
    }
}
