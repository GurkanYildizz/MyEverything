namespace MyEverything.ThisMvc.Entities.DTOs
{
    public class ProjectInfoAndImage_Dto:Projects_Dto
    {
        public string? ProjectName { get; set; }
        public double Version { get; set; }
        public string? Explanation { get; set; }
        public string? YoutubeLink { get; set; }
        public string? Title { get; set; }

        public IFormFile? ImageFile { get; set; }
        public List<IFormFile>? ProjectImageFiles { get; set; }
    }
}
