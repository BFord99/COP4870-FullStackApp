using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project2final.Models
{
    public class Employee : INotifyPropertyChanged
    {
        private int _id;
        private decimal _rate;
        private string _name;

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
        public decimal Rate
        {
            get { return _rate; }
            set
            {
                if (_rate != value)
                {
                    _rate = value;
                    OnPropertyChanged(nameof(Rate));
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

        public override string ToString()
        {
            return $"Name: {Name} <> Rate: {Rate}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
}
