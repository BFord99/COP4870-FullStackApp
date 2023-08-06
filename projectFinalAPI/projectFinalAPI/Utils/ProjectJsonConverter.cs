using Newtonsoft.Json.Linq;
using projectFinalAPI.Models;

namespace projectFinalAPI.Utils
{
    public class ProjectJsonConverter : JsonCreationConverter<Project>
    {
        protected override Project Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            return new Project();

        }
    }
}


