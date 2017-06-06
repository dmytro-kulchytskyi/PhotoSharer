using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSharer.MVC.ViewModels.Groups
{
    public class CreatePhotoStremViewModel
    {
        [Required]
        public Guid GroupId { get; set; }

        [Required]
        public string Provider { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string Url { get; set; }
    }
}
