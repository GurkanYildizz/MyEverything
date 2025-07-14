using Microsoft.Identity.Client;
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
            Fixing();
           
            UpdateDate = DateTime.Now;
        }
        public void Fixing()
        {
            if (Explanation?.Length >= 50)
            {
                MinExplanation = $"{Explanation?.Substring(0, 50)}...";
            }
            else
            {
                MinExplanation = Explanation;
            }
        }

    }
}
