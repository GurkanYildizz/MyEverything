using Microsoft.EntityFrameworkCore;
using MyEverything.ThisMvc.Entities;
using MyMediatr;
using System.Diagnostics.CodeAnalysis;

namespace MyEverything.ThisMvc.CQRS.Queries;

public class DetailsProjectQuery:IRequest<ProjectInfo>
{
    public Guid Id { get; set; }
}
public class DetailsProjectHandler : IRequestHandler<DetailsProjectQuery, ProjectInfo>
{
    private readonly EverythingDbContext context;

    public DetailsProjectHandler(EverythingDbContext context)
    {
        this.context = context;
    }

    public async Task<ProjectInfo> Handle(DetailsProjectQuery request, CancellationToken cancellationToken = default)
    {
        var selectedData = await context.ProjectsInfo.FirstOrDefaultAsync(f => f.Id == request.Id);
        if (selectedData == null)
        {
            return null;
        }
        
        return selectedData;
        
    }
}
