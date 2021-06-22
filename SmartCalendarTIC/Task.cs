using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmartCalendarTIC
{
    [DataContract]
    public class Task
    {
        private string sbj;
        private string tName;
        private DateTime date;
        private string status;
        public int min { get; set; }
        public int hour { get; set; }



        /// <summary>
        /// Название предмета
        /// </summary>
        [DataMember]
        public string Subject
        {
            get { return sbj; }
            set { sbj = value; }
        }

        /// <summary>
        /// Название задания 
        /// </summary>
        [DataMember]
        public string TaskTitle
        {
            get { return tName; }
            set { tName = value; }
        }

        /// <summary>
        /// Срок сдачи
        /// </summary>
        [DataMember]
        public DateTime DeadLine
        {
            get { return date; }
            set { date = value; }
        }

        /// <summary>
        /// Теущий статус
        /// </summary>
        [DataMember]
        public string Status
        {
            get
            {
                return status;
            }
            set { status = value; }
        }

        public Task() : this("Unknown subject", "Uncnown work", DateTime.MinValue) { }

        public Task(string subj, string taskName, DateTime dueTo)
        {
            Subject = subj;
            TaskTitle = taskName;
            DeadLine = dueTo;
        }

    }
}
