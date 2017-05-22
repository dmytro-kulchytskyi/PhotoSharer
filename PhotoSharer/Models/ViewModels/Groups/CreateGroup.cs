using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PhotoSharer.Models.ViewModels
{
    public class CreateGroup
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }


        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Display(Name = "Имя Группы")]
        [StringLength(255, ErrorMessage = "Максимальная длинна имени - 255 символов")]
        public string Name { get; set; }

        [ScaffoldColumn(false)]
        public string InviteCode { get; set; }

        [ScaffoldColumn(false)]
        public Guid CreatorId { get; set; }
    }
}