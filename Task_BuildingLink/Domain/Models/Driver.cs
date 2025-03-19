namespace Task_BuildingLink.Domain.Models;
using System.ComponentModel.DataAnnotations;

public class Driver
{
    public int Id { get; set; }
    [Required(ErrorMessage = "FirstName is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "FirstName must be between 3 and 100 characters.")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "LastName is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "LastName must be between 3 and 100 characters.")]
    public string LastName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Invalid phone number format.")]
    public string PhoneNumber { get; set; }
        
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; }
}