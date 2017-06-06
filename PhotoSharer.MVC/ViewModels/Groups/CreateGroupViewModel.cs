using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoSharer.MVC.ViewModels.Groups
{
    public class CreateGroupViewModel
    {
        [Required]
        [Display(Name = "Group name")]
        [StringLength(100, ErrorMessage = "The maximum length is {1}")]
        public string Name { get; set; }
    }
}