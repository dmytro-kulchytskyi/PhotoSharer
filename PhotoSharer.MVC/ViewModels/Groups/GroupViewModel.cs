using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSharer.MVC.ViewModels.Groups
{
    public class GroupViewModel
    {
        public Guid Id { get; set; }
        public bool IsMember { get; set; }
        public bool IsCreator { get; set; }
        public string Name { get; set; }
    }
}
