using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyEverything.ThisMvc.CQRS.Command;
using MyMediatr;

namespace MyEverything.ThisMvc.Controllers.Api;

[Route("api/[controller]")]
[ApiController]

[Authorize]
public class ProjectsAdminController:ControllerBase
{
    private readonly ISender sender;

    public ProjectsAdminController(ISender sender)
    {
        this.sender = sender;
    }

    

    [HttpPost("addproject")]
    public async Task<IActionResult> AddProject([FromForm] ProjectInfoCommand projectInfoCommand, CancellationToken cancellationToken)
    {

        await sender.Send(projectInfoCommand, cancellationToken); // Daha sonra burada başarılı veya değil gibi dönüşler olacak result pattern kullanılacak

        return Ok();
    }

   
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(Guid id,CancellationToken cancellationToken)
    {
        ProjectDeleteCommand projectDeleteCommand = new ProjectDeleteCommand();
        projectDeleteCommand.Id = id;

        await sender.Send(projectDeleteCommand, cancellationToken);
        
        return Ok();
    }

}
