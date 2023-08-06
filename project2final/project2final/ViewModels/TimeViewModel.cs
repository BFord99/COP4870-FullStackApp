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
    public class TimeViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Time> _times = new ObservableCollection<Time>();

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
        private Time _selectedTime;
        public Time SelectedTime
        {
            get { return _selectedTime; }
            set
            {
                _selectedTime = value;
                _Messenger.Send(new SelectedTime(_selectedTime));
                OnPropertyChanged(nameof(SelectedTime));
            }
        }

        private Time _newTime = new Time();

        public Time NewTime
        {
            get { return _newTime; }
            set
            {
                if (_newTime != value)
                {
                    _newTime = value;
                    OnPropertyChanged(nameof(NewTime));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand AddTimeCommand { get; private set; }
        public ICommand DeleteTimeCommand { get; private set; }

        private readonly IMessenger _Messenger;

        public ICommand OpenUpdateTimePageCommand { get; private set; }
        public ICommand UpdateTimeCommand { get; private set; }
        public ICommand OpenAddTimeCommand { get; private set; }

        public TimeViewModel(IMessenger messenger)
        {
            Times = new ObservableCollection<Time>
            {
                new Time { Date = DateTime.Now, EmployeeID = 0, ProjectID = 1, Narrative = "Narrative1", Hours = 6 },
                new Time { Date = DateTime.Now, EmployeeID = 2, ProjectID = 3, Narrative = "Narrative2", Hours = 3 },
                new Time { Date = DateTime.Now, EmployeeID = 1, ProjectID = 1, Narrative = "Narrative3", Hours = 2 },
                new Time { Date = DateTime.Now, EmployeeID = 0, ProjectID = 1, Narrative = "Narrative4", Hours = 1 },
                new Time { Date = DateTime.Now, EmployeeID = 1, ProjectID = 2, Narrative = "Narrative5", Hours = 5 },
                new Time { Date = DateTime.Now, EmployeeID = 0, ProjectID = 2, Narrative = "Narrative6", Hours = 2 },
                new Time { Date = DateTime.Now, EmployeeID = 0, ProjectID = 2, Narrative = "Narrative1", Hours = 6 },
                new Time { Date = DateTime.Now, EmployeeID = 2, ProjectID = 1, Narrative = "Narrative2", Hours = 3 },
                new Time { Date = DateTime.Now, EmployeeID = 1, ProjectID = 0, Narrative = "Narrative3", Hours = 2 },
                new Time { Date = DateTime.Now, EmployeeID = 0, ProjectID = 0, Narrative = "Narrative4", Hours = 1 },
                new Time { Date = DateTime.Now, EmployeeID = 1, ProjectID = 0, Narrative = "Narrative5", Hours = 5 },
                new Time { Date = DateTime.Now, EmployeeID = 0, ProjectID = 3, Narrative = "Narrative6", Hours = 2 },
            };

            IsVisible = true;

            OpenAddTimeCommand = new Command(OpenAddTimePage);
            AddTimeCommand = new Command(AddTime);
            OpenUpdateTimePageCommand = new Command(OpenUpdateTimePage);
            UpdateTimeCommand = new Command(UpdateTime);
            DeleteTimeCommand = new Command(DeleteTime);

            _Messenger = messenger;
            _Messenger.Subscribe<needsTimes>(this, needsTimes);

            // message from Bills NeedsTime, NeedsTime Method Sends Times, Bills Recieves Times For Viewing




        }

        private void needsTimes(object obj)
        {
            _Messenger.Send(new TimesforBills(Times));
        }

        public void AddTime()
        {
            Times.Add(NewTime);
            NewTime = new Time();

        }

        private void OpenAddTimePage()
        {
            var addTimePage = new AddTimePage
            {
                BindingContext = this
            };

            Application.Current.MainPage.Navigation.PushAsync(addTimePage);
        }


        private void UpdateTime()
        {
            var timetoUpdate = Times.FirstOrDefault(p => p.EmployeeID == SelectedTime.EmployeeID);
            if (timetoUpdate != null)
            {
                NewTime.EmployeeID = SelectedTime.EmployeeID;
                int index = Times.IndexOf(timetoUpdate);
                Times.Remove(SelectedTime);
                Times.Insert(index, NewTime);
            }
        }

        private void DeleteTime()
        {
            Times.Remove(SelectedTime);
        }

        private void OpenUpdateTimePage()
        {
            if (SelectedTime == null) return;

            var updateTimePage = new UpdateTimePage
            {
                BindingContext = this
            };
            Application.Current.MainPage.Navigation.PushAsync(updateTimePage);
        }

    }
}
