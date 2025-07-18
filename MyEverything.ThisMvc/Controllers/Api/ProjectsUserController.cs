using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyEverything.ThisMvc.CQRS.Queries;
using MyEverything.ThisMvc.Entities;
using MyMediatr;

namespace MyEverything.ThisMvc.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsUserController : ControllerBase
    {
        private readonly ISender sender;

        public ProjectsUserController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet("main-projects")]
        public async Task<List<ProjectInfo>> MainProjects(CancellationToken cancellationToken)
        {
            var mainProjectsQuery = new MainProjectsQuery();
            var datas = await sender.Send(mainProjectsQuery, cancellationToken);
            return datas;
        }

        [HttpGet("project-details/{id:guid}")]
        public async Task<ProjectInfo> ProjectDetails([FromRoute]Guid id, CancellationToken cancellationToken)
        {
            var detailsProjectQuery= new DetailsProjectQuery { Id = id };
            var data = await sender.Send(detailsProjectQuery, cancellationToken);
            return data;
        }
    }
}
