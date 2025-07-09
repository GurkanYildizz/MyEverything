using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MyEverything.ThisMvc.Helpers.Images;

public class ImagesControl
{
    private readonly IWebHostEnvironment webHostEnvironment;

    public ImagesControl(IWebHostEnvironment webHostEnvironment)
    {
        webHostEnvironment = webHostEnvironment;
    }
   // public async Task AddImage(ProjeView)
}
