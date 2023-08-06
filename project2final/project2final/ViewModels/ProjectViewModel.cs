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
using project2final.Views;
using project2final.Utils;
using projectFinalAPI.Utils;
using Newtonsoft.Json;

namespace project2final.ViewModels
{
    public class ProjectViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Project> _projects = new ObservableCollection<Project>();

        private ObservableCollection<Project> _filteredprojects = new ObservableCollection<Project>();

        private ObservableCollection<Bill> _bills = new ObservableCollection<Bill>();


        private int currentClientID;

        public ObservableCollection<Project> Projects
        {
            get { return _projects; }
            set
            {
                if (_projects != value)
                {
                    _projects = value;
                    OnPropertyChanged(nameof(Projects));
                }
            }
        }

        public ObservableCollection<Project> FilteredProjects
        {
            get { return _filteredprojects; }
            set
            {
                if (_filteredprojects != value)
                {
                    _filteredprojects = value;
                    OnPropertyChanged(nameof(FilteredProjects));
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


        private bool _canClose;
        private readonly IMessenger _Messenger;
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
        private Project _selectedProject;
        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                OnPropertyChanged(nameof(SelectedProject));
            }
        }

        private Project _newProject = new Project();

        public Project NewProject
        {
            get { return _newProject; }
            set
            {
                if (_newProject != value)
                {
                    _newProject = value;
                    OnPropertyChanged(nameof(NewProject));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand AddProjectCommand { get; private set; }
        public ICommand DeleteProjectCommand { get; private set; }

        public ICommand OpenUpdateProjectPageCommand { get; private set; }
        public ICommand UpdateProjectCommand { get; private set; }
        public ICommand OpenAddProjectCommand { get; private set; }

        public ProjectViewModel(IMessenger messenger)
        {

            var projectStr = new WebRequestHandler().Get("http://localhost:5192/project").Result;
            Projects = new ObservableCollection<Project>(JsonConvert.DeserializeObject<List<Project>>(projectStr));

            _Messenger = messenger;
            _Messenger.Subscribe<SelectedClientID>(this, getClientID);
            _Messenger.Subscribe<sendBills_proj>(this, getBills_proj);
            _Messenger.Subscribe<needsBills_client>(this, getBills_client);


            IsVisible = true;
            _canClose = false;

            OpenAddProjectCommand = new Command(OpenAddProjectPage);
            AddProjectCommand = new Command(AddProject);
            OpenUpdateProjectPageCommand = new Command(OpenUpdateProjectPage);
            UpdateProjectCommand = new Command(UpdateProject);
            DeleteProjectCommand = new Command(DeleteProject);

        }

        private void getBills_client(object obj)
        {
            var message = (needsBills_client)obj;

            var projects_for_clients = new ObservableCollection<Project>();
            foreach (var project in Projects)
            {
                if (project.ClientID == message._clientID)
                {
                    if (!projects_for_clients.Any(b => b.ID == project.ID))
                    {
                        projects_for_clients.Add(project);
                    }
                }
            }

            // sends project IDs to bill, bill will send to Client
            _Messenger.Send(new sendBills_clients(projects_for_clients));
        }

        private void getBills_proj(object obj)
        {
            var message = (sendBills_proj)obj;
            Bills = message._bills;

            Debug.WriteLine(message._bills);
        }

        private void getClientID(object obj)
        {
            var message = (SelectedClientID)obj;
            currentClientID = message._clientID;
            Debug.WriteLine(currentClientID);

            FilteredProjects = new ObservableCollection<Project>(Projects.Where(p => p.ClientID == currentClientID));

            _canClose = FilteredProjects.All(p => !p.IsActive);

            _Messenger.Send(new CanCloseClient(_canClose));

            OnPropertyChanged(nameof(FilteredProjects));

        }

        public void AddProject()
        {

            NewProject.ID = ProjectIDCheck;

            var projStr = new WebRequestHandler().Post($"http://localhost:5192/project/Add/", NewProject);
            Projects.Add(NewProject);
            NewProject = new Project();

        }

        public int ProjectIDCheck
        {
            get
            {
                return Projects.Select(i => i.ID).Max() + 1;
            }
        }


        private void OpenAddProjectPage()
        {
            var addProjectPage = new AddProjectPage
            {
                BindingContext = this
            };

            Application.Current.MainPage.Navigation.PushAsync(addProjectPage);
        }

        private void UpdateProject()
        {

            var projectToUpdate = Projects.FirstOrDefault(p => p.ID == SelectedProject.ID);
            if (projectToUpdate != null && SelectedProject != null)
            {
                int index = Projects.IndexOf(projectToUpdate);
                NewProject.ID = SelectedProject.ID;
                Projects.RemoveAt(index);
                Projects.Insert(index, NewProject);
                var projStr = new WebRequestHandler().Post($"http://localhost:5192/project/Update/{SelectedProject.ID}", NewProject);
                NewProject = new Project();


            }

        }


        private void DeleteProject()
        {
            Projects.Remove(SelectedProject);
            var projStr = new WebRequestHandler().Get($"http://localhost:5192/project/Delete/{SelectedProject.ID}");

        }

        private void OpenUpdateProjectPage()
        {
            if (SelectedProject == null) return;

            // needs bills? 
            _Messenger.Send(new needsBills_proj(SelectedProject.ID));

            var updateProjectPage = new UpdateProjectPage
            {
                BindingContext = this
            };
            Application.Current.MainPage.Navigation.PushAsync(updateProjectPage);
        }

    }
}
