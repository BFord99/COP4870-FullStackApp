using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using project2final.Models;
using project2final.Utils;
using project2final.Views;

namespace project2final.ViewModels
{
    public class BillViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Bill> _bills = new ObservableCollection<Bill>();
        private ObservableCollection<Time> _times = new ObservableCollection<Time>();


        public ObservableCollection<Bill> Bills
        {
            get { return _bills; }
            set
            {
                if (_bills != value)
                {
                    _bills = value;
                    OnPropertyChanged(nameof(Bills));
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
                    _times = value;
                    OnPropertyChanged(nameof(Times));
                }
            }
        }
        private bool _isVisible;
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }
        private Bill _selectedBill;
        public Bill SelectedBill
        {
            get { return _selectedBill; }
            set
            {
                _selectedBill = value;
                OnPropertyChanged(nameof(SelectedBill));
            }
        }

        private Time _selectedTime;
        public Time SelectedTime
        {
            get { return _selectedTime; }
            set
            {
                _selectedTime = value;
                OnPropertyChanged(nameof(SelectedTime));
            }
        }

        private Bill _newBill = new Bill();
        private bool _Neg;

        public Bill NewBill
        {
            get { return _newBill; }
            set
            {
                if (_newBill != value)
                {
                    _newBill.Times = new ObservableCollection<Time>();
                    _newBill.TotalAmount = 0;
                    _newBill = value;
                    OnPropertyChanged(nameof(NewBill));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand AddBillCommand { get; private set; }
        public ICommand DeleteBillCommand { get; private set; }

        private readonly IMessenger _Messenger;

        public ICommand OpenUpdateBillPageCommand { get; private set; }
        public ICommand UpdateBillCommand { get; private set; }
        public ICommand OpenAddBillCommand { get; private set; }

        public ICommand AddTimeBillCommand { get; private set; }

        public ICommand DeleteTimeBillCommand { get; private set; }

        public BillViewModel(IMessenger messenger)
        {
            Bills = new ObservableCollection<Bill>();

            Times = new ObservableCollection<Time>();

            IsVisible = true;

            OpenAddBillCommand = new Command(OpenAddBillPage);
            AddBillCommand = new Command(AddBill);
            OpenUpdateBillPageCommand = new Command(OpenUpdateBillPage);
            UpdateBillCommand = new Command(UpdateBill);
            DeleteBillCommand = new Command(DeleteBill);
            AddTimeBillCommand = new Command(AddTimeBill);
            DeleteTimeBillCommand = new Command(DeleteTimeBill);

            _Messenger = messenger;

            _Messenger.Subscribe<TimesforBills>(this, getTimes);
            _Messenger.Subscribe<getEmployeeRate>(this, getEmployeeRate); 
            _Messenger.Subscribe<needsBills_proj>(this, needsbills_proj);
            _Messenger.Subscribe<sendBills_clients>(this, sendBills_final); 



        }

        private void sendBills_final(object obj)
        {
            var message = (sendBills_clients)obj;

            var uniqueProjectIDs = message._projects.Select(p => p.ID).Distinct().ToList();

            var bills_final = new ObservableCollection<Bill>();
            foreach (var bill in Bills)
            {
                foreach (var time in bill.Times)
                {
                    if (uniqueProjectIDs.Contains(time.ProjectID))
                    {
                        if (!bills_final.Any(b => b.ID == bill.ID))
                        {
                            bills_final.Add(bill);
                        }
                    }
                }
            }
            _Messenger.Send(new sendClients_bills(bills_final)); 
        }

        private void needsbills_proj(object obj)
        {
            var message = (needsBills_proj)obj;

            // send back bills that are attatched to projects
            // for time in times for bill in bills ; list.append(time) if time.projectID = message._projectID

            var bills_for_projects = new ObservableCollection<Bill>();
            foreach (var bill in Bills)
            {
                foreach (var time in bill.Times)
                {
                    if (time.ProjectID == message._projectID)
                    {
                        if (!bills_for_projects.Any(b => b.ID == bill.ID))
                        {
                            bills_for_projects.Add(bill);
                        }
                    }
                }
            }

            Debug.WriteLine(bills_for_projects);
            _Messenger.Send(new sendBills_proj(bills_for_projects));
        }


        private void DeleteTimeBill()
        {
            _Neg =  true;
            var billToUpdate = Bills.FirstOrDefault(p => p.ID == SelectedBill.ID);
            if (billToUpdate != null)
            {
                int index = Bills.IndexOf(billToUpdate);
                Bills[index].Times.Remove(SelectedTime);
                _Messenger.Send(new needsRate(SelectedTime.EmployeeID));
            }

        }

        private void getEmployeeRate(object obj)
        {
            var message = (getEmployeeRate)obj;

            if (_Neg == true)
            {
                SelectedBill.TotalAmount = SelectedBill.TotalAmount - message._rate * SelectedTime.Hours;
            }
            if (_Neg == false)
            {
                SelectedBill.TotalAmount = SelectedBill.TotalAmount + message._rate * SelectedTime.Hours;
            }

            Debug.WriteLine(SelectedTime.Hours);
            Debug.WriteLine(message._rate);
            Debug.WriteLine(SelectedBill.TotalAmount);


        }

        private void AddTimeBill()
        {
            var billToUpdate = Bills.FirstOrDefault(p => p.ID == SelectedBill.ID);
            if (billToUpdate != null)
            {
                _Neg = false;
                int index = Bills.IndexOf(billToUpdate);
                Bills[index].Times.Add(SelectedTime);
                _Messenger.Send(new needsRate(SelectedTime.EmployeeID));
            }
        }

        private void getTimes(object obj)
        {
            var message = (TimesforBills)obj;
            Times = message._timesForBills;
            Debug.WriteLine(Times[0].ToString());
        }

        public void AddBill()
        {
            Bills.Add(NewBill);
            NewBill = new Bill();

        }

        private void OpenAddBillPage()
        {
            var addBillPage = new AddBillPage
            {
                BindingContext = this
            };

            Application.Current.MainPage.Navigation.PushAsync(addBillPage);
        }

        private void UpdateBill()
        {
            var billToUpdate = Bills.FirstOrDefault(p => p.ID == SelectedBill.ID);
            if (billToUpdate != null)
            {
                int index = Bills.IndexOf(billToUpdate);
                Bills[index] = SelectedBill;
            }
        }

        private void DeleteBill()
        {
            Bills.Remove(SelectedBill);
        }


        // gets times from TimeViewModel
        private void OpenUpdateBillPage()
        {
            if (SelectedBill == null) return;
            _Messenger.Send(new needsTimes(true));

            var updateBillPage = new UpdateBillPage
            {
                BindingContext = this
            };
            Application.Current.MainPage.Navigation.PushAsync(updateBillPage);
        }

    }
}