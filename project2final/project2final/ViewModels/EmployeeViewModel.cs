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
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Employee> _employees = new ObservableCollection<Employee>();

        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set
            {
                if (_employees != value)
                {
                    _employees = value;
                    OnPropertyChanged(nameof(Employees));
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
        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }

        private Employee _newEmployee = new Employee();

        public Employee NewEmployee
        {
            get { return _newEmployee; }
            set
            {
                if (_newEmployee != value)
                {
                    _newEmployee = value;
                    OnPropertyChanged(nameof(NewEmployee));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand AddEmployeeCommand { get; private set; }
        public ICommand DeleteEmployeeCommand { get; private set; }

        private readonly IMessenger _Messenger;

        public ICommand OpenUpdateEmployeePageCommand { get; private set; }
        public ICommand UpdateEmployeeCommand { get; private set; }
        public ICommand OpenAddEmployeeCommand { get; private set; }

        public EmployeeViewModel(IMessenger messenger)
        {
            Employees = new ObservableCollection<Employee>
            {
                new Employee { ID = 0, Name = "Name1", Rate = 3 },
                new Employee { ID = 1, Name = "Name2", Rate = 2 },
                new Employee { ID = 2, Name = "Name3", Rate = 1 },
            };

            IsVisible = true;

            OpenAddEmployeeCommand = new Command(OpenAddEmployeePage);
            AddEmployeeCommand = new Command(AddEmployee);
            OpenUpdateEmployeePageCommand = new Command(OpenUpdateEmployeePage);
            UpdateEmployeeCommand = new Command(UpdateEmployee);
            DeleteEmployeeCommand = new Command(DeleteEmployee);

            _Messenger = messenger;

            _Messenger.Subscribe<needsRate>(this, needsRate);
           


        }

        private void needsRate(object obj)
        {
            var message = (needsRate)obj;

            foreach (var employee in _employees)
            {
                if (employee.ID == message._Rate)
                {
                    _Messenger.Send(new getEmployeeRate(employee.Rate));
                    break;
                }
            }
        }

        public void AddEmployee()
        {
            Employees.Add(NewEmployee);
            NewEmployee = new Employee();

        }

        private void OpenAddEmployeePage()
        {
            var addEmployeePage = new AddEmployeePage
            {
                BindingContext = this
            };

            Application.Current.MainPage.Navigation.PushAsync(addEmployeePage);
        }
        private void UpdateEmployee()
        {
            var clientToUpdate = Employees.FirstOrDefault(p => p.ID == SelectedEmployee.ID);
            if (clientToUpdate != null)
            {
                NewEmployee.ID = SelectedEmployee.ID;
                int index = Employees.IndexOf(clientToUpdate);
                Employees.Remove(SelectedEmployee);
                Employees.Insert(index, NewEmployee); 
            }
        }

        private void DeleteEmployee()
        {
            Employees.Remove(SelectedEmployee);
        }

        private void OpenUpdateEmployeePage()
        {
            if (SelectedEmployee == null) return;

            var updateEmployeePage = new UpdateEmployeePage
            {
                BindingContext = this
            };
            Application.Current.MainPage.Navigation.PushAsync(updateEmployeePage);
        }

    }
}
