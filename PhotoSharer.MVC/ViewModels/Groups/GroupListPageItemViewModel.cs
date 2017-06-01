using System;

namespace PhotoSharer.MVC.ViewModels.Groups
{
    public class GroupListPageItemViewModel
    {
        public string Name { get; set; }    
        public string Url { get; set; }
        public Guid CreatorId { get; set; }
        public Guid CurrentUserId { get; set; }
    }
}