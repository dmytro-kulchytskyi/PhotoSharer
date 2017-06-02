using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoSharer.MVC.ViewModels.Groups
{
    public class CreateGroupViewModel
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Group name")]
        [StringLength(255, ErrorMessage = "The maximum length is {1}")]
        public string Name { get; set; }

        [ScaffoldColumn(false)]
        public string InviteCode { get; set; }

        [ScaffoldColumn(false)]
        public Guid Creator { get; set; }
    }
}