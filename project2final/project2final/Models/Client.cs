using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace project2final.Models
{
    public class Client : INotifyPropertyChanged
    {
        private int _id;
        private DateTime _opendate;
        private DateTime _closedate;
        private bool _isactive;
        private bool _canclose;
        private string _name;
        private string _notes;

        public int ID
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(ID));
                }
            }
        }

        public DateTime OpenDate
        {
            get { return _opendate; }
            set
            {
                if (_opendate != value)
                {
                    _opendate = value;
                    OnPropertyChanged(nameof(OpenDate));
                }
            }

        }

        public DateTime CloseDate
        {
            get { return _closedate; }
            set
            {
                if (_closedate != value)
                {
                    _closedate = value;
                    OnPropertyChanged(nameof(CloseDate));
                }
            }
        }

        public bool IsActive
        {
            get { return _isactive; }
            set
            {
                if (_isactive != value)
                {
                    _isactive = value;
                    OnPropertyChanged(nameof(IsActive));
                }
            }
        }

        public bool CanClose
        {
            get { return _canclose; }
            set
            {
                if (_canclose != value)
                {
                    _canclose = value;
                    OnPropertyChanged(nameof(CanClose));
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
        public string Notes
        {
            get { return _notes; }
            set
            {
                if (_notes != value)
                {
                    _notes = value;
                    OnPropertyChanged(nameof(Notes));
                }
            }
        }

        public override string ToString()
        {
            return $"Name: {Name} <> Active: {IsActive} <> Dates[Open: {OpenDate} Closed: {CloseDate}]";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
