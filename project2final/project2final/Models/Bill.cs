using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace project2final.Models
{
    public class Bill : INotifyPropertyChanged
    {
        private int _id;
        private ObservableCollection<Time> _times;
        private DateTime _dueDate;
        private decimal _totalAmount;


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

        public ObservableCollection<Time> Times
        {
            get { return _times; }
            set
            {
                if (_times != value)
                {
                    if (_times != null)
                    {
                        _times.CollectionChanged -= Times_CollectionChanged;
                    }

                    _times = value;

                    if (_times != null)
                    {
                        _times.CollectionChanged += Times_CollectionChanged;
                    }

                    OnPropertyChanged(nameof(Times));
                }
            }
        }

        public DateTime DueDate
        {
            get { return _dueDate; }
            set
            {
                if (_dueDate != value)
                {
                    _dueDate = value;
                    OnPropertyChanged(nameof(DueDate));
                }
            }

        }

        public decimal TotalAmount
        {
            get { return _totalAmount; }
            set
            {
                if (_totalAmount != value)
                {
                    _totalAmount = value;
                    OnPropertyChanged(nameof(TotalAmount));
                }
            }
        }

        public override string ToString()
        {
            return $"Due: {DueDate} <> Total: {TotalAmount}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Times_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Times));
        }

        private void AmountChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(TotalAmount));
        }


    }
}
