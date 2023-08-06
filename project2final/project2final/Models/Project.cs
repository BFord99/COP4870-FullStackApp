using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace project2final.Models
{
    public class Project : INotifyPropertyChanged
    {
        private int _id;
        private DateTime _opendate;
        private DateTime _closedate;
        private bool _isactive;
        private string _shortname;
        private string _longname;
        private int _clientID;

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

        public string ShortName
        {
            get { return _shortname; }
            set
            {
                if (_shortname != value)
                {
                    _shortname = value;
                    OnPropertyChanged(nameof(ShortName));
                }
            }
        }
        public string LongName
        {
            get { return _longname; }
            set
            {
                if (_longname != value)
                {
                    _longname = value;
                    OnPropertyChanged(nameof(LongName));
                }
            }
        }

        public int ClientID
        {
            get { return _clientID; }
            set
            {
                if (_clientID != value)
                {
                    _clientID = value;
                    OnPropertyChanged(nameof(ClientID));
                }
            }
        }

        public override string ToString()
        {
            return $"ShortName: {ShortName} <> Active: {IsActive} <> Dates[Open: {OpenDate} Closed: {CloseDate}] <> ClientID: {ClientID}";
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


