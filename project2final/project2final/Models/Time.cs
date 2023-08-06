using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project2final.Models
{
    public class Time : INotifyPropertyChanged
    {
        private DateTime _date;
        private string _narrative;
        private int _hours;
        private int _projectID;
        private int _employeeID;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }
        public string Narrative
        {
            get { return _narrative; }
            set
            {
                if (_narrative != value)
                {
                    _narrative = value;
                    OnPropertyChanged(nameof(Narrative));
                }
            }
        }
        public int Hours
        {
            get { return _hours; }
            set
            {
                if (_hours != value)
                {
                    _hours = value;
                    OnPropertyChanged(nameof(Hours));
                }
            }
        }
        public int ProjectID
        {
            get { return _projectID; }
            set
            {
                if (_projectID != value)
                {
                    _projectID = value;
                    OnPropertyChanged(nameof(ProjectID));
                }
            }
        }
        public int EmployeeID
        {
            get { return _employeeID; }
            set
            {
                if (_employeeID != value)
                {
                    _employeeID = value;
                    OnPropertyChanged(nameof(EmployeeID));
                }
            }
        }

        public override string ToString()
        {
            return $"ProjectID: {ProjectID} <> EmployeeID: {EmployeeID} <> Dates[Date: {Date} Hours: {Hours}]";
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
