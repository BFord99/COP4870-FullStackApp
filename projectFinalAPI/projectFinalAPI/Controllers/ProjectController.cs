using Microsoft.AspNetCore.Mvc;
using projectFinalAPI.EC;
using projectFinalAPI.Models;


namespace projectFinalAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : ControllerBase
    {
        [HttpGet]
        public List<Project> Get()
        {
            return new ProjectEC().Get();
        }

        [HttpGet("Delete/{id}")]
        public Project? Delete(int id)
        {
            return new ProjectEC().Delete(id);
        }

        [HttpPost("Add")]

        public Project? Add([FromBody] Project p)
        {
            return new ProjectEC().Add(p);
        }


        [HttpPost("Update/{id}")]

        public Project? Update([FromBody] Project p)
        {
            return new ProjectEC().Update(p);
        }
    }
}
