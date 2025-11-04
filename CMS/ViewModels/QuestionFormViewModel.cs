using System.ComponentModel.DataAnnotations;

namespace CMS.ViewModels;

public class QuestionFormViewModel
{
    [Required(ErrorMessage = "Name is required")]
    [Display(Name = "Name")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Email is required")]
    [Display(Name = "E-mail address")]
    [RegularExpression(@"^(?!\.)[A-Za-z0-9._%+-]+(?<!\.)@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$",
    ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Please enter a question")]
    [Display(Name = "Question")]
    public string Question { get; set; } = null!;
}
