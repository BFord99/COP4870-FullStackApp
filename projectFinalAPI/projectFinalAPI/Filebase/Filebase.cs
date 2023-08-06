using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using projectFinalAPI.Models;


namespace projectFinalAPI.Filebase
{
    public class Filebase
    {
        private string _root;
        private string _projectRoot;
        private static Filebase _instance;

        public static Filebase Current
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Filebase();
                }

                return _instance;
            }
        }

        private Filebase()
        {
            _root = "C:\\temp\\projectFinalAPI";
            _projectRoot = "C:\\Users\\Braden\\Desktop\\BFord_FullProject\\projectFinalAPI";
        }

        public List<Project> Projects
        {
            get
            {
                if (!File.Exists($"{_projectRoot}\\projects.json"))
                {
                    var payload = JsonConvert.SerializeObject(new List<Project>
                    {
                        new Project { ID = 0, ShortName = "SN1", LongName = "LN1", IsActive = true, CloseDate = DateTime.Now, OpenDate = DateTime.Now, ClientID =0 },
                        new Project { ID = 1, ShortName = "SN2", LongName = "LN2", IsActive = false, CloseDate = DateTime.Now, OpenDate = DateTime.Now, ClientID = 0 },
                        new Project { ID = 2, ShortName = "SN3", LongName = "LN3", IsActive = true, CloseDate = DateTime.Now, OpenDate = DateTime.Now, ClientID =0 },
                        new Project { ID = 3, ShortName = "SN4", LongName = "LN4", IsActive = false, CloseDate = DateTime.Now, OpenDate = DateTime.Now , ClientID = 1},
                        new Project { ID = 4, ShortName = "SN5", LongName = "LN5", IsActive = true, CloseDate = DateTime.Now, OpenDate = DateTime.Now, ClientID = 1}
                    });

                    File.WriteAllText($"{_projectRoot}\\projects.json", payload);
                }
                var stringProj = File.ReadAllText($"{_projectRoot}\\projects.json");
                var projects = JsonConvert.DeserializeObject<List<Project>>(stringProj) ?? new List<Project>();
                return projects;
            }
        }

        public Project? Delete(int id)
        {
            var projects = Projects;
            var projectToDelete = projects.FirstOrDefault(i => i.ID == id);

            projects.Remove(projectToDelete);
 
            var projStr = JsonConvert.SerializeObject(projects);
            File.WriteAllText($"{_projectRoot}\\projects.json", projStr);

            return projectToDelete;
        }

        public Project? Update(Project p)
        {
            var projects = Projects;
            var itemToRemove = projects.FirstOrDefault(i => i.ID == p.ID);
            var index = projects.IndexOf(itemToRemove);
            projects.RemoveAt(index);
            projects.Insert(index, p);
            var projectStr = JsonConvert.SerializeObject(projects);
            File.WriteAllText($"{_projectRoot}\\projects.json", projectStr);
            return p;

        }

        public Project? Add(Project p)
        {
            var projects = Projects; 

            if (p.ID >= 0)
            {
                p.ID = ProjectIdCheck;
                projects.Add(p);
            }

            var projectStr = JsonConvert.SerializeObject(projects);
            File.WriteAllText($"{_projectRoot}\\projects.json", projectStr);
            return p;
        }

        public int ProjectIdCheck
        {
            get
            {
                return Projects.Select(i => i.ID).Max() + 1;
            }
        }

    }
}
