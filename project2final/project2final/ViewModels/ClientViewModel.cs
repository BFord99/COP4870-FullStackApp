using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using project2final.Models;
using project2final.Views;
using project2final.Utils;
using System.Diagnostics;

namespace project2final.ViewModels
{
    public class ClientViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Client> _clients = new ObservableCollection<Client>();

        private ObservableCollection<Bill> _bills = new ObservableCollection<Bill>();

        public ObservableCollection<Client> Clients
        {
            get { return _clients; }
            set
            {
                if (_clients != value)
                {
                    _clients = value;
                    OnPropertyChanged(nameof(Clients));
                }
            }
        }

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

        private readonly IMessenger _Messenger;
        private int _selectedID;
        private Client _selectedClient;

        public Client SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                OnPropertyChanged(nameof(SelectedClient));
                if (_selectedClient != null)
                {
                    _Messenger.Send(new SelectedClientID(_selectedClient.ID));
                    Debug.WriteLine(_canClose);

                    _selectedClient.CanClose = _canClose;
                }
            }
        }

        private Client _newClient = new Client();
        private bool _canClose;

        public Client NewClient
        {
            get { return _newClient; }
            set
            {
                if (_newClient != value)
                {
                    _newClient = value;
                    OnPropertyChanged(nameof(NewClient));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand AddClientCommand { get; private set; }
        public ICommand DeleteClientCommand { get; private set; }

        public ICommand OpenUpdateClientPageCommand { get; private set; }
        public ICommand UpdateClientCommand { get; private set; }
        public ICommand OpenAddClientCommand { get; private set; }

        public ClientViewModel(IMessenger messenger)
        {
            Clients = new ObservableCollection<Client>
            {
                new Client { ID = 0, Name = "Name1", Notes = "Notes1", IsActive = true, CloseDate = DateTime.Now, OpenDate = DateTime.Now, CanClose = true},
                new Client { ID = 1, Name = "Name2", Notes = "Notes2", IsActive = true, CloseDate = DateTime.Now, OpenDate = DateTime.Now, CanClose = true},
                new Client { ID = 2, Name = "Name3", Notes = "Notes3", IsActive = true, CloseDate = DateTime.Now, OpenDate = DateTime.Now, CanClose = true},

            };

            IsVisible = true;
            _Messenger = messenger;
            _Messenger.Subscribe<CanCloseClient>(this, CanClose);
            _Messenger.Subscribe<sendClients_bills>(this, getBills);


            if (SelectedClient != null)
            {
                _selectedID = SelectedClient.ID;
            }
            else
            {
                _selectedID = 0;
            }

            OpenAddClientCommand = new Command(OpenAddClientPage);
            AddClientCommand = new Command(AddClient);
            OpenUpdateClientPageCommand = new Command(OpenUpdateClientPage);
            UpdateClientCommand = new Command(UpdateClient);
            DeleteClientCommand = new Command(DeleteClient);

        }

        private void getBills(object obj)
        {
            var message = (sendClients_bills)obj;
            Bills = message._bills;
        }

        private void CanClose(object obj)
        {
            var message = (CanCloseClient)obj;
            _canClose = message._canClose;
        }

        public void AddClient()
        {
            Clients.Add(NewClient);
            NewClient = new Client();
        }

        private void OpenAddClientPage()
        {
            var addClientPage = new AddClientPage
            {
                BindingContext = this
            };

            Application.Current.MainPage.Navigation.PushAsync(addClientPage);
        }

        private void UpdateClient()
        {
            var clientToUpdate = Clients.FirstOrDefault(p => p.ID == SelectedClient.ID);
            if (clientToUpdate != null)
            {
                NewClient.ID = SelectedClient.ID;
                int index = Clients.IndexOf(clientToUpdate);
                Clients.Remove(SelectedClient);
                Clients.Insert(index, NewClient); 
            }
        }



        private void DeleteClient()
        {
            Clients.Remove(SelectedClient);
        }

        private void OpenUpdateClientPage()
        {
            if (SelectedClient == null) return;

            // tells projects it needs bills
            // projects get bills 
            // bills sends back to client
            _Messenger.Send(new needsBills_client(SelectedClient.ID));

            var updateClientPage = new UpdateClientPage
            {
                BindingContext = this
            };
            Application.Current.MainPage.Navigation.PushAsync(updateClientPage);
        }

    }
}
