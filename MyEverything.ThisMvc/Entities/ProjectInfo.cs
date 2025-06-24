using MyEverything.ThisMvc.Entities.DTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyEverything.ThisMvc.Entities
{
    [Table("ProjectsInfo",Schema ="info")]
    public class ProjectInfo:Projects_Dto
    {
       public ProjectInfo()
        {
            MinExplanation = Explanation?.Substring(0,50);
        }

        [MaxLength(150)]
        public string? YoutubeLink { get; set; }


        [Required]
        [MaxLength(100)]
        public string? ProjectName { get; set; }

        [Required]
        public double Version { get; set; }

        [Required]
        public string? Explanation { get; set; }
        public List<string> Images { get; set; }=new List<string>();

        public void UpdateData()
        {
            MinExplanation = Explanation.Substring(0, 100);
            UpdateDate = DateTime.Now;
        }

    }
}
