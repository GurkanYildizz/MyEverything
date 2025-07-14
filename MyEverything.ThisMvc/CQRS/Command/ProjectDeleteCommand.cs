using Microsoft.EntityFrameworkCore;
using MyEverything.ThisMvc.Entities;
using MyEverything.ThisMvc.Helpers.DbHelpers;
using MyMediatr;

namespace MyEverything.ThisMvc.CQRS.Command;

public class ProjectDeleteCommand : IRequest
{
    public Guid Id { get; set; }
}

public class ProjectDeleteHandler : IRequestHandler<ProjectDeleteCommand>
{
    private readonly EverythingDbContext everythingDbContext;
    public ProjectDeleteHandler(EverythingDbContext everythingDbContext)
    {
        this.everythingDbContext = everythingDbContext;
    }
    public async Task Handle(ProjectDeleteCommand request, CancellationToken cancellationToken = default)
    {
        var data = await everythingDbContext.ProjectsInfo.FirstOrDefaultAsync(f => f.Id == request.Id);
        everythingDbContext.ProjectsInfo.RemoveRange(data);
        await everythingDbContext.SaveChangesAsync();
    }
}
