using System;

namespace PhotoSharer.MVC.ViewModels.Groups
{
    public class GroupListPageItemViewModel
    {
        public string Name { get; set; }    
        public string Link { get; set; }
        public Guid CreatorId { get; set; }
    }
}