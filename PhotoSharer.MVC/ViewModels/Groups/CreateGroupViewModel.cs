﻿using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoSharer.MVC.ViewModels.Groups
{
    public class CreateGroupViewModel
    {
        [Required]
        [Display(Name = "Group name")]
        [StringLength(100, ErrorMessage = "The maximum length is {1}!")]
        [RegularExpression(@"(\w|\d| )+", ErrorMessage = "{0} can contains only letters and digits!")]
        public string Name { get; set; }
    }
}