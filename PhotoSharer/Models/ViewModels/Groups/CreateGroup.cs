using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace PhotoSharer.Models.ViewModels
{
    public class CreateGroup
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }


        [Required(ErrorMessage = "Поле должно быть установлено")]
        [Display(Name = "Имя Группы")]
        [StringLength(50, ErrorMessage = "Максимальная длинна имени - 50 символов")]
        public string Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string InviteCode { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Guid CreatorId { get; set; }
    }
}