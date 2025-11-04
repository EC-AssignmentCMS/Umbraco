using System.ComponentModel.DataAnnotations;

namespace CMS.ViewModels;

public class NewsletterViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [Display(Name = "E-mail address")]
    [RegularExpression(@"^(?!\.)[A-Za-z0-9._%+-]+(?<!\.)@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$",
    ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; } = null!;
}
