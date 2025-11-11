namespace CMS.Models;

public class EmailRequestModel
{
    public string FormType { get; set; } = null!;
    public string? NewsletterEmail { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Question { get; set; }
    public string? SelectedOption { get; set; }
}
