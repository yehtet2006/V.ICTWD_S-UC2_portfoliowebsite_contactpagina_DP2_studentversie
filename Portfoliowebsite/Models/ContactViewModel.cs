using System.ComponentModel.DataAnnotations;

namespace Portfoliowebsite.Models;

public class ContactViewModel
{
    [Required(ErrorMessage = "E-mail is verplicht ") ]
    [EmailAddress (ErrorMessage = "Ongeldig e-mailadres")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Er moet een naam bevatten")]
    [StringLength(50, ErrorMessage = "De naam mag niet langer zijn dan 50 tekens")]
    public string Name { get; set; }
    
    [StringLength(150, ErrorMessage = "Het onderwerp mag niet langer zijn dan 150 tekens")]
    [Required (ErrorMessage = "Er moet een onderwerp bevatten")]
    public string Subject { get; set; }
    
    [Required(ErrorMessage = "Er moet een bericht bevatten")]
    [StringLength(1000, ErrorMessage = "Het bericht mag niet langer zijn dan 1000 tekens")]
    public string Message { get; set; }
    
    //honeypot veld
    public string Website { get; set; }
}