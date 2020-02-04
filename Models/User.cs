using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginReg.Models
{
    public class User
    {
        [Key]
        public int UserId {get; set;}

        [Required(ErrorMessage="Please provide your first name.")]
        [MinLength(2, ErrorMessage="Your first name must be at least two characters.")]
        [Display(Name="First Name: ")]
        public string FirstName {get; set;}


        [Required(ErrorMessage="Please provide a last name.")]
        [MinLength(2, ErrorMessage="Your last name must be at least two characters.")]
        [Display(Name="Last Name: ")]
        public string LastName {get; set;}


        [Required(ErrorMessage="Email is required.")]
        [EmailAddress]
        [Display(Name="Email: ")]
        public string Email {get; set;}


        [Required(ErrorMessage="You must create a password.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Your password must be at least 8 characters.")]
        [Display(Name="Password: ")]
        public string Password {get; set;}


        [Required(ErrorMessage="Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage="Those don't match.")]
        [Display(Name="Confirm Password: ")]
        [NotMapped]
        public string ConfirmPassword {get; set;}

        public DateTime CreatedAt {get; set;} = DateTime.Now;

        public DateTime UpdatedAt {get; set;} = DateTime.Now;
        
    }
}