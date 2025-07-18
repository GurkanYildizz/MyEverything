using Microsoft.EntityFrameworkCore;
using MyEverything.ThisMvc.Entities;
using MyMediatr;

namespace MyEverything.ThisMvc.CQRS.Queries;

public class MainProjectsQuery : IRequest<List<ProjectInfo>>
{

}

public class MainProjectsHandler : IRequestHandler<MainProjectsQuery, List<ProjectInfo>>
{
    private readonly EverythingDbContext everythingDbContext;
    public MainProjectsHandler(EverythingDbContext everythingDbContext)
    {
        this.everythingDbContext = everythingDbContext;
    }


    public async Task<List<ProjectInfo>> Handle(MainProjectsQuery request, CancellationToken cancellationToken = default)
    {
        var datas = await everythingDbContext.ProjectsInfo.ToListAsync();
        if (datas==null)
        {
            return null;
        }

        return datas;
    }

}

