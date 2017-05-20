using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PhotoSharer.Models.ViewModels
{
    public class CreateGroup
    {
        public int Id { get; set; }

        [Display(Name = "Имя Группы")]
        [StringLength(50, ErrorMessage = "Максимальная длинна имени - 50 символов")]
        public string Title { get; set; }
    }
}