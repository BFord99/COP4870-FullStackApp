using project2final.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using project2final.ViewModels;
using project2final.Utils;

public class MainViewModel : INotifyPropertyChanged
{
    public readonly IMessenger messenger;

    public ProjectViewModel ProjectViewModel { get; set; }
    public ClientViewModel ClientViewModel { get; set; }
    public EmployeeViewModel EmployeeViewModel { get; set; }
    public TimeViewModel TimeViewModel { get; set; }
    public BillViewModel BillViewModel { get; set; }

    public ICommand ProjectViewCommand { get; set; }
    public ICommand ClientViewCommand { get; set; }
    public ICommand EmployeeViewCommand { get; set; }
    public ICommand TimeViewCommand { get; set; }
    public ICommand BillViewCommand { get; set; }



    public MainViewModel()
    {
        // TODO: new view models for each model, will be intiialized when mainviewmodel

        messenger = new Messenger();

        ProjectViewModel = new ProjectViewModel(messenger);
        ClientViewModel = new ClientViewModel(messenger);
        TimeViewModel = new TimeViewModel(messenger);
        EmployeeViewModel = new EmployeeViewModel(messenger);
        BillViewModel = new BillViewModel(messenger);

        ProjectViewModel.IsVisible = true;
        ClientViewModel.IsVisible = false;
        TimeViewModel.IsVisible = false;
        EmployeeViewModel.IsVisible = false;
        BillViewModel.IsVisible = false;

        ProjectViewCommand = new Command(ViewProjects);
        ClientViewCommand = new Command(ViewClients);
        EmployeeViewCommand = new Command(ViewEmployees);
        TimeViewCommand = new Command(ViewTimes);
        BillViewCommand = new Command(ViewBills);
    }

    // Add to View Models
    private void ViewProjects()
    {
        ProjectViewModel.IsVisible = true;
        ClientViewModel.IsVisible = false;
        TimeViewModel.IsVisible = false;
        EmployeeViewModel.IsVisible = false;
        BillViewModel.IsVisible = false;
    }
    private void ViewClients()
    {
        ProjectViewModel.IsVisible = false;
        ClientViewModel.IsVisible = true;
        TimeViewModel.IsVisible = false;
        EmployeeViewModel.IsVisible = false;
        BillViewModel.IsVisible = false;
    }
    private void ViewEmployees()
    {
        ProjectViewModel.IsVisible = false;
        ClientViewModel.IsVisible = false;
        TimeViewModel.IsVisible = false;
        EmployeeViewModel.IsVisible = true;
        BillViewModel.IsVisible = false;
    }
    private void ViewTimes()
    {
        ProjectViewModel.IsVisible = false;
        ClientViewModel.IsVisible = false;
        TimeViewModel.IsVisible = true;
        EmployeeViewModel.IsVisible = false;
        BillViewModel.IsVisible = false;
    }

    private void ViewBills()
    {
        ProjectViewModel.IsVisible = false;
        ClientViewModel.IsVisible = false;
        TimeViewModel.IsVisible = false;
        EmployeeViewModel.IsVisible = false;
        BillViewModel.IsVisible = true;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
