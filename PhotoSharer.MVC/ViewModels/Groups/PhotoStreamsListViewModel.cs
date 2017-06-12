using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSharer.MVC.ViewModels.Groups
{
    public class PhotoStreamsListViewModel
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public IList<PhotoStreamViewModel> PhotoStreams { get; set; }
    }
}
