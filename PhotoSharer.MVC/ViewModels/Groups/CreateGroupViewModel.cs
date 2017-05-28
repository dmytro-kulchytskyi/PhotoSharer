using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoSharer.MVC.ViewModels.Groups
{
    public class CreateGroupViewModel
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }


        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Display(Name = "Имя Группы")]
        [StringLength(255, ErrorMessage = "Максимальная длинна имени - 255 символов")]
        public string Name { get; set; }

        [ScaffoldColumn(false)]
        public string InviteCode { get; set; }

        [ScaffoldColumn(false)]
        public Guid Creator { get; set; }
    }
}