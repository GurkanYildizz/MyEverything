using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace MyEverything.ThisMvc.Entities;

public class AdminLoginInfo:IdentityUser
{/*
    [Key]
    public Guid Id { get; set; }    

    [Required(ErrorMessage = "E-posta adresi zorunludur.")]
    [EmailAddress(ErrorMessage ="Geçerli bir E-posta adresi giriniz")]
    [Display(Name ="E-posta Adresi")]
    [MaxLength(100)]
        public string? Email { get; set; }
    [Required(ErrorMessage ="Şifre analanı zorunludur")]
    [DataType(DataType.Password)]
    [Display(Name ="Şifre")]
    [MaxLength(255)]
    public string? Password { get; set; }
    */
}
