using projectFinalAPI.Filebase;
using projectFinalAPI.Models;


namespace projectFinalAPI.EC
{
    public class ProjectEC
    {
        public List<Project> Get()
        {
            return Filebase.Filebase.Current.Projects;
        }

        public Project? Delete(int id)
        {
            return Filebase.Filebase.Current.Delete(id);
        }

        public Project? Add(Project p)
        {
            return Filebase.Filebase.Current.Add(p);
        }

        public Project? Update(Project p)
        {
            return Filebase.Filebase.Current.Update(p);
        }

    }
}
