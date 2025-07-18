﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyEverything.ThisMvc.Entities.DTOs;

public class Projects_Dto
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(100)]
    public string? Title { get; set; }

   
    [MaxLength(255)]
    public string? Image { get; set; } //Resim ekleme hala yapılamadı...

    
    [MaxLength(255)]
    public string? MinExplanation { get; set; } 

    public DateTime CreatedDate { get; set; } = DateTime.Now;
    
    
    public DateTime UpdateDate { get; set; } = DateTime.Now;

}

