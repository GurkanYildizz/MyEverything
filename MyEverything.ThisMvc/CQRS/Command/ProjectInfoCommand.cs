using MyEverything.ThisMvc.CQRS.Command;
using MyEverything.ThisMvc.Entities;
using MyEverything.ThisMvc.Entities.DTOs;
using MyMediatr;

namespace MyEverything.ThisMvc.CQRS.Command;

public class ProjectInfoCommand : ProjectInfoAndImage_Dto, IRequest
{

}


public class ProjectInfoCommandHandler : IRequestHandler<ProjectInfoCommand>
{
    private readonly EverythingDbContext context;
    private readonly IWebHostEnvironment env;

    public ProjectInfoCommandHandler(EverythingDbContext context, IWebHostEnvironment env)
    {
        this.context = context;
        this.env = env;
    }



    public async Task Handle(ProjectInfoCommand request, CancellationToken cancellationToken = default)
    {
        var entity = new ProjectInfo
        {
            ProjectName = request.ProjectName,
            Explanation = request.Explanation,
            YoutubeLink = request.YoutubeLink,
            Version = request.Version,
            Title = request.Title,
           
        };
        entity.Fixing();

        var imageList = new List<string>();

        if (request.ProjectImageFiles is not null)
        {
            foreach (var imageFile in request.ProjectImageFiles)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var savePath = Path.Combine(env.WebRootPath, "uploads", fileName);

                using var stream = new FileStream(savePath, FileMode.Create);
                await imageFile.CopyToAsync(stream, cancellationToken);

                imageList.Add("/uploads/" + fileName); 
            }
        }

        if (request.ImageFile is not null)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(request.ImageFile.FileName);
            var savePath = Path.Combine(env.WebRootPath, "uploads", fileName);

            using var stream = new FileStream(savePath, FileMode.Create);
            await request.ImageFile.CopyToAsync(stream, cancellationToken);

            entity.Image = ("/uploads/" + fileName); // Veya sadece dosya adı
        }

        entity.Images = imageList;
       

        context.ProjectsInfo.Add(entity);
        await context.SaveChangesAsync(cancellationToken);


        
    }
}
